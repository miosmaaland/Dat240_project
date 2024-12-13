using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmaHauJenHoaVij.Infrastructure.Data;
using SmaHauJenHoaVij.Core.Domain.Cart;

namespace SmaHauJenHoaVij.Core.Domain.Cart.Pipelines
{
    public class CreateCart
    {
        public record Request(Guid CartId, Guid CustomerId) : IRequest<Unit>; // Specify Unit explicitly

        public class Handler : IRequestHandler<Request, Unit> // Specify Unit as the response type
        {
            private readonly ShopContext _db;

            public Handler(ShopContext db)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                // Use both CartId and CustomerId in the constructor
                var cart = new ShoppingCart(request.CartId, request.CustomerId);

                _db.ShoppingCarts.Add(cart);
                await _db.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
