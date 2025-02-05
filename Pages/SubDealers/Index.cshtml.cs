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
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<SubDealer> SubDealer { get;set; } = default!;

        public async Task OnGetAsync()
        {
            SubDealer = await _context.SubDealers
                .Include(s => s.Dealer).ToListAsync();
        }
    }
}
