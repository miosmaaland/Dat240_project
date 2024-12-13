using MediatR;
using SmaHauJenHoaVij.Core.Domain.Ordering.Events;
using SmaHauJenHoaVij.Infrastructure.Data;
using SmaHauJenHoaVij.Core.Domain.Invoicing;

namespace SmaHauJenHoaVij.Core.Domain.Invoicing.Handlers
{
    public class OrderPlacedHandler : INotificationHandler<OrderPlaced>
    {
        private readonly ShopContext _db;

        public OrderPlacedHandler(ShopContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task Handle(OrderPlaced notification, CancellationToken cancellationToken)
        {
            var invoice = new Invoice(notification.CustomerId, notification.OrderId, notification.TotalAmount);
            _db.Invoices.Add(invoice);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
