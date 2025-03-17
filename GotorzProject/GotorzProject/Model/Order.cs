using System.ComponentModel.DataAnnotations;

namespace GotorzProject.Model
{
    public class Order
    {

        [Key]
        public int Id { get; set; }

        public enum OrderStatuses
        {
            Created,
            Processing,
            Finalized
        }
        public string PaymentMethod {  get; set; }
        public double Price { get; set; }
        public OrderStatuses OrderStatus { get; set; }

        public Order(string paymentMethod, double price, OrderStatuses orderStatus)
        {
            PaymentMethod = paymentMethod;
            Price = price;
            OrderStatus = orderStatus;
        }
    }
}
