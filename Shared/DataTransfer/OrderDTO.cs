using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotorzProject.Shared.DataTransfer
{
    public class OrderDTO
    {

        public enum OrderStatuses
        {
            Created,
            Processing,
            Finalized
        }

        public int ID { get; set; }
        public string? Date { get; set; }
        public string? DateYear { get; set; }
        public string? Destination { get; set; }
        public string? Hotel { get; set; }
        public double Price { get; set; }
        public string? PaymentMethod { get; set; }
        public OrderStatuses OrderStatus { get; set; }
    }
}
