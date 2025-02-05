namespace TraceLink.Models
{
    public class ItemCategory
    {
        public int ItemCategoryId { get; set; }
        public string? Name { get; set; }
        public ICollection<ItemCategoryParameter>? Parameters { get; set; }
        public ICollection<Item>? Items { get; set; }
    }

}
