using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TraceLink.Pages.ProductionRequests
{
    public class IndexModel : PageModel
    {
        public List<ProductionRequest> ProductionRequests { get; set; } = new List<ProductionRequest>();

        public void OnGet()
        {
            // Hardcoded production requests
            ProductionRequests = new List<ProductionRequest>
            {
                new ProductionRequest { Id = 1, PrNumber = "PR-1001", Vendor = "ABC Supplies", RequestDate = DateTime.Today, Product = "Widget X", Quantity = 50, Status = "Pending" },
                new ProductionRequest { Id = 2, PrNumber = "PR-1002", Vendor = "XYZ Manufacturing", RequestDate = DateTime.Today, Product = "Gadget Y", Quantity = 20, Status = "Pending" },
                new ProductionRequest { Id = 3, PrNumber = "PR-1003", Vendor = "LMN Corp", RequestDate = DateTime.Today, Product = "Tool Z", Quantity = 100, Status = "Pending" }
            };
        }

        public IActionResult OnPostAccept(int id)
        {
            // Normally, you'd update the database here
            // But since it's hardcoded, we just simulate an action

            TempData["Message"] = $"Production Request {id} has been accepted!";
            return RedirectToPage();
        }
    }

    
}
