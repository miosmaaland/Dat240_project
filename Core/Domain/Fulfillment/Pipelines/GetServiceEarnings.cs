using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;

namespace SmaHauJenHoaVij.Core.Domain.Fulfillment.Pipelines
{
    public class GetServiceEarnings
    {
        public record Request : IRequest<Response>;

        public record Response(decimal ServiceEarnings);

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ShopContext _db;

            public Handler(ShopContext db)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                // Fetch all reimbursements into memory
                var reimbursements = await _db.Reimbursements
                    .ToListAsync(cancellationToken);

                // Perform summation in memory
                var totalReimbursementAmount = reimbursements.Sum(r => r.Amount);

                // Calculate service earnings (20% of total earnings)
                var serviceEarnings = (totalReimbursementAmount / 0.8m) * 0.2m;

                return new Response(serviceEarnings);
            }
        }
    }
}
