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
    public class DeleteModel : PageModel
    {
        private readonly TraceLink.Data.ApplicationDbContext _context;

        public DeleteModel(TraceLink.Data.ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subdealer = await _context.SubDealers.FindAsync(id);
            if (subdealer != null)
            {
                SubDealer = subdealer;
                _context.SubDealers.Remove(SubDealer);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
