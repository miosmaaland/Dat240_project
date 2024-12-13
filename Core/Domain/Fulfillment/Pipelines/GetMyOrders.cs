using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;

namespace SmaHauJenHoaVij.Core.Domain.Fulfillment.Pipelines
{
    public class GetMyOrders
    {
        public record Request(Guid CourierId) : IRequest<List<Response>>;

        public record Response(
            Guid OfferId,
            Guid OrderId,
            string Status,
            decimal ReimbursementAmount,
            string CustomerName,
            string Location
        );

        public class Handler : IRequestHandler<Request, List<Response>>
        {
            private readonly ShopContext _db;

            public Handler(ShopContext db)
            {
                _db = db;
            }

            public async Task<List<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                var orders = await _db.Offers
                    .Where(o => o.CourierId == request.CourierId)
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
                            order.Status,
                            OrderLocation = order.Location,
                            OrderId = order.Id
                        }
                    )
                    .Join(
                        _db.Customers,
                        result => result.CustomerId,
                        customer => customer.Id,
                        (result, customer) => new Response(
                            result.offer.Id, 
                            result.OrderId, 
                            result.Status.ToString(), 
                            result.reimbursement.Amount, 
                            customer.Name, 
                            $"{result.OrderLocation.Building}, {result.OrderLocation.RoomNumber}"
                        )
                    )
                    .ToListAsync(cancellationToken);

                return orders;
            }
        }
    }
}