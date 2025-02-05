namespace TraceLink.Models
{
    public class ItemStockParameter
    {
        public int ItemStockParameterId { get; set; }
        public int ItemStockId { get; set; }
        public ItemStock ItemStock { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
    }

}
