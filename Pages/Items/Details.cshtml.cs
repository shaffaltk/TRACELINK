using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TraceLink.Data;
using TraceLink.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TraceLink.Pages.Items
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Item Item { get; set; }
        public List<Item> UsedItems { get; set; } = new List<Item>();
        public List<ItemParameter> Parameters { get; set; } = new List<ItemParameter>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Fetch the item
            Item = await _context.Items.FindAsync(id);

            if (Item == null)
            {
                return NotFound();
            }

            // Fetch the used items
            if (Item.UsedItemIds != null && Item.UsedItemIds.Any())
            {
                UsedItems = await _context.Items
                    .Where(i => Item.UsedItemIds.Contains(i.Id))
                    .ToListAsync();
            }

            // Fetch the parameters for this item
            Parameters = await _context.ItemParameters
                .Where(p => p.ItemId == id)
                .ToListAsync();

            return Page();
        }
    }
}
