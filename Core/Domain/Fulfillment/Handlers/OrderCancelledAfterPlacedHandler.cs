using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;
using SmaHauJenHoaVij.Core.Domain.Ordering.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Core.Domain.Fulfillment.Handlers
{
    public class OrderCancelledAfterPlacedHandler : INotificationHandler<OrderCancelledAfterPlaced>
    {
        private readonly ShopContext _db;

        public OrderCancelledAfterPlacedHandler(ShopContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task Handle(OrderCancelledAfterPlaced notification, CancellationToken cancellationToken)
        {
            // Fetch the offer
            var offer = await _db.Offers.FirstOrDefaultAsync(o => o.OrderId == notification.OrderId, cancellationToken);
            if (offer != null)
            {
                _db.Offers.Remove(offer); // Remove the offer
                await _db.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
