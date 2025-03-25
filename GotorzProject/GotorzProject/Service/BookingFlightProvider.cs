using GotorzProject.Model;
using Newtonsoft.Json;

using GotorzProject.Shared;
using System.Configuration;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using GotorzProject.Service.Misc;
using Microsoft.Extensions.Options;
using GotorzProject.Shared.DataTransfer;
using GotorzProject.Service.Model;


namespace GotorzProject.Service
{
    public class BookingFlightProvider : IBookingFlightProvider
    {


        HttpClient _httpClient;

        string _apiKey;
        string _hostBase;

        // todo : Dynamically set in future
        private readonly string apiBase;


        public BookingFlightProvider(IHttpClientFactory factory, IOptions<BookingAPIModel> apiModel)
        {
            _httpClient = factory.CreateClient("BookingCOM");

            //somewhat disgusting
            apiBase = "/api/v1/flights/";
        }

        public async Task<List<BaseFlightDTO>> GetFlights(string from, string to, DateOnly departureDate, DateOnly? returnDate = null)
        {
           

            string airportSearchEndpoint = apiBase + "searchDestination";
            string flightSearchEndpoint = apiBase + "searchFlights";

            string airportFrom, airportTo, query;

            const string dateFormat = "yyyy-MM-dd";

            // firstly we need the airport code for the [from] part of this method
            query = airportSearchEndpoint + $"?query={from}";

            // first result best result 😎 (await (await _httpClient.GetAsync(query)).Content.ReadFromJsonAsync<AirportSearchResponse>()).Data[0].Code;
            var fromResponse = await _httpClient.GetAsync(query);


            fromResponse.EnsureSuccessStatusCode();
            var fromContent = await fromResponse.Content.ReadFromJsonAsync<AirportSearchResponse>();
            fromResponse.Dispose();


            if(fromContent == null || fromContent.Data.Count == 0 || fromContent.Status != true)
            {
                // log instead?
                Console.WriteLine("exit a");
                throw new Exception($"No airport found from : {from}");
            }

            airportFrom = fromContent.Data.First().Id;

            query = airportSearchEndpoint + $"?query={to}";

            var toResponse = await _httpClient.GetAsync(query);

            toResponse.EnsureSuccessStatusCode();

            var toContent = await toResponse.Content.ReadFromJsonAsync<AirportSearchResponse>();
            toResponse.Dispose();

            if (toContent == null || toContent.Data.Count == 0 || toContent.Status != true)
            {
                Console.WriteLine("exit b");
                throw new Exception($"No airport found from : {to}");
            }

            airportTo = toContent.Data.First().Id;

                
            string stringDeparture;

            stringDeparture = departureDate.ToString(dateFormat);

            var parameters = new Dictionary<string, string>
            {
                {"fromId", airportFrom},
                {"toId", airportTo},
                {"departDate", stringDeparture}
            };

            if (returnDate != null)
            {
                string returnString = returnDate?.ToString(dateFormat);
                parameters.Add("returnDate", returnString);
            }


            foreach(var parameter in parameters)
            {
                Console.WriteLine($"{parameter.Key} : {parameter.Value}");
            }

            query = flightSearchEndpoint + Helper.ToQueryString(parameters);
            var flightSearchResponse = await _httpClient.GetAsync(query);
            flightSearchResponse.EnsureSuccessStatusCode();

            var jsonParse = await flightSearchResponse.Content.ReadAsStringAsync();
            //Console.WriteLine(jsonParse);

            FlightSearchModel fsr = await flightSearchResponse.Content.ReadFromJsonAsync<FlightSearchModel>();

            if(fsr == null)
            {
                // log error ?
                return null;
            }

            return fsr.ToBaseFlightDTO();     
        }
        
        public Task<List<BaseFlightDTO>> GetFlights(string from, string to, DateOnly departureDate)
        {
            return GetFlights(from, to, departureDate, null);
        }
    }

