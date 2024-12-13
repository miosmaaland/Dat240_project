using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Core.Domain.Invoicing.Pipelines;

public class GetCustomerInvoices
{
    public record Request(Guid CustomerId) : IRequest<List<Response>>;

    public record Response(Guid Id, Guid OrderId, decimal Amount, string Status);

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
                .Where(i => i.CustomerId == request.CustomerId && i.Status == InvoiceStatus.Sent) // Filter sent invoices
                .Select(i => new Response(
                    i.Id,
                    i.OrderId,
                    i.Amount,
                    i.Status.ToString()
                ))
                .ToListAsync(cancellationToken);
        }
    }
}

