using System.ComponentModel.DataAnnotations;

namespace EventManagementWebApp.Models
{
    public class ParticipantCreateEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Range(18, 100)]
        public int Age { get; set; }

        [Required]
        public int EventId { get; set; }

        // EventName is optional by specifying string?
        public string? EventName { get; set; }
    }
}
