using System.ComponentModel.DataAnnotations;

namespace TraceLink.Models
{
    public class ProductionRequest
    {
        [Key]
        public int Id { get; set; }

        public string PrNumber { get; set; }

        public string Vendor { get; set; }
        public DateTime RequestDate { get; set; }

        public string Product { get; set; }
        public int Quantity { get; set; }
        public string SpecialInstruction { get; set; }

        public string Status { get; set; }

        public DateTime? ClosedDate { get; set; }
    }



}
