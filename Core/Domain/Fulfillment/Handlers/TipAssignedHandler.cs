using MediatR;
using SmaHauJenHoaVij.Core.Domain.Invoicing.Events;
using SmaHauJenHoaVij.Infrastructure.Data;
using SmaHauJenHoaVij.Core.Domain.Fulfillment;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SmaHauJenHoaVij.Core.Domain.Fulfillment.Handlers
{
    public class TipAssignedHandler : INotificationHandler<InvoicePaid>
    {
        private readonly ShopContext _db;

        public TipAssignedHandler(ShopContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task Handle(InvoicePaid notification, CancellationToken cancellationToken)
        {
            // Step 1: Retrieve the invoice
            var invoice = await _db.Invoices.FindAsync(notification.InvoiceId);

            if (invoice == null)
            {
                throw new Exception($"Invoice with ID {notification.InvoiceId} not found.");
            }

            if (invoice.Tip <= 0)
            {
                Console.WriteLine($"No tip found for Invoice {invoice.Id}");
                return;
            }

            // Step 2: Retrieve the Offer using the OrderId from the Invoice
            var offer = await _db.Offers.FirstOrDefaultAsync(o => o.OrderId == invoice.OrderId);
            if (offer == null)
            {
                throw new Exception($"No Offer found for OrderId {invoice.OrderId}");
            }

            // Step 3: Retrieve the Reimbursement from the Offer
            var reimbursement = await _db.Reimbursements.FindAsync(offer.Reimbursement);
            if (reimbursement == null)
            {
                throw new Exception($"No Reimbursement found for Offer {offer.Id}");
            }

            // Step 4: Assign the tip to the Reimbursement
            reimbursement.AssignTip(invoice.Tip);

            // Step 5: Update the Offer with the tip
            offer.AssignTip(invoice.Tip);

            // Step 6: Save changes
            await _db.SaveChangesAsync(cancellationToken);

            Console.WriteLine($"Tip of {invoice.Tip} assigned to Reimbursement {reimbursement.Id} and Offer {offer.Id}");
        }
    }
}