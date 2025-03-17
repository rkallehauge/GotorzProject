using System.ComponentModel.DataAnnotations;

namespace GotorzProject.Model
{
    public class Hotel
    {
        //todo: switch with paymentmodel
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Stars { get; set; }
        public int Price { get; set; }
        public int NumberOfRooms { get; set; }

        public string? Image {  get; set; }

        public Hotel(string name, string address, int stars, int price, int numberOfRooms)
        {
            Name = name;
            Address = address;
            Stars = stars;
            Price = price;
            NumberOfRooms = numberOfRooms;
        }
    }
}
