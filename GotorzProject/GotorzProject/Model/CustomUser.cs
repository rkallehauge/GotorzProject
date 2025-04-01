using Microsoft.AspNetCore.Identity;

namespace GotorzProject.Model
{
    public class CustomUser : IdentityUser
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? TelephoneNumber { get; set; }
        public string? ProfilePictureSrc { get; set; }
  
    }
}
