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

namespace TraceLink.Pages.SubDealers
{
    public class EditModel : PageModel
    {
        private readonly TraceLink.Data.ApplicationDbContext _context;

        public EditModel(TraceLink.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SubDealer SubDealer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subdealer =  await _context.SubDealers.FirstOrDefaultAsync(m => m.SubDealerId == id);
            if (subdealer == null)
            {
                return NotFound();
            }
            SubDealer = subdealer;
           ViewData["DealerId"] = new SelectList(_context.Dealers, "DealerId", "DealerId");
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

            _context.Attach(SubDealer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubDealerExists(SubDealer.SubDealerId))
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

        private bool SubDealerExists(int id)
        {
            return _context.SubDealers.Any(e => e.SubDealerId == id);
        }
    }
}
