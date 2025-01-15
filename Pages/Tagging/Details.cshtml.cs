using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TraceLink.Models;

namespace TraceLink.Pages.Tagging
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public TagDetails TagDetails { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tagdetails = await _context.TagDetails.FirstOrDefaultAsync(m => m.TaggingId == id);
            if (tagdetails == null)
            {
                return NotFound();
            }
            else
            {
                TagDetails = tagdetails;
            }
            return Page();
        }
    }
}
