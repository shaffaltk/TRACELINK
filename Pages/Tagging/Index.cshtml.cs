using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TraceLink.Data;
using TraceLink.Models;

namespace TraceLink.Pages.Tagging
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Holds the list of Tagging Requests
        public IList<TaggingRequest> TaggingRequests { get; set; } = new List<TaggingRequest>();

        public async Task OnGetAsync()
        {
            // Retrieve TaggingRequests from the database, including TaggingStatus if needed
            TaggingRequests = await _context.TaggingRequests
                                             .Include(t => t.TaggingStatus)
                                            
                                             .Include(t =>t.SubDealer)// Include related data, if applicable
                                             .ToListAsync();
        }
    }
}
