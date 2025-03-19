using System.ComponentModel.DataAnnotations;

namespace GotorzProject.Model.Auth
{
    public class CustomToken
    {
        [Key]
        public string? Key { get; set; }

        public Customer? Assignee { get; set; }
        public DateTime? Expires { get; set; }
        public DateTime? Created { get; set; }
    }
}
