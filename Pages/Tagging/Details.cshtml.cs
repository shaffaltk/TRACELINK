using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TraceLink.Data;
using TraceLink.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using TraceLink.Hubs;

namespace TraceLink.Pages.Tagging
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<TraceLinkUser> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;

        public DetailsModel(
            ApplicationDbContext context,
            UserManager<TraceLinkUser> userManager,
            IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        public TaggingRequest TaggingRequest { get; set; }
        public IQueryable<Document> Documents { get; set; }
        public IQueryable<ChatMessage> ChatMessages { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Fetch the tagging request by ID
            TaggingRequest = await _context.TaggingRequests.FindAsync(id);
            if (TaggingRequest == null)
            {
                return NotFound();
            }

            // Fetch associated documents
            Documents = _context.Documents.Where(d => d.TaggingRequestId == id);

            // Fetch chat messages for this tagging request
            ChatMessages = _context.ChatMessages
                .Where(c => c.TaggingRequestId == id)
                .OrderBy(c => c.Timestamp);

            return Page();
        }

        // Handler for sending a chat message
        public async Task<IActionResult> OnPostSendMessageAsync(int taggingRequestId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("Message cannot be empty.");
            }

            // Get the current user
            var user = await _userManager.GetUserAsync(User);
            

            // Get the user's role (Dealer or SubDealer)
            var userRole = User.IsInRole("Dealer") ? "Dealer" : "SubDealer";

            // Validate the tagging request
            var taggingRequest = await _context.TaggingRequests.FindAsync(taggingRequestId);
            if (taggingRequest == null)
            {
                return NotFound("Tagging request not found.");
            }

            // Create a new chat message
            var chatMessage = new ChatMessage
            {
                TaggingRequestId = taggingRequestId,
                SenderId = user.Id,
                SenderRole = userRole,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            // Save the chat message to the database
            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            // Broadcast the message to the SignalR group (taggingRequestId)
            await _hubContext.Clients.Group(taggingRequestId.ToString())
                .SendAsync("ReceiveMessage", userRole, user.Email, message);

            return RedirectToPage(new { id = taggingRequestId });
        }
    }
}