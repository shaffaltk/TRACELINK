namespace TraceLink.Models
{
    public class TaggingHistory
    {
        public int Id { get; set; } // Primary Key
        public int TaggingRequestId { get; set; } // Foreign Key to TaggingRequest
        public TaggingRequest TaggingRequest { get; set; } // Navigation Property
        public string Action { get; set; } // Action performed (e.g., "Request Created", "Approved")
        public string Remarks { get; set; } // Optional remarks about the action
        public DateTime ActionDate { get; set; } // Date and time of the action
       // public int? PerformedById { get; set; } // Optional: User who performed the action
       // public User PerformedBy { get; set; } // Navigation property
    }

}
