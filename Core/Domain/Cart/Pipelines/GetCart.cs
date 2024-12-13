using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;

namespace SmaHauJenHoaVij.Core.Domain.Cart.Pipelines
{
    public class GetCart
    {
        public record Request(Guid CartId) : IRequest<ShoppingCart?>;

        public class Handler : IRequestHandler<Request, ShoppingCart?>
        {
            private readonly ShopContext _db;

            public Handler(ShopContext db)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
            }

            public async Task<ShoppingCart?> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _db.ShoppingCarts
                    .Include(c => c.Items)
                    .SingleOrDefaultAsync(c => c.Id == request.CartId, cancellationToken);
            }
        }
    }
}
