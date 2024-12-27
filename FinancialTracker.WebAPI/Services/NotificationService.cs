using Microsoft.AspNetCore.SignalR;
using FinancialTracker.Domain.Interfaces;
using FinancialTracker.WebAPI.Hubs;
 using FinancialTracker.Domain.Entites;    // Adjust this to your Notification entity namespace
using System;
using System.Threading.Tasks;
using FinancialTracker.Infrastructure;

namespace FinancialTracker.WebAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IServiceProvider _serviceProvider; // Inject the global ServiceProvider

        public NotificationService(IHubContext<NotificationHub> hubContext, IServiceProvider serviceProvider)
        {
            _hubContext = hubContext;
            _serviceProvider = serviceProvider; // Save the service provider for later use
        }

        // Notify a specific user with a real-time notification
        public async Task NotifyUserAsync(string userId, Notification notification)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notification);
        }

        // Mark a notification as read
        public async Task MarkNotificationAsReadAsync(int notificationId)
        {
            // Use the ServiceProvider to create a scope
            using (var scope = _serviceProvider.CreateScope())
            {
                // Resolve the DbContext from the scoped provider
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Find the notification
                var notification = await dbContext.Notifications.FindAsync(notificationId);
                if (notification == null)
                {
                    throw new KeyNotFoundException($"Notification with ID {notificationId} not found.");
                }

                // Mark the notification as read
                notification.IsRead = true;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task NotifyAllAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}
