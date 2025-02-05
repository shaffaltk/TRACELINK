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
    public class DeleteModel : PageModel
    {
        private readonly TraceLink.Data.ApplicationDbContext _context;

        public DeleteModel(TraceLink.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dealer = await _context.Dealers.FindAsync(id);
            if (dealer != null)
            {
                Dealer = dealer;
                _context.Dealers.Remove(Dealer);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
