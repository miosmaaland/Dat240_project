using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;

namespace SmaHauJenHoaVij.Core.Domain.Ordering.Pipelines
{
    public class GetAllOrders
    {
        public record Request : IRequest<List<Response>>;

        public record Response(
            Guid OrderId,
            string CustomerName,
            string Status,
            decimal TotalPrice,
            DateTime OrderDate);

        public class Handler : IRequestHandler<Request, List<Response>>
        {
            private readonly ShopContext _db;

            public Handler(ShopContext db)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
            }

            public async Task<List<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                // Fetch orders with their order lines
                var orders = await _db.Orders
                    .Include(o => o.OrderLines) // Fetch order lines
                    .ToListAsync(cancellationToken);

                // Calculate the total price in memory
                return orders.Select(o => new Response(
                    o.Id,
                    o.CustomerName,
                    o.Status.ToString(),
                    o.OrderLines.Sum(ol => ol.Price * ol.Amount) + o.DeliveryFee.Value, // Sum in memory
                    o.OrderDate
                )).ToList();
            }
        }
    }
}
