namespace TraceLink.Models
{
    public class TaxDetails
    {
        public int TaxDetailsId { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public string? TaxPreference { get; set; }
        public decimal? TaxRatePurchase { get; set; }
        public decimal? TaxRateSales { get; set; }
    }

}
