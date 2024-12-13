/**

using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;

namespace SmaHauJenHoaVij.Core.Domain.Invoicing.Pipelines
{
    public class MarkInvoiceAsPaid
    {
        public record Request(Guid InvoiceId) : IRequest;

        public class Handler : IRequestHandler<Request>
        {
            private readonly ShopContext _db;

            public Handler(ShopContext db)
            {
                _db = db;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var invoice = await _db.Invoices.FindAsync(request.InvoiceId);

                if (invoice == null)
                    throw new Exception($"Invoice with ID {request.InvoiceId} not found.");

                invoice.MarkAsPaid();
                await _db.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
**/