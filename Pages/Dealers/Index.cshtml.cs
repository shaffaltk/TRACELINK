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
    public class IndexModel : PageModel
    {
        private readonly TraceLink.Data.ApplicationDbContext _context;

        public IndexModel(TraceLink.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Dealer> Dealer { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Dealer = await _context.Dealers.ToListAsync();
        }
    }
}
