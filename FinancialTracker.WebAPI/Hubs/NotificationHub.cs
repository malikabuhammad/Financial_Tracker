using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace FinancialTracker.WebAPI.Hubs
{
    public sealed class NotificationHub : Hub
    {
        // Optional: This method lets clients send a message to the server
        public async Task SendNotificationToServer(string message)
        {
            // Log or process messages from the client
            await Task.CompletedTask;
        }

        // Optional: Log when a user connects or disconnects
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            // Additional logic when a client connects (e.g., logging)
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            // Additional logic when a client disconnects (e.g., cleanup)
        }
    }
}
