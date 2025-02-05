using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TraceLink.Data;
using TraceLink.Models;

namespace TraceLink.Pages.Dealers
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

        [BindProperty]
        public Dealer Dealer { get; set; }

        [BindProperty]
        public string UserEmail { get; set; }

        [BindProperty]
        public string UserPassword { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            

            // Create the Dealer first
            _context.Dealers.Add(Dealer);
            await _context.SaveChangesAsync();

            // Create a new TraceLinkUser and associate with the created Dealer
            var user = new TraceLinkUser
            {
                UserName = UserEmail,
                Email = UserEmail,
                DealerId = Dealer.DealerId // Associate the user with the created dealer
            };

            var result = await _userManager.CreateAsync(user, UserPassword);

            if (result.Succeeded)
            {
                // Assign the "Dealer" role to the user
                var roleResult = await _userManager.AddToRoleAsync(user, "Dealer");

                if (roleResult.Succeeded)
                {
                    return RedirectToPage("Index");
                }
                else
                {
                    // Add role assignment errors to the model state
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                // Add user creation errors to the model state
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}
