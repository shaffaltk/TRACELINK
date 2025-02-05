using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TraceLink.Pages.ProductionRequests
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public ProductionRequest ProductionRequest { get; set; }

        public void OnGet()
        {
            // Hardcoded default values
            ProductionRequest = new ProductionRequest
            {
                PrNumber = "PR-123456",
                Vendor = "ABC Supplies",
                RequestDate = DateTime.Today,
                Product = "Widget X",
                Quantity = 100,
                SpecialInstruction = "Handle with care",
                Status = "Pending",
                ClosedDate = null
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Normally, you would save to the database
            // Here, we'll just redirect to a confirmation page
            return RedirectToPage("/ProductionRequests/Index");
        }
    }

    public class ProductionRequest
    {
        [Key]
        public int Id { get; set; }

        public string PrNumber { get; set; }
        public string Vendor { get; set; }
        public DateTime RequestDate { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public string SpecialInstruction { get; set; }
        public string Status { get; set; }
        public DateTime? ClosedDate { get; set; }
    }
}
