using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Core.Domain.Fulfillment.Events;
using SmaHauJenHoaVij.Infrastructure.Data;
using SmaHauJenHoaVij.Infrastructure.Services;
using System.Threading;
using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Core.Domain.Notifications.Handlers
{
    public class OrderDeliveredNotificationHandler : INotificationHandler<OrderDelivered>
    {
        private readonly ShopContext _db;
        private readonly NotificationService _notificationService;

        public OrderDeliveredNotificationHandler(NotificationService notificationService, ShopContext db)
        {
            _notificationService = notificationService;
            _db = db;
        }

        public async Task Handle(OrderDelivered notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[OrderDeliveredNotificationHandler] OrderId: {notification.OrderId}");
            // Fetch the customer ID associated with the order
            var customerId = await _db.Orders
                .Where(o => o.Id == notification.OrderId)
                .Select(o => o.CustomerId)
                .FirstOrDefaultAsync(cancellationToken);
            

            Console.WriteLine($"customerid: {customerId}");

            if (customerId == Guid.Empty)
            {
                Console.WriteLine("Customer ID not found for order");           
            }

            // Send a notification to the customer
            await _notificationService.NotifyCustomerAsync(customerId,
                $"Your order has been picked up.");
        }
    }
}
