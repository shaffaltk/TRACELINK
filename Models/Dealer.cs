using System.ComponentModel.DataAnnotations;

namespace TraceLink.Models
{
    public class Dealer
    {
        [Key]
        public int DealerId { get; set; }
        public string DealerName { get; set; }

        public ICollection<SubDealer> SubDealers { get; set; }


    }
}
