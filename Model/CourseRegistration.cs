using System.ComponentModel.DataAnnotations;

namespace StudentRegistrationWebApp.Models
{
    public class CourseRegistration
    {
        [Key]
        public int RegistrationId { get; set; }

        [Required]
        public string StudentId { get; set; } = string.Empty;

        [Required]
        public int CourseId { get; set; }

        [Display(Name = "Registration Date")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Navigation properties
        public ApplicationUser? Student { get; set; }
        public Course? Course { get; set; }
    }
}
