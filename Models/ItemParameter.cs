using System.ComponentModel.DataAnnotations;

namespace TraceLink.Models
{
    public class ItemParameter
    {
        [Key]
        public int ParameterId { get; set; }
        public string ParameterName { get; set; }

        public string DataType { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
