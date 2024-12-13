using MediatR;
using SmaHauJenHoaVij.SharedKernel;

namespace SmaHauJenHoaVij.Core.Domain.Invoicing.Events
{
    public record InvoiceCancelled : BaseDomainEvent
    {
        public Guid InvoiceId { get; }

        public InvoiceCancelled(Guid invoiceId)
        {
            InvoiceId = invoiceId;
        }
    }
}