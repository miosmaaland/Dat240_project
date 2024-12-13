using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;

namespace SmaHauJenHoaVij.Core.Domain.Ordering.Pipelines
{
    public class GetOrderStats
    {
        public record Request : IRequest<Response>;

        public record OrderStats(int Open, int BeingDelivered, int Delivered, int Total);

        public record Response(OrderStats TotalStats, OrderStats MonthToDateStats);

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ShopContext _db;

            public Handler(ShopContext db)
            {
                _db = db;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var currentMonthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

                var orders = await _db.Orders
                    .Select(o => new
                    {
                        o.Status,
                        o.OrderDate
                    })
                    .ToListAsync(cancellationToken);

                // Calculate stats
                var totalStats = CalculateStats(orders);
                var monthToDateStats = CalculateStats(orders.Where(o => o.OrderDate >= currentMonthStart).ToList());

                return new Response(totalStats, monthToDateStats);
            }

            private OrderStats CalculateStats(IEnumerable<dynamic> orders)
            {
                var open = orders.Count(o => o.Status == Status.Placed || o.Status == Status.Accepted);
                var beingDelivered = orders.Count(o => o.Status == Status.Picked_Up);
                var delivered = orders.Count(o => o.Status == Status.Delivered);

                return new OrderStats(open, beingDelivered, delivered, orders.Count());
            }
        }
    }
}
