using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TraceLink.Data;
using TraceLink.Models;

namespace TraceLink.Pages.SubDealers
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<TraceLinkUser> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<TraceLinkUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Retrieve the logged-in user's information
            var user = await _userManager.GetUserAsync(User);

            if (user == null || user.DealerId == null)
            {
                ModelState.AddModelError(string.Empty, "Unable to determine dealer information. Please contact the administrator.");
                return Page();
            }

            // Only show subdealers associated with the dealer
            ViewData["DealerId"] = new SelectList(_context.Dealers.Where(d => d.DealerId == user.DealerId), "DealerId", "DealerName");
            return Page();
        }

        [BindProperty]
        public SubDealer SubDealer { get; set; } = default!;

        [BindProperty]
        public string UserEmail { get; set; }

        [BindProperty]
        public string UserPassword { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // Retrieve the logged-in user's information
            var user = await _userManager.GetUserAsync(User);

            if (user == null || user.DealerId == null)
            {
                ModelState.AddModelError(string.Empty, "Unable to determine dealer information. Please contact the administrator.");
                return Page();
            }

            // Assign the logged-in user's DealerId to the SubDealer
            SubDealer.DealerId = user.DealerId.Value;

            // Create the SubDealer first
            _context.SubDealers.Add(SubDealer);
            await _context.SaveChangesAsync();

            // Create the user and associate it with the SubDealer and the Dealer
            var subDealerUser = new TraceLinkUser
            {
                UserName = UserEmail,
                Email = UserEmail,
                DealerId = user.DealerId, // Assign the dealer's ID to the new user
                SubDealerId = SubDealer.SubDealerId // Assign the subdealer's ID to the new user
            };

            var result = await _userManager.CreateAsync(subDealerUser, UserPassword);

            if (result.Succeeded)
            {
                // Optionally assign the "SubDealer" role to the user
                var roleResult = await _userManager.AddToRoleAsync(subDealerUser, "SubDealer");

                if (roleResult.Succeeded)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewData["DealerId"] = new SelectList(_context.Dealers.Where(d => d.DealerId == user.DealerId), "DealerId", "DealerName");
            return Page();
        }

    }
}
