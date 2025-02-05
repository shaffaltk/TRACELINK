using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TraceLink.Data;
using TraceLink.Models;

namespace TraceLink.Pages.Dealers
{
    public class DetailsModel : PageModel
    {
        private readonly TraceLink.Data.ApplicationDbContext _context;

        public DetailsModel(TraceLink.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Dealer Dealer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dealer = await _context.Dealers.FirstOrDefaultAsync(m => m.DealerId == id);
            if (dealer == null)
            {
                return NotFound();
            }
            else
            {
                Dealer = dealer;
            }
            return Page();
        }
    }
}
