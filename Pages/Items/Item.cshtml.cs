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
    public class CreateDynamicItemModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateDynamicItemModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Item NewItem { get; set; } = new Item();

        [BindProperty]
        public string SelectedItemIds { get; set; } // Comma-separated list of selected items

        public List<Item> AvailableItems { get; set; } = new List<Item>();

        public async Task<IActionResult> OnGetAsync()
        {
            AvailableItems = await _context.Items.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
          

            // Convert selected item IDs to a list
            var selectedItemIds = string.IsNullOrEmpty(SelectedItemIds)
                ? new List<int>()
                : SelectedItemIds.Split(',').Select(int.Parse).ToList();

            // Ensure `UsedItemIds` is stored properly
            NewItem.UsedItemIds = selectedItemIds;

            // Save the new item first
            _context.Items.Add(NewItem);
            await _context.SaveChangesAsync();

            // Retrieve parameters from the selected items
            var inheritedParameters = await _context.ItemParameters
                .Where(p => selectedItemIds.Contains(p.ItemId))
                .ToListAsync();

            // Clone and add inherited parameters to the new item
            foreach (var param in inheritedParameters)
            {
                var newParam = new ItemParameter
                {
                    ItemId = NewItem.Id,  // Assign new item ID
                    ParameterName = param.ParameterName,
                    DataType = param.DataType
                };
                _context.ItemParameters.Add(newParam);
            }

            await _context.SaveChangesAsync(); // Save the inherited parameters

            return RedirectToPage("Index");
        }

    }
}
