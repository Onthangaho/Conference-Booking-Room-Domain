using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ConferenceBookingRoomAPI.Hubs
{
    [Authorize]
    public class BookingsHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var username = Context.User?.Identity?.Name;
            if (!string.IsNullOrWhiteSpace(username))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{username.ToLowerInvariant()}");
            }

            var roles = Context.User?.FindAll(ClaimTypes.Role).Select(r => r.Value.ToLowerInvariant()) ?? Enumerable.Empty<string>();
            foreach (var role in roles)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"role:{role}");
            }

            await base.OnConnectedAsync();
        }
    }
}
