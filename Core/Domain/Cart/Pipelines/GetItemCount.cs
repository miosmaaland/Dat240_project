using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;

namespace SmaHauJenHoaVij.Core.Domain.Cart.Pipelines
{
    public class GetItemCount
    {
        public record Request(Guid CartId) : IRequest<int>;

        public class Handler : IRequestHandler<Request, int>
        {
            private readonly ShopContext _db;

            public Handler(ShopContext db)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
            }

            public async Task<int> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _db.ShoppingCarts
                    .Include(c => c.Items)
                    .Where(c => c.Id == request.CartId)
                    .SelectMany(c => c.Items)
                    .SumAsync(i => i.Count, cancellationToken);
            }
        }
    }
}
