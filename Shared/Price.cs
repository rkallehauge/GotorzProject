using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotorzProject.Shared
{
    public class Price
    {
        public double Value { get; set; }
        public string Currency { get; set; }
        public string Print()
        {
            return $"{Value} {Currency}";
        }
    }
}
