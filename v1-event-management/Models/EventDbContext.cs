using Microsoft.EntityFrameworkCore;

namespace EventManagementWebApp.Models
{
    public class EventDbContext : DbContext
    {
        // Constructor Injection
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options) { }

        public DbSet<Participant> Participants { get; set; }
        public DbSet<EventInfo> Event { get; set; }

        // Seed Data - Create the table with some initial records
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seeding data for the EventInfo entity using Fluent API
            modelBuilder.Entity<EventInfo>().HasData(
                new EventInfo { EventId = 1, EventName = "AI Fundamentals", EventLocation = "Hyderabad" },
                new EventInfo { EventId = 2, EventName = "ASP.NET CORE Web Api Workshop", EventLocation = "Mumbai" }
            );
        }
    }
}
