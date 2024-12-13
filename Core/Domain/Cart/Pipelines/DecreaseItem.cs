using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;

namespace SmaHauJenHoaVij.Core.Domain.Cart.Pipelines
{
    public class DecreaseItem
    {
        public record Request(Guid CartId, Guid Sku) : IRequest<Unit>;

        public class Handler : IRequestHandler<Request, Unit>
        {
            private readonly ShopContext _db;

            public Handler(ShopContext db)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                // Fetch the cart
                var cart = await _db.ShoppingCarts
                    .Include(c => c.Items)
                    .SingleOrDefaultAsync(c => c.Id == request.CartId, cancellationToken);

                if (cart == null)
                    throw new InvalidOperationException($"Cart with Id {request.CartId} does not exist.");

                // Find the item in the cart
                var item = cart.Items.SingleOrDefault(i => i.Sku == request.Sku);
                if (item != null)
                {
                    item.RemoveOne();

                    if (item.Count <= 0)
                    {
                        // Remove the item entirely if the count is 0
                        cart.RemoveItem(request.Sku);
                    }

                    _db.ShoppingCarts.Update(cart); // Update the cart
                    await _db.SaveChangesAsync(cancellationToken);
                }

                return Unit.Value;
            }
        }
    }
}
