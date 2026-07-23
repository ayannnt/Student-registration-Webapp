using System.ComponentModel.DataAnnotations;

namespace EventManagementWebApp.Models
{
    public class EventCreateEditViewModel
    {
        public int EventId { get; set; }

        [Required]
        public string EventName { get; set; }

        [Required]
        public string EventLocation { get; set; }
    }
}
