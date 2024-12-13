using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;

namespace SmaHauJenHoaVij.Core.Domain.Fulfillment.Pipelines
{
    public class GetAvailableOffers
    {
        public record Request : IRequest<List<Response>>;

        public record Response(Guid OfferId, Guid OrderId, string CustomerName, string Location, decimal ReimbursementAmount);

        public class Handler : IRequestHandler<Request, List<Response>>
        {
            private readonly ShopContext _db;

            public Handler(ShopContext db)
            {
                _db = db;
            }

            public async Task<List<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                var offers = await _db.Offers
                    .Where(o => o.CourierId == null) // Unassigned offers
                    .Join(
                        _db.Reimbursements,
                        offer => offer.Reimbursement,
                        reimbursement => reimbursement.Id,
                        (offer, reimbursement) => new { offer, reimbursement }
                    )
                    .Join(
                        _db.Orders,
                        result => result.offer.OrderId,
                        order => order.Id,
                        (result, order) => new
                        {
                            result.offer,
                            result.reimbursement,
                            order.CustomerId,
                            OrderLocation = order.Location, // Alias for the location object
                            OrderId = order.Id // Alias for order ID
                        }
                    )
                    .Join(
                        _db.Customers,
                        result => result.CustomerId,
                        customer => customer.Id,
                        (result, customer) => new Response(
                            result.offer.Id, // Offer ID
                            result.OrderId, // Order ID
                            customer.Name, // Customer Name
                            $"{result.OrderLocation.Building}, {result.OrderLocation.RoomNumber}", // Formatted Location
                            result.reimbursement.Amount // Reimbursement Amount
                        )
                    )
                    .ToListAsync(cancellationToken);

                return offers;
            }
        }


    }
}