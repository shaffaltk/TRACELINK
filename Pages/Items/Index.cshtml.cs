using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TraceLink.Data;
using TraceLink.Models;

namespace TraceLink.Pages.Items
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // List of all items
        public List<Item> Items { get; set; }

        // List of available items to display their names in the UsedItems column
        public List<Item> AvailableItems { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Fetch all items from the database
            Items = await _context.Items.ToListAsync();

            // Fetch available items for the "Used Item" references
            AvailableItems = await _context.Items.ToListAsync();

            return Page();
        }

        // Handle delete request
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return RedirectToPage(); // Refresh the page after deletion
        }
    }
}
