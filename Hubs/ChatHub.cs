using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using TraceLink.Data;
using TraceLink.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace TraceLink.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<TraceLinkUser> _userManager;

        public ChatHub(ApplicationDbContext context, UserManager<TraceLinkUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SendMessage(int taggingRequestId, string message)
        {
            var user = Context.User;
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = user.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userEmail))
            {
                throw new HubException("User is not authenticated.");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new HubException("Message cannot be empty.");
            }

            if (message.Length > 1000) // Example max length
            {
                throw new HubException("Message is too long.");
            }

            // Fetch the user's role (Dealer or SubDealer)
            var userRole = user.IsInRole("Dealer") ? "Dealer" : "SubDealer";

            // Validate taggingRequestId
            var taggingRequest = await _context.TaggingRequests.FindAsync(taggingRequestId);
            if (taggingRequest == null)
            {
                throw new HubException("Invalid tagging request.");
            }

            // Save the message to the database
            var chatMessage = new ChatMessage
            {
                TaggingRequestId = taggingRequestId,
                SenderId = userId,
                SenderRole = userRole,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            try
            {
                await _context.ChatMessages.AddAsync(chatMessage);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new HubException("Failed to save message.", ex);
            }

            // Broadcast the message to the group (taggingRequestId)
            await Clients.Group(taggingRequestId.ToString())
                .SendAsync("ReceiveMessage", userRole, userEmail, message);
        }

        public async Task JoinGroup(int taggingRequestId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, taggingRequestId.ToString());
        }

        public async Task LeaveGroup(int taggingRequestId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, taggingRequestId.ToString());
        }
    }
}