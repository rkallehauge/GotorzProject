namespace GotorzProject.Model
{
    public class Employee : Customer
    {
        public string Role { get; set; }

        public Employee(string firstName, string lastName, string address, string postalCode, string country, string telephoneNumber, int iD, string role) : base(firstName, lastName, address, postalCode, country, telephoneNumber, iD)
        {
            Role = role;
        }

        public Employee() { }
    }
}
