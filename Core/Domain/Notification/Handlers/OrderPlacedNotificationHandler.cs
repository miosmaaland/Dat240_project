using MediatR;
using SmaHauJenHoaVij.Core.Domain.Ordering.Events;
using SmaHauJenHoaVij.Infrastructure.Services;
using System.Threading;
using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Core.Domain.Notifications.Handlers
{
    public class OrderPlacedNotificationHandler : INotificationHandler<OrderPlaced>
    {
        private readonly NotificationService _notificationService;

        public OrderPlacedNotificationHandler(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(OrderPlaced notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[OrderPlacedNotificationHandler] OrderId: {notification.OrderId}");

            try
            {
                await _notificationService.NotifyCustomerAsync(notification.CustomerId,
                    $"Order has been placed successfully.");
                Console.WriteLine($"[OrderPlacedNotificationHandler] Customer {notification.CustomerId} notified.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OrderPlacedNotificationHandler] Error notifying customer {notification.CustomerId}: {ex.Message}");
            }

            try
            {
                await _notificationService.NotifyAllCouriersAsync("A new order is available for pickup.");
                Console.WriteLine($"[OrderPlacedNotificationHandler] Couriers notified.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OrderPlacedNotificationHandler] Error notifying couriers: {ex.Message}");
            }
        }

    }
}
