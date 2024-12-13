using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;

namespace SmaHauJenHoaVij.Core.Domain.Invoicing.Pipelines
{
    public class GetInvoicesByStatus
    {
        public record Request(InvoiceStatus Status) : IRequest<List<Response>>;

        public record Response(Guid InvoiceId, Guid CustomerId, decimal Amount, Guid OrderId, InvoiceStatus Status);

        public class Handler : IRequestHandler<Request, List<Response>>
        {
            private readonly ShopContext _db;

            public Handler(ShopContext db)
            {
                _db = db;
            }

            public async Task<List<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _db.Invoices
                    .Where(i => i.Status == request.Status)
                    .Select(i => new Response(i.Id, i.CustomerId, i.Amount, i.OrderId, i.Status))
                    .ToListAsync(cancellationToken);
            }
        }
    }
}
