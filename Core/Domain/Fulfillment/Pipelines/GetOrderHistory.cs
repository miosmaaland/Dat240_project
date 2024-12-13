using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;

namespace SmaHauJenHoaVij.Core.Domain.Fulfillment.Pipelines
{
    public class GetOrderHistory
    {
        public record Request(Guid CourierId) : IRequest<List<Response>>;

        public record Response(Guid OfferId, Guid OrderId, string Status, decimal ReimbursementAmount);

        public class Handler : IRequestHandler<Request, List<Response>>
        {
            private readonly ShopContext _db;

            public Handler(ShopContext db)
            {
                _db = db;
            }

            public async Task<List<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _db.Offers
                    .Where(o => o.CourierId == request.CourierId)
                    .Join(
                        _db.Reimbursements,
                        offer => offer.Reimbursement,
                        reimbursement => reimbursement.Id,
                        (offer, reimbursement) => new Response(
                            offer.Id,
                            offer.OrderId,
                            _db.Orders.First(o => o.Id == offer.OrderId).Status.ToString(),
                            reimbursement.Amount
                        )
                    )
                    .ToListAsync(cancellationToken);
            }
        }
    }
}
