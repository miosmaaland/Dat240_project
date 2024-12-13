using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Core.Domain.Invoicing.Pipelines
{
    public class GetInvoiceDetails
    {
        public record Request(Guid InvoiceId) : IRequest<Response>;

        public record Response(Guid InvoiceId, Guid OrderId, decimal Amount, string Status);

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ShopContext _db;

            public Handler(ShopContext db)
            {
                _db = db;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var invoice = await _db.Invoices
                    .Where(i => i.Id == request.InvoiceId)
                    .Select(i => new Response(
                        i.Id,
                        i.OrderId,
                        i.Amount,
                        i.Status.ToString()
                    ))
                    .FirstOrDefaultAsync(cancellationToken);

                return invoice ?? throw new Exception($"Invoice with ID {request.InvoiceId} not found.");
            }
        }
    }
}
