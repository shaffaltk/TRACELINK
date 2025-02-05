namespace TraceLink.Models
{
    public class ItemCategoryParameter
    {
        public int ItemCategoryParameterId { get; set; }
        public int ItemCategoryId { get; set; }
        public ItemCategory ItemCategory { get; set; }
        public string? ParameterName { get; set; }
        public string? DataType { get; set; }
    }

}
