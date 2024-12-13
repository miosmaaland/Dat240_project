using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Core.Domain.Fulfillment.Events;
using SmaHauJenHoaVij.Infrastructure.Services;
using SmaHauJenHoaVij.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;
using SmaHauJenHoaVij.Core.Exceptions;

namespace SmaHauJenHoaVij.Core.Domain.Notifications.Handlers
{
    public class OrderPickedUpNotificationHandler : INotificationHandler<OrderPickedUp>
    {
        private readonly NotificationService _notificationService;
        private readonly ShopContext _db;

        public OrderPickedUpNotificationHandler(NotificationService notificationService, ShopContext db)
        {
            _notificationService = notificationService;
            _db = db;
        }

        public async Task Handle(OrderPickedUp notification, CancellationToken cancellationToken)
        {
            
            Console.WriteLine($"[OrderPickedUpNotificationHandler] OrderId: {notification.OrderId}");

            // Fetch the customer ID associated with the order
            var customerId = await _db.Orders
                .Where(o => o.Id == notification.OrderId)
                .Select(o => o.CustomerId)
                .FirstOrDefaultAsync(cancellationToken);

            if (customerId == Guid.Empty)
            {
                return;           
            }

            // Send a notification to the customer
            await _notificationService.NotifyCustomerAsync(customerId,
                $"Your order has been picked up.");
        }
    }
}
