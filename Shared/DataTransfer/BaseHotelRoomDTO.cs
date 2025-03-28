using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotorzProject.Shared.DataTransfer
{
    public class BaseHotelRoomDTO
    {
        public string Name { get; set; }

        // Commented out until an actual way to get these is possible
        //public List<string> Amenities { get; set; }

        public string Beds { get; set; }

        public Price TotalPrice { get; set; }
        public Price PricePerNight { get; set; }

        public double Rating {  get; set; }
        public string RatingDesc {  get; set; }
        public int RatingAmount { get; set; }


        // no actual way to get this without calling hoteldetails for EACH and every one
        //public string Location { get; set; }

        public string ImageSource { get; set; }
    }
}
