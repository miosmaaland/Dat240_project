using System;

namespace SmaHauJenHoaVij.Core.Domain.Fulfillment
{
    public class Reimbursement
    {
        public Guid Id { get; private set; }
        public Guid? CourierId { get; private set; } // Nullable to represent unassigned state
        public decimal Amount { get; private set; }
        public decimal Tip { get; private set; }
        public Guid? InvoiceId { get; private set; }
        public DateTime CreatedAt { get; private set; } // New field to track creation time

        private Reimbursement() { } // EF Core

        public Reimbursement(decimal amount)
        {
            Id = Guid.NewGuid();
            Amount = amount;
            Tip = 0;
            CourierId = null;
            InvoiceId = null;
            CreatedAt = DateTime.UtcNow; // Set creation time to now
        }

        public void AssignCourier(Guid courierId)
        {
            CourierId = courierId;
        }

        public void AssignInvoice(Guid invoiceId)
        {
            InvoiceId = invoiceId;
        }

        public void AssignTip(decimal tipAmount)
        {
            Tip = tipAmount;
        }

        public decimal GetTotalPayout()
        {
            return Amount + Tip;
        }
    }
}
