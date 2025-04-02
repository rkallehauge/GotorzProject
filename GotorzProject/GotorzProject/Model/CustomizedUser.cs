using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GotorzProject.Model
{

    // TODO Implement
    public class CustomizedUser : IdentityUser
    {

        [PersonalData]
        [Required]
        public string? FirstName { get; set; }

        [PersonalData]
        public string? LastName { get; set; }

    }
}
