using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotorzProject.Shared.DataTransfer
{
    public class UserDTO
    {

        // TODO : TBD : implement this when identityuser is setup with these

        

        [Required]
        [Display(Name ="First name")]

        public string? FirstName { get; set; }

        [Required]
        [Display(Name ="Last name")]
        public string? LastName { get; set; }

        
        public string? Address { get; set; }
        public string? Country { get; set; }

        public string? TelephoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; private set; } // email is only unique identifier here, so don't change it

        [MustHaveOneElement]
        public List<string>? Roles { get; set; }


        public string? ProfilePicture { get; set; }

        public UserDTO(string firstName, string lastName, string address, string country, string telephonePhone, string email, List<string> roles, string profilePicture)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            Country = country;
            TelephoneNumber = telephonePhone;
            Email = email;
            Roles = roles;
            ProfilePicture = profilePicture;
        }

        public UserDTO() { }
    }
}
