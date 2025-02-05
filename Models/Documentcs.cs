namespace TraceLink.Models
{
    public class Document
    {
        public int Id { get; set; }
        public int TaggingRequestId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedDate { get; set; }

        public string DocumentType { get; set; }

        // Navigation property
        public TaggingRequest TaggingRequest { get; set; }
    }
}
