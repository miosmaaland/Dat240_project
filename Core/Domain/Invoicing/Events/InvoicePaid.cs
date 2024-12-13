using MediatR;
using SmaHauJenHoaVij.SharedKernel;

namespace SmaHauJenHoaVij.Core.Domain.Invoicing.Events
{
    public record InvoicePaid : BaseDomainEvent
    {
        public Guid InvoiceId { get; }

        public InvoicePaid(Guid invoiceId)
        {
            InvoiceId = invoiceId;
        }
    }
}
