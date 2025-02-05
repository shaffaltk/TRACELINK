using System.ComponentModel.DataAnnotations;

namespace TraceLink.Models
{
    public class SubDealer
    {
        [Key]
        public int SubDealerId { get; set; }
        public string? SubDealerName { get; set; }

        public int DealerId { get; set; }
        public Dealer Dealer { get; set; }
    }
}
