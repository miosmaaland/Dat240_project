using MediatR;
using SmaHauJenHoaVij.Core.Domain.Invoicing.Events;
using SmaHauJenHoaVij.Infrastructure.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Core.Domain.Invoicing.Handlers
{
    public class InvoicePaidHandler : INotificationHandler<InvoicePaid>
    {
        private readonly ShopContext _db;

        public InvoicePaidHandler(ShopContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task Handle(InvoicePaid notification, CancellationToken cancellationToken)
        {
            // Fetch the invoice
            var invoice = await _db.Invoices.FindAsync(notification.InvoiceId);

            if (invoice == null)
            {
                throw new Exception($"Invoice with ID {notification.InvoiceId} not found.");
            }

            // Mark the invoice as paid
            invoice.MarkAsPaid();

            // Save changes to the database
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
