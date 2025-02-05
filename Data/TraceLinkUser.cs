using Microsoft.AspNetCore.Identity;
using TraceLink.Models;

namespace TraceLink.Data
{
    public class TraceLinkUser : IdentityUser
    {
        public int? DealerId { get; set; } // Make nullable
        public virtual Dealer Dealer { get; set; }

        public int? SubDealerId { get; set; } // Make nullable
        public virtual SubDealer SubDealer { get; set; }
    }

}
