using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TraceLink.Data;
using TraceLink.Models;

namespace TraceLink.Pages.SubDealers
{
    public class DetailsModel : PageModel
    {
        private readonly TraceLink.Data.ApplicationDbContext _context;

        public DetailsModel(TraceLink.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public SubDealer SubDealer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subdealer = await _context.SubDealers.FirstOrDefaultAsync(m => m.SubDealerId == id);
            if (subdealer == null)
            {
                return NotFound();
            }
            else
            {
                SubDealer = subdealer;
            }
            return Page();
        }
    }
}
