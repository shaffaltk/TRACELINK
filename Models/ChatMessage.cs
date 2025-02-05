namespace TraceLink.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int TaggingRequestId { get; set; } // Link to the tagging request
        public TaggingRequest TaggingRequest { get; set; }
        public string SenderId { get; set; } // ID of the sender (Dealer or SubDealer)
        public string SenderRole { get; set; } // Role of the sender (e.g., "Dealer" or "SubDealer")
        public string Message { get; set; } // The chat message
        public DateTime Timestamp { get; set; } // Timestamp of the message
    }
}