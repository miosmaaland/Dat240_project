using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;

namespace SmaHauJenHoaVij.Core.Domain.Cart.Pipelines
{
    public class RemoveItem
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
                var cart = await _db.ShoppingCarts
                    .Include(c => c.Items)
                    .SingleOrDefaultAsync(c => c.Id == request.CartId, cancellationToken);

                if (cart == null)
                {
                    throw new InvalidOperationException($"Cart with Id {request.CartId} does not exist.");
                }

                cart.RemoveItem(request.Sku);

                _db.ShoppingCarts.Update(cart);
                await _db.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
