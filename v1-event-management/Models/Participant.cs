namespace EventManagementWebApp.Models
{
    public class Participant
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }

        // Foreign Key
        public int EventId { get; set; }

        // Navigation property to retrieve information of registered Event
        public virtual EventInfo Event { get; set; }
    }
}
