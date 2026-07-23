using System.ComponentModel.DataAnnotations;

namespace StudentRegistrationWebApp.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Course name is required.")]
        [StringLength(100, ErrorMessage = "Course name cannot exceed 100 characters.")]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course code is required.")]
        [StringLength(20, ErrorMessage = "Course code cannot exceed 20 characters.")]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Credits are required.")]
        [Range(1, 10, ErrorMessage = "Credits must be between 1 and 10.")]
        public int Credits { get; set; }

        [Display(Name = "Instructor")]
        public string Instructor { get; set; } = string.Empty;

        public ICollection<CourseRegistration>? Registrations { get; set; }
    }
}
