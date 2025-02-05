using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TraceLink.Data;
using TraceLink.Models;

namespace TraceLink.Pages.ItemParameters
{
    public class AddParameterModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddParameterModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Item Item { get; set; }

        [BindProperty]
        public string SerializedParameters { get; set; } // JSON Serialized Parameters

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Item = await _context.Items.FindAsync(id);
            if (Item == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(SerializedParameters))
            {
                var parametersList = JsonConvert.DeserializeObject<List<ParameterDTO>>(SerializedParameters);
                foreach (var param in parametersList)
                {
                    var itemParameter = new ItemParameter
                    {
                        ItemId = item.Id,
                        ParameterName = param.Name,
                        DataType = param.Type // ✅ Save data type
                    };
                    _context.ItemParameters.Add(itemParameter);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Items/Index");
        }

        public class ParameterDTO
        {
            public string Name { get; set; }
            public string Type { get; set; }
        }
    }
}
