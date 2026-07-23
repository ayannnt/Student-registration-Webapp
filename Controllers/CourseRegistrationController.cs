using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRegistrationWebApp.Data;
using StudentRegistrationWebApp.Models;

namespace StudentRegistrationWebApp.Controllers
{
    // ═══ Course Registration — Student can register for ONE course only ═══
    [Authorize(Roles = "Student")]
    public class CourseRegistrationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseRegistrationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: CourseRegistration/Register — Show available courses for registration
        public async Task<IActionResult> Register()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            // Check if student already registered for a course
            var existingRegistration = await _context.CourseRegistrations
                .Include(r => r.Course)
                .FirstOrDefaultAsync(r => r.StudentId == user.Id);

            if (existingRegistration != null)
            {
                TempData["InfoMessage"] = $"You have already registered for '{existingRegistration.Course?.CourseName}'. Students can register for only one course.";
                return RedirectToAction("Index", "Profile");
            }

            // Show available courses
            var courses = await _context.Courses.ToListAsync();
            return View(courses);
        }

        // POST: CourseRegistration/Register/5 — Register for a specific course
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int courseId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            // Double-check student hasn't already registered
            var existing = await _context.CourseRegistrations
                .FirstOrDefaultAsync(r => r.StudentId == user.Id);

            if (existing != null)
            {
                TempData["ErrorMessage"] = "You have already registered for a course. Students can only register for one course.";
                return RedirectToAction("Index", "Profile");
            }

            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                TempData["ErrorMessage"] = "Course not found.";
                return RedirectToAction(nameof(Register));
            }

            var registration = new CourseRegistration
            {
                StudentId = user.Id,
                CourseId = courseId,
                RegistrationDate = DateTime.Now
            };

            _context.CourseRegistrations.Add(registration);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Successfully registered for '{course.CourseName}' ({course.CourseCode})!";
            return RedirectToAction("Index", "Profile");
        }

        // POST: CourseRegistration/Drop — Drop the registered course
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Drop()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var registration = await _context.CourseRegistrations
                .FirstOrDefaultAsync(r => r.StudentId == user.Id);

            if (registration != null)
            {
                var course = await _context.Courses.FindAsync(registration.CourseId);
                _context.CourseRegistrations.Remove(registration);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Dropped course '{course?.CourseName}'. You can now register for a different course.";
            }

            return RedirectToAction(nameof(Register));
        }
    }
}
