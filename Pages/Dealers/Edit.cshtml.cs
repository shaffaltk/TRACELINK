using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TraceLink.Data;
using TraceLink.Models;

namespace TraceLink.Pages.Dealers
{
    public class EditModel : PageModel
    {
        private readonly TraceLink.Data.ApplicationDbContext _context;

        public EditModel(TraceLink.Data.ApplicationDbContext context)
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

            var dealer =  await _context.Dealers.FirstOrDefaultAsync(m => m.DealerId == id);
            if (dealer == null)
            {
                return NotFound();
            }
            Dealer = dealer;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Dealer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DealerExists(Dealer.DealerId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DealerExists(int id)
        {
            return _context.Dealers.Any(e => e.DealerId == id);
        }
    }
}
