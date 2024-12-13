using MediatR;
using SmaHauJenHoaVij.Core.Domain.Fulfillment.Events;
using SmaHauJenHoaVij.Infrastructure.Data;

namespace SmaHauJenHoaVij.Core.Domain.Fulfillment.Handlers;

public class OfferCourierSetHandler : INotificationHandler<OfferCourierSet>
{
    private readonly ShopContext _db;

    public OfferCourierSetHandler(ShopContext db)
    {
        _db = db;
    }

    public async Task Handle(OfferCourierSet notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[OfferCourierSetHandler] Event received at {notification.DateOccurred}: " +
                          $"OrderId={notification.OrderId}, CourierId={notification.CourierId}");

        var order = await _db.Orders.FindAsync(notification.OrderId);
        if (order == null)
        {
            throw new Exception($"Order not found for Order ID {notification.OrderId}");
        }

        order.MarkAsAccepted(); // Update the order status to "Accepted"
        await _db.SaveChangesAsync(cancellationToken);
    }
}
