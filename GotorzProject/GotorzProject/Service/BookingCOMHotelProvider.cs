﻿using GotorzProject.Service.Model.Hotel;
using GotorzProject.Service.Model.Location;
using GotorzProject.Shared;
using GotorzProject.Shared.DataTransfer;
//using Newtonsoft.Json;
using System.Numerics;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace GotorzProject.Service
{
    public class BookingCOMHotelProvider : IHotelProvider
    {
        string dateFormat = "yyyy-MM-dd";
        HttpClient _httpClient;

        private readonly string apiBase;


        public BookingCOMHotelProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BookingCOM");
            apiBase = "/api/v1/hotels/";
        }


        // get hotels from generic search query
        public async Task<IEnumerable<BaseHotelRoomDTO>> GetHotels(string location, DateOnly checkin, DateOnly checkout)
        {
            if(checkin >= checkout)
            {
                throw new ArgumentException("Check in cannot be equal to or greather than checkout.");
            }

            string locationSearchEndpoint = apiBase + "searchDestination";
            string hotelSearchEndpoint = apiBase + "searchHotels";

            string locationQuery = $"{locationSearchEndpoint}?query={location}";
            Console.WriteLine(locationQuery);

            var locationResponse = await _httpClient.GetAsync(locationQuery);
            locationResponse.EnsureSuccessStatusCode();

            Console.WriteLine("Location Search");
            Console.WriteLine(await locationResponse.Content.ReadAsStringAsync());

            LocationSearchModel? lsm = await locationResponse.Content.ReadFromJsonAsync<LocationSearchModel>();

            if (lsm == null || lsm.Status != true || lsm.Data.Count == 0)
            {
                //if (lsm == null) Console.WriteLine("lsm was null");
                //if (lsm.Status != true) Console.WriteLine("lsm was false");
                //if (lsm.Data.Count <= 0) Console.WriteLine($"Count was {lsm.Data.Count}");
                return null;
            }

            string destId = lsm.Data.First().DestId;
            string searchType = lsm.Data.First().SearchType;

            string checkIn = checkin.ToString(dateFormat);
            string checkOut = checkout.ToString(dateFormat);


            Dictionary<string, string> param = new Dictionary<string, string>()
            {
                {"dest_id", destId},
                {"search_type", searchType },

                { "arrival_date", checkIn },
                { "departure_date", checkOut}
            };

            var hotelQuery = hotelSearchEndpoint + Helper.ToQueryString(param);


            Console.WriteLine(hotelQuery);
            //return null;
            //Console.WriteLine(hotelQuery);

            var hotelResponse = await _httpClient.GetAsync(hotelQuery);
            hotelResponse.EnsureSuccessStatusCode();


            Console.WriteLine("Hotel Search Model");
            Console.WriteLine(await hotelResponse.Content.ReadAsStringAsync());

            var hotels = await hotelResponse.Content.ReadFromJsonAsync<HotelSearchModel>();

            if(hotels.data.hotels.Length == 0)
            {
                // no hotels found
                return null;
            }

            List<BaseHotelRoomDTO> result = new();

            Regex bedPattern = new Regex(@"(?:Hotel room|Room with shared bathroom|Private suite|Shared dorm room|Entire apartment)\s*[:-]\s*\d+\s*beds?");


            foreach (var hotel in hotels.data.hotels)
            {
                int dates = DateOnly.Parse(hotel.property.checkoutDate).DayNumber - DateOnly.Parse(hotel.property.checkinDate).DayNumber;

                result.Add(new BaseHotelRoomDTO()
                {
                    // ill leave this here to show how dumb the fucking api is
                    //Beds = hotel.accessibilityLabel.Split("\n")[3].Split(":")[1],
                    Beds = bedPattern.Match(hotel.accessibilityLabel).Value,
                    TotalPrice = new()
                    {
                        Currency = hotel.property.priceBreakdown.grossPrice.currency,
                        Value = hotel.property.priceBreakdown.grossPrice.value,
                    },
                    PricePerNight = new()
                    {
                        Currency = hotel.property.priceBreakdown.grossPrice.currency,
                        Value = (hotel.property.priceBreakdown.grossPrice.value / dates)
                    },

                    Rating = hotel.property.reviewScore,
                    RatingDesc = hotel.property.reviewScoreWord,
                    RatingAmount = hotel.property.reviewCount,

                    //Location = 
                    ImageSource = hotel.property.photoUrls.First(),
                    Name = hotel.property.name,
                });
            }


            return result;
        }

        public Task<IEnumerable<BaseHotelRoomDTO>> GetHotels(int destinationId, DateOnly checkin, DateOnly checkout)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<BaseHotelRoomDTO>> GetHotels(string city, string country, DateOnly checkin, DateOnly checkout)
        {
            throw new NotImplementedException();
        }
    }




}
