using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentRegistrationWebApp.Models;

namespace StudentRegistrationWebApp.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // ═══ Ensure database is created ═══
            await context.Database.MigrateAsync();

            // ═══ Create Roles ═══
            string[] roles = { "Admin", "Student" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // ═══ Create Administrator Account ═══
            string adminEmail = "admin@studentapp.com";
            string adminPassword = "Admin@123";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Administrator",
                    EmailConfirmed = true,
                    RegisteredOn = DateTime.Now
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
            else if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // ═══ Seed Sample Courses ═══
            if (!context.Courses.Any())
            {
                var courses = new Course[]
                {
                    new Course
                    {
                        CourseName = "Introduction to Computer Science",
                        CourseCode = "CS101",
                        Description = "Fundamentals of computer science, algorithms, and programming.",
                        Credits = 3,
                        Instructor = "Dr. Smith"
                    },
                    new Course
                    {
                        CourseName = "Database Management Systems",
                        CourseCode = "CS201",
                        Description = "Relational databases, SQL, normalization, and transaction management.",
                        Credits = 4,
                        Instructor = "Dr. Johnson"
                    },
                    new Course
                    {
                        CourseName = "Web Application Development",
                        CourseCode = "CS301",
                        Description = "ASP.NET Core MVC, Entity Framework, and modern web development.",
                        Credits = 4,
                        Instructor = "Prof. Williams"
                    },
                    new Course
                    {
                        CourseName = "Data Structures and Algorithms",
                        CourseCode = "CS202",
                        Description = "Trees, graphs, sorting, searching, and algorithm analysis.",
                        Credits = 4,
                        Instructor = "Dr. Brown"
                    },
                    new Course
                    {
                        CourseName = "Software Engineering",
                        CourseCode = "CS401",
                        Description = "Software development lifecycle, design patterns, and project management.",
                        Credits = 3,
                        Instructor = "Prof. Davis"
                    }
                };

                context.Courses.AddRange(courses);
                await context.SaveChangesAsync();
            }
        }
    }
}
