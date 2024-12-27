using FinancialTracker.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Domain.Interfaces
{
    public interface INotificationService
    {
        Task NotifyUserAsync(string userId, Notification notification);
        Task NotifyAllAsync(string message);

        Task MarkNotificationAsReadAsync(int notificationId);

    }
}