    public class AirportSearchResponse
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("data")]
        public List<LocationData> Data { get; set; }
    }

    public class LocationData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("regionName")]
        public string RegionName { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("countryName")]
        public string CountryName { get; set; }

        [JsonProperty("countryNameShort")]
        public string CountryNameShort { get; set; }

        [JsonProperty("photoUri")]
        public string PhotoUri { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("cityName")]
        public string CityName { get; set; }

        [JsonProperty("distanceToCity")]
        public Distance DistanceToCity { get; set; }

        [JsonProperty("parent")]
        public string Parent { get; set; }
    }

    public class Distance
    {
        [JsonProperty("value")]
        public double Value { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }
    }

    public class Aggregation
    {
        public int totalCount { get; set; }
        public int filteredTotalCount { get; set; }
        public List<Stop> stops { get; set; }
        public List<Airline> airlines { get; set; }
        public List<DepartureInterval> departureIntervals { get; set; }
        public List<FlightTime> flightTimes { get; set; }
        public ShortLayoverConnection shortLayoverConnection { get; set; }
        public int durationMin { get; set; }
        public int durationMax { get; set; }
        public MinPrice minPrice { get; set; }
        public MinRoundPrice minRoundPrice { get; set; }
        public MinPriceFiltered minPriceFiltered { get; set; }
        public List<Baggage> baggage { get; set; }
        public Budget budget { get; set; }
        public BudgetPerAdult budgetPerAdult { get; set; }
        public List<Duration> duration { get; set; }
        public List<string> filtersOrder { get; set; }
    }

    public class Airline
    {
        public string name { get; set; }
        public string logoUrl { get; set; }
        public string iataCode { get; set; }
        public int count { get; set; }
        public MinPrice minPrice { get; set; }
    }

    public class Ancillaries
    {
        public FlexibleTicket flexibleTicket { get; set; }
    }

    public class Arrival
    {
        public string start { get; set; }
        public string end { get; set; }
        public int count { get; set; }
    }

    public class ArrivalAirport
    {
        public string type { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string cityName { get; set; }
        public string country { get; set; }
        public string countryName { get; set; }
        public string province { get; set; }
    }

    public class AvgPerAdult
    {
        public string currencyCode { get; set; }
        public int units { get; set; }
        public int nanos { get; set; }
    }

    public class AvgPerChild
    {
        public string currencyCode { get; set; }
        public int units { get; set; }
        public int nanos { get; set; }
    }

    public class Baggage
    {
        public string paramName { get; set; }
        public int count { get; set; }
        public bool enabled { get; set; }
        public string baggageType { get; set; }
    }

    public class BaggagePolicy
    {
        public string code { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class BaseFare
    {
        public string currencyCode { get; set; }
        public int units { get; set; }
        public int nanos { get; set; }
    }

    public class Budget
    {
        public string paramName { get; set; }
        public Min min { get; set; }
        public Max max { get; set; }
    }

    public class BudgetPerAdult
    {
        public string paramName { get; set; }
        public Min min { get; set; }
        public Max max { get; set; }
    }

    public class CabinClassExtension
    {
    }

    public class Carrier
    {
        public string name { get; set; }
        public string code { get; set; }
        public string logo { get; set; }
    }

    public class CarrierInfo
    {
        public string operatingCarrier { get; set; }
        public string marketingCarrier { get; set; }
        public string operatingCarrierDisclosureText { get; set; }
    }

    public class CarriersDatum
    {
        public string name { get; set; }
        public string code { get; set; }
        public string logo { get; set; }
    }

    public class CarrierTaxBreakdown
    {
        public Carrier carrier { get; set; }
        public AvgPerAdult avgPerAdult { get; set; }
        public AvgPerChild avgPerChild { get; set; }
    }

    public class Data
    {
        public Aggregation aggregation { get; set; }
        public List<FlightOffer> flightOffers { get; set; }
        public List<FlightDeal> flightDeals { get; set; }
        public string atolProtectedStatus { get; set; }
        public string searchId { get; set; }
        public List<object> banners { get; set; }
        public DisplayOptions displayOptions { get; set; }
        public bool isOffersCabinClassExtended { get; set; }
        public CabinClassExtension cabinClassExtension { get; set; }
        public SearchCriteria searchCriteria { get; set; }
        public List<BaggagePolicy> baggagePolicies { get; set; }
        public PriceAlertStatus priceAlertStatus { get; set; }
    }

    public class Departure
    {
        public string start { get; set; }
        public string end { get; set; }
        public int count { get; set; }
    }

    public class DepartureAirport
    {
        public string type { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string cityName { get; set; }
        public string country { get; set; }
        public string countryName { get; set; }
        public string province { get; set; }
    }

    public class DepartureInterval
    {
        public string start { get; set; }
        public string end { get; set; }
    }

    public class Discount
    {
        public string currencyCode { get; set; }
        public int units { get; set; }
        public int nanos { get; set; }
    }

    public class DisplayOptions
    {
        public bool brandedFaresShownByDefault { get; set; }
        public bool directFlightsOnlyFilterIgnored { get; set; }
        public bool hideFlexiblePricesBanner { get; set; }
    }

    public class Duration
    {
        public int min { get; set; }
        public int max { get; set; }
        public string durationType { get; set; }
        public bool enabled { get; set; }
        public string paramName { get; set; }
    }

    public class ExtraProduct
    {
        public string type { get; set; }
        public PriceBreakdown priceBreakdown { get; set; }
    }

    public class ExtraProductDisplayRequirements
    {
    }

    public class Fee
    {
        public string currencyCode { get; set; }
        public int units { get; set; }
        public int nanos { get; set; }
    }

    public class FlexibleTicket
    {
        public string airProductReference { get; set; }
        public List<string> travellers { get; set; }
        public Recommendation recommendation { get; set; }
        public PriceBreakdown priceBreakdown { get; set; }
        public SupplierInfo supplierInfo { get; set; }
        public bool preSelected { get; set; }
    }

    public class FlightDeal
    {
        public string key { get; set; }
        public string offerToken { get; set; }
        public Price price { get; set; }
        public PriceRounded priceRounded { get; set; }
        public List<TravellerPrice> travellerPrices { get; set; }
    }

    public class FlightInfo
    {
        public List<object> facilities { get; set; }
        public int flightNumber { get; set; }
        public string planeType { get; set; }
        public CarrierInfo carrierInfo { get; set; }
    }

    public class FlightOffer
    {
        public string token { get; set; }
        public List<Segment> segments { get; set; }
        public PriceBreakdown priceBreakdown { get; set; }
        public List<TravellerPrice> travellerPrices { get; set; }
        public List<object> priceDisplayRequirements { get; set; }
        public string pointOfSale { get; set; }
        public string tripType { get; set; }
        public PosMismatch posMismatch { get; set; }
        public List<List<string>> includedProductsBySegment { get; set; }
        public IncludedProducts includedProducts { get; set; }
        public List<ExtraProduct> extraProducts { get; set; }
        public OfferExtras offerExtras { get; set; }
        public Ancillaries ancillaries { get; set; }
        public List<object> appliedDiscounts { get; set; }
        public string offerKeyToHighlight { get; set; }
        public ExtraProductDisplayRequirements extraProductDisplayRequirements { get; set; }
        public UnifiedPriceBreakdown unifiedPriceBreakdown { get; set; }
    }

    public class FlightTime
    {
        public List<Arrival> arrival { get; set; }
        public List<Departure> departure { get; set; }
    }

    public class IncludedProducts
    {
        public bool areAllSegmentsIdentical { get; set; }
        public List<List<string>> segments { get; set; }
    }

    public class Item
    {
        public string scope { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public Price price { get; set; }
        public List<Item> items { get; set; }
    }

    public class Leg
    {
        public DateTime departureTime { get; set; }
        public DateTime arrivalTime { get; set; }
        public DepartureAirport departureAirport { get; set; }
        public ArrivalAirport arrivalAirport { get; set; }
        public string cabinClass { get; set; }
        public FlightInfo flightInfo { get; set; }
        public List<string> carriers { get; set; }
        public List<CarriersDatum> carriersData { get; set; }
        public int totalTime { get; set; }
        public List<object> flightStops { get; set; }
        public List<object> amenities { get; set; }
    }

    public class LuggageAllowance
    {
        public string luggageType { get; set; }
        public string ruleType { get; set; }
        public int maxPiece { get; set; }
        public double maxWeightPerPiece { get; set; }
        public string massUnit { get; set; }
        public SizeRestrictions sizeRestrictions { get; set; }
    }

    public class Max
    {
        public string currencyCode { get; set; }
        public int units { get; set; }
        public int nanos { get; set; }
    }

    public class Min
    {
        public string currencyCode { get; set; }
        public int units { get; set; }
        public int nanos { get; set; }
    }

    public class MinPrice
    {
        public string currencyCode { get; set; }
        public int units { get; set; }
        public int nanos { get; set; }
    }

    public class MinPriceFiltered
    {
        public string currencyCode { get; set; }
        public int units { get; set; }
        public int nanos { get; set; }
    }

    public class MinPriceRound
    {
        public string currencyCode { get; set; }
        public int units { get; set; }
        public int nanos { get; set; }
    }

    public class MinRoundPrice
    {
        public string currencyCode { get; set; }
        public int units { get; set; }
        public int nanos { get; set; }
    }

    public class OfferExtras
    {
        public FlexibleTicket flexibleTicket { get; set; }
    }

    public class PosMismatch
    {
        public string detectedPointOfSale { get; set; }
        public bool isPOSMismatch { get; set; }
        public string offerSalesCountry { get; set; }
    }

    public class Price
    {
        public string currencyCode { get; set; }
        public int units { get; set; }
        public int nanos { get; set; }
    }

    public class PriceAlertStatus
    {
        public bool isEligible { get; set; }
        public bool isSearchEligible { get; set; }
        public bool isBlockoutEligible { get; set; }
    }

    public class PriceBreakdown
    {
        public Total total { get; set; }
        public BaseFare baseFare { get; set; }
        public Fee fee { get; set; }
        public Tax tax { get; set; }
        public TotalRounded totalRounded { get; set; }
        public Discount discount { get; set; }
        public TotalWithoutDiscount totalWithoutDiscount { get; set; }
        public TotalWithoutDiscountRounded totalWithoutDiscountRounded { get; set; }
        public List<CarrierTaxBreakdown> carrierTaxBreakdown { get; set; }
    }

    public class PriceRounded
    {
        public string currencyCode { get; set; }
        public int units { get; set; }
        public int nanos { get; set; }
    }

    public class Recommendation
    {
        public bool recommended { get; set; }
        public string confidence { get; set; }
    }

    public class FlightSearchResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public long timestamp { get; set; }
        public Data data { get; set; }
    }

    public class SearchCriteria
    {
        public string cabinClass { get; set; }
    }

    public class Segment
    {
        public DepartureAirport departureAirport { get; set; }
        public ArrivalAirport arrivalAirport { get; set; }
        public DateTime departureTime { get; set; }
        public DateTime arrivalTime { get; set; }
        public List<Leg> legs { get; set; }
        public int totalTime { get; set; }
        public List<TravellerCheckedLuggage> travellerCheckedLuggage { get; set; }
        public List<TravellerCabinLuggage> travellerCabinLuggage { get; set; }
        public bool isAtolProtected { get; set; }
        public bool showWarningDestinationAirport { get; set; }
        public bool showWarningOriginAirport { get; set; }
    }

    public class ShortLayoverConnection
    {
        public int count { get; set; }
    }

    public class SizeRestrictions
    {
        public double maxLength { get; set; }
        public double maxWidth { get; set; }
        public double maxHeight { get; set; }
        public string sizeUnit { get; set; }
    }

    public class Stop
    {
        public int numberOfStops { get; set; }
        public int count { get; set; }
        public MinPrice minPrice { get; set; }
        public MinPriceRound minPriceRound { get; set; }
    }

    public class SupplierInfo
    {
        public string name { get; set; }
        public string termsUrl { get; set; }
        public string privacyPolicyUrl { get; set; }
    }

    public class Tax
    {
        public string currencyCode { get; set; }
        public int units { get; set; }
        public int nanos { get; set; }
    }

    public class Total
    {
        public string currencyCode { get; set; }
        public int units { get; set; }
        public int nanos { get; set; }
    }

    public class TotalRounded
    {
        public string currencyCode { get; set; }
        public int nanos { get; set; }
        public int units { get; set; }
    }

    public class TotalWithoutDiscount
    {
        public string currencyCode { get; set; }
        public int units { get; set; }
        public int nanos { get; set; }
    }

    public class TotalWithoutDiscountRounded
    {
        public string currencyCode { get; set; }
        public int nanos { get; set; }
        public int units { get; set; }
    }

    public class TravellerCabinLuggage
    {
        public string travellerReference { get; set; }
        public LuggageAllowance luggageAllowance { get; set; }
        public bool personalItem { get; set; }
    }

    public class TravellerCheckedLuggage
    {
        public string travellerReference { get; set; }
        public LuggageAllowance luggageAllowance { get; set; }
    }

    public class TravellerPrice
    {
        public TravellerPriceBreakdown travellerPriceBreakdown { get; set; }
        public string travellerReference { get; set; }
        public string travellerType { get; set; }
    }

    public class TravellerPriceBreakdown
    {
        public Total total { get; set; }
        public BaseFare baseFare { get; set; }
        public Fee fee { get; set; }
        public Tax tax { get; set; }
        public TotalRounded totalRounded { get; set; }
        public Discount discount { get; set; }
        public TotalWithoutDiscount totalWithoutDiscount { get; set; }
        public TotalWithoutDiscountRounded totalWithoutDiscountRounded { get; set; }
    }

    public class UnifiedPriceBreakdown
    {
        public Price price { get; set; }
        public List<Item> items { get; set; }   
        public List<object> addedItems { get; set; }
    }

}