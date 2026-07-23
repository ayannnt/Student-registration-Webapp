using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagementWebApp.Models
{
    public class EventInfo
    {
        [Key]
        public int EventId { get; set; }

        public string EventName { get; set; }

        public string EventLocation { get; set; }

        // One-to-Many relationship
        // Navigation Property to retrieve List of Participants
        public virtual List<Participant> Participants { get; set; }
    }
}
