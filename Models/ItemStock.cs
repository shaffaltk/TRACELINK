namespace TraceLink.Models
{
    public class ItemStock
    {
        public int ItemStockId { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public ICollection<ItemStockParameter>? Parameters { get; set; }
    }


}
