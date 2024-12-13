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
    public class GetOrderByCustomerId
    {
        public record Request(Guid CustomerId, bool OnlyActive) : IRequest<List<Response>>;

        public record Response(Guid OrderId, string Status, string Location, decimal TotalPrice, DateTime OrderDate);

        public class Handler : IRequestHandler<Request, List<Response>>
        {
            private readonly ShopContext _db;

            public Handler(ShopContext db)
            {
                _db = db;
            }

            public async Task<List<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                // Fetch orders with their lines from the database
                var orders = await _db.Orders
                    .Include(o => o.OrderLines) // Ensure OrderLines are eagerly loaded
                    .Where(o => o.CustomerId == request.CustomerId)
                    .ToListAsync(cancellationToken);

                if (request.OnlyActive)
                {
                    orders = orders
                        .Where(o => o.Status == Status.Placed || o.Status == Status.Accepted || o.Status == Status.Picked_Up)
                        .ToList();
                }

                // Perform aggregation in memory
                return orders.Select(o => new Response(
                    o.Id,
                    o.Status.ToString(),
                    $"{o.Location.Building}, {o.Location.RoomNumber}",
                    o.OrderLines.Sum(ol => ol.Price * ol.Amount) + o.DeliveryFee.Value,
                    o.OrderDate
                ))
                .ToList();
            }

        }
    }
}
