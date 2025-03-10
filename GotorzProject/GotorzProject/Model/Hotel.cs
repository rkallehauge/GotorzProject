namespace GotorzProject.Model
{
    public class Hotel
    {
        //todo: switch with paymentmodel

        public string Name { get; set; }
        public string Adress { get; set; }
        public int Stars { get; set; }
        public int Price { get; set; }
        public int NumberOfRooms { get; set; }

        public Hotel(string name, string adress, int stars, int price, int numberOfRooms)
        {
            Name = name;
            Adress = adress;
            Stars = stars;
            Price = price;
            NumberOfRooms = numberOfRooms;
        }
    }
}
