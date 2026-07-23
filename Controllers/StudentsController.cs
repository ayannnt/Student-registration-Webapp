using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRegistrationWebApp.Data;
using StudentRegistrationWebApp.Models;

namespace StudentRegistrationWebApp.Controllers
{
    // ═══ Admin only — View all registered students ═══
    [Authorize(Roles = "Admin")]
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Students — Admin only
        public async Task<IActionResult> Index()
        {
            // Get all users in Student role
            var students = await _userManager.GetUsersInRoleAsync("Student");

            // Include their course registrations
            var studentIds = students.Select(s => s.Id).ToList();
            var registrations = await _context.CourseRegistrations
                .Include(r => r.Course)
                .Where(r => studentIds.Contains(r.StudentId))
                .ToListAsync();

            var studentList = students.Select(s => new StudentViewModel
            {
                Id = s.Id,
                FullName = s.FullName,
                Email = s.Email ?? "",
                PhoneNumber = s.PhoneNumber ?? "",
                RegisteredOn = s.RegisteredOn,
                RegisteredCourse = registrations.FirstOrDefault(r => r.StudentId == s.Id)?.Course?.CourseName ?? "Not Registered"
            }).ToList();

            return View(studentList);
        }
    }

    public class StudentViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime RegisteredOn { get; set; }
        public string RegisteredCourse { get; set; } = string.Empty;
    }
}
