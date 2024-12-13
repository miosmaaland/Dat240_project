using MediatR;
using SmaHauJenHoaVij.Core.Domain.Fulfillment.Events;
using SmaHauJenHoaVij.Infrastructure.Data;
using SmaHauJenHoaVij.Infrastructure.Services;

namespace SmaHauJenHoaVij.Core.Domain.Fulfillment.Handlers
{
    public class OrderDeliveredHandler : INotificationHandler<OrderDelivered>
    {
        private readonly ShopContext _db;
        private readonly IEmailService _emailService;

        public OrderDeliveredHandler(ShopContext db, IEmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }

        public async Task Handle(OrderDelivered notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[OrderDeliveredHandler] Event received at {notification.DateOccurred}: " +
                              $"OrderId={notification.OrderId}, CourierId={notification.CourierId}");

            var order = await _db.Orders.FindAsync(notification.OrderId);
            if (order == null)
            {
                throw new Exception($"Order not found for Order ID {notification.OrderId}");
            }

            var user = await _db.Customers.FindAsync(order.CustomerId);
            if (user == null)
            {
                throw new Exception($"user not found for user ID {order.CustomerId}");
            }

            order.MarkAsDelivered(); // Oppdater status til "Delivered"
            await _db.SaveChangesAsync(cancellationToken);

            // Send e-postvarsel til kundens registrerte e-postadresse
            var emailBody = $"Ordren med ID {notification.OrderId} har blitt levert av kurér {notification.CourierId}.";
            await _emailService.SendEmailAsync(user.Email, "Ordre levert", emailBody);
        }
    }
}
