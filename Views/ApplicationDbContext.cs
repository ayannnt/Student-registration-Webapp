using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentRegistrationWebApp.Models;

namespace StudentRegistrationWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseRegistration> CourseRegistrations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Ensure a student can only register for ONE course
            builder.Entity<CourseRegistration>()
                .HasIndex(r => r.StudentId)
                .IsUnique();

            builder.Entity<CourseRegistration>()
                .HasOne(r => r.Student)
                .WithMany()
                .HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CourseRegistration>()
                .HasOne(r => r.Course)
                .WithMany(c => c.Registrations)
                .HasForeignKey(r => r.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
