namespace TraceLink.Models
{
    public class Pricing
    {
        public int PricingId { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? SellingPrice { get; set; }
    }

}
