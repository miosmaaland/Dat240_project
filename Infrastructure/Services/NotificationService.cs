using Microsoft.AspNetCore.SignalR;
using SmaHauJenHoaVij.Infrastructure.SignalR;
using System;
using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Infrastructure.Services
{
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyCustomerAsync(Guid customerId, string message)
        {
            await _hubContext.Clients.User(customerId.ToString()).SendAsync("ReceiveNotification", message);
        }

        public async Task NotifyAllCouriersAsync(string message)
        {
            await _hubContext.Clients.Group("Couriers").SendAsync("ReceiveNotification", message);
        }
    }
}
