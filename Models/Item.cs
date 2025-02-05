using TraceLink.Models;

public class Item
{
    public int Id { get; set; } // Primary Key
    public string? IMEI { get; set; } // IMEI of the item (if applicable)
    public string? Name { get; set; } // Name of the item (e.g., "VLTD Device")
    public bool? IsTagged { get; set; } // Indicates if the item has already been tagged
    public string? Category { get; set; } // Category of the item (e.g., "VLTD Device", "Panic Button")
    public DateTime? TaggingDate { get; set; } // Date when the item was tagged

    public List<int> UsedItemIds { get; set; } = new List<int>();
    public List<ItemParameter> Parameters { get; set; } = new List<ItemParameter>();
}
