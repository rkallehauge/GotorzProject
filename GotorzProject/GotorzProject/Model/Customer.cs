using System.ComponentModel.DataAnnotations;

namespace GotorzProject.Model
{
    public class Customer
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? TelephoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }


        public string? Password { get; set; }

        [Key]
        public int? ID { get; set; } // todo: switch to unique username and password

        public Customer(string firstName, string lastName, string address, string postalCode, string country, string telephoneNumber, int iD)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            PostalCode = postalCode;
            Country = country;
            TelephoneNumber = telephoneNumber;
            ID = iD;
        }

        public Customer() { }

    }
}
