using Syncfusion.Blazor.Notifications;
using System.ComponentModel.DataAnnotations;

namespace GotorzProject.Model
{
    public class LoggedEvent
    {
        [Key]
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        [Required]
        public LogLevel LogLevel { get; set; }

        public string Category { get; set; }

        public string Message { get; set; }

        public string? Exception { get; set; }
    }

}
