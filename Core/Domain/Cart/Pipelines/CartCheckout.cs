using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Core.Domain.Ordering;
using SmaHauJenHoaVij.Core.Domain.Ordering.Dtos;
using SmaHauJenHoaVij.Core.Domain.Ordering.Services;
using SmaHauJenHoaVij.Infrastructure.Data;

namespace SmaHauJenHoaVij.Core.Domain.Cart.Pipelines
{
    public class CartCheckout
    {
        public record Request(Guid CustomerId, Location Location) : IRequest<Guid>; // Updated to return Guid

        public class Handler : IRequestHandler<Request, Guid> // Updated to return Guid
        {
            private readonly ShopContext _db;
            private readonly IOrderingService _orderingService;

            public Handler(ShopContext db, IOrderingService orderingService)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _orderingService = orderingService ?? throw new ArgumentNullException(nameof(orderingService));
            }

            public async Task<Guid> Handle(Request request, CancellationToken cancellationToken)
            {
                var cart = await _db.ShoppingCarts
                    .Include(c => c.Items)
                    .SingleOrDefaultAsync(c => c.CustomerId == request.CustomerId, cancellationToken);

                if (cart == null)
                {
                    throw new InvalidOperationException($"Cart for CustomerId {request.CustomerId} not found.");
                }

                var orderLines = cart.Items.Select(i => new OrderLineDto(
                    i.Sku, i.Name, i.Count, i.Price)).ToArray();

                // Use the ordering service to place the order
                var orderId = await _orderingService.PlaceOrder(request.CustomerId, request.Location, orderLines);

                // Remove the cart after checkout
                _db.ShoppingCarts.Remove(cart);
                await _db.SaveChangesAsync();

                return orderId; // Return the generated order ID
            }
        }
    }
}
