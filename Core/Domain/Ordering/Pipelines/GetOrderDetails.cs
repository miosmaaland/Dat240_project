using MediatR;
using SmaHauJenHoaVij.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Core.Domain.Ordering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmaHauJenHoaVij.Core.Domain.Ordering.Dtos;

namespace SmaHauJenHoaVij.Core.Domain.Ordering.Pipelines;

public class GetOrderDetails
{
    public record Request(Guid OrderId) : IRequest<Response>;

    public record Response(Guid OrderId, List<OrderLineDto> OrderLines);

    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ShopContext _db;

        public Handler(ShopContext db)
        {
            _db = db;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var order = await _db.Orders
                .Where(o => o.Id == request.OrderId)
                .Select(o => new Response(
                    o.Id,
                    o.OrderLines.Select(ol => new OrderLineDto(
                        ol.FoodItemId,
                        ol.ItemName,
                        ol.Amount,
                        ol.Price
                    )).ToList()
                ))
                .FirstOrDefaultAsync(cancellationToken);

            if (order == null)
                throw new Exception($"Order with ID {request.OrderId} not found.");

            return order;
        }
    }
}
