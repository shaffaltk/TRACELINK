namespace TraceLink.Models
{
    public class UnitMap
    {
        public int UnitMapId { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public string? Unit { get; set; }
        public int? NoOfEach { get; set; }
    }

}
