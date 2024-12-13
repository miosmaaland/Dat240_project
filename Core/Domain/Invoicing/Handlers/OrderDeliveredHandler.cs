using MediatR;
using SmaHauJenHoaVij.Core.Domain.Fulfillment.Events;
using SmaHauJenHoaVij.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Core.Domain.Invoicing.Handlers
{
    public class OrderDeliveredHandler : INotificationHandler<OrderDelivered>
    {
        private readonly ShopContext _db;

        public OrderDeliveredHandler(ShopContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task Handle(OrderDelivered notification, CancellationToken cancellationToken)
        {
            // Find the corresponding invoice for the delivered order
            var invoice = await _db.Invoices
                .FirstOrDefaultAsync(i => i.OrderId == notification.OrderId, cancellationToken);

            if (invoice == null)
            {
                throw new Exception($"Invoice for Order ID {notification.OrderId} not found.");
            }

            // Mark the invoice as sent
            invoice.MarkAsSent();

            // Save changes to the database
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
