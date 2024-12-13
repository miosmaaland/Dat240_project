using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Core.Domain.Ordering.Events;
using SmaHauJenHoaVij.Infrastructure.Data;
using SmaHauJenHoaVij.Core.Domain.Fulfillment;
using SmaHauJenHoaVij.Core.Exceptions;

namespace SmaHauJenHoaVij.Core.Domain.Fulfillment.Handlers;

public class OrderPlacedHandler : INotificationHandler<OrderPlaced>
{
    private readonly ShopContext _db;

    public OrderPlacedHandler(ShopContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task Handle(OrderPlaced notification, CancellationToken cancellationToken)
    {

        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == notification.OrderId, cancellationToken);

        if (order == null)
        {
            throw new EntityNotFoundException($"Order with ID {notification.OrderId} was not found.");
        }

        // Check if an offer already exists for the order
        if (await _db.Offers.AnyAsync(o => o.OrderId == order.Id, cancellationToken))
        {
            throw new InvalidOperationException($"An offer for Order ID {order.Id} already exists.");
        }

        // Calculate reimbursement
        var reimbursementAmount = order.DeliveryFee.Value * 0.8m;

        // Create the Reimbursement
        var reimbursement = new Reimbursement(reimbursementAmount);
        _db.Reimbursements.Add(reimbursement);

        await _db.SaveChangesAsync(cancellationToken);
        // Create and save the Offer with the Reimbursement
        var offer = new Offer(order.Id, reimbursement.Id);

        _db.Offers.Add(offer);
        await _db.SaveChangesAsync(cancellationToken);


    }
}
