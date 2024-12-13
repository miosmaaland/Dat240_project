using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;

namespace SmaHauJenHoaVij.Core.Domain.Fulfillment.Pipelines
{
    public class GetCourierEarnings
    {
        public record Request(Guid CourierId) : IRequest<Response>;

        public record Response(decimal TotalEarnings, List<MonthlyEarnings> MonthlyEarnings);

        public record MonthlyEarnings(string Month, decimal Earnings);

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ShopContext _db;

            public Handler(ShopContext db)
            {
                _db = db;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var reimbursements = await _db.Reimbursements
                    .Where(r => r.CourierId == request.CourierId)
                    .ToListAsync(cancellationToken);

                var totalEarnings = reimbursements.Sum(r => r.Amount + r.Tip);

                var monthlyEarnings = reimbursements
                    .GroupBy(r => new { r.CreatedAt.Year, r.CreatedAt.Month })
                    .Select(g => new MonthlyEarnings(
                        $"{g.Key.Year}-{g.Key.Month:00}",
                        g.Sum(r => r.Amount + r.Tip)
                    ))
                    .OrderBy(me => me.Month)
                    .ToList();

                return new Response(totalEarnings, monthlyEarnings);
            }
        }
    }
}
