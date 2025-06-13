using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotorzProject.Shared.DataTransfer
{
    public class TravelPackageDTO
    {
        public string DestinationCity { get; set; }
        public string DestinationCountry { get; set; }
        public string Hotel { get; set; }
        public double Price { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
