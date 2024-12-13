using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;
using System;
using SmaHauJenHoaVij.Core.Domain.Cart;

namespace SmaHauJenHoaVij.Core.Domain.Cart.Pipelines
{
    public class AddItem
    {
        public record Request(
            Guid CartId,
            Guid CustomerId,
            Guid Sku,
            string Name,
            decimal Price
        ) : IRequest<Unit>;

        public class Handler : IRequestHandler<Request, Unit>
        {
            private readonly ShopContext _db;

            public Handler(ShopContext db)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var cart = await _db.ShoppingCarts
                    .Include(c => c.Items)
                    .SingleOrDefaultAsync(c => c.Id == request.CartId, cancellationToken);

                if (cart == null)
                {
                    cart = new ShoppingCart(request.CartId, request.CustomerId);
                    _db.ShoppingCarts.Add(cart);
                }

                cart.AddItem(request.Sku, request.Name, request.Price);

                await _db.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
