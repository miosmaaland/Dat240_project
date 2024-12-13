using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;
using SmaHauJenHoaVij.Core.Domain.Cart;

namespace SmaHauJenHoaVij.Core.Domain.Cart.Pipelines;

public class GetCartByCustomerId
{
    public record Request(Guid CustomerId) : IRequest<ShoppingCart?>;

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
                .Include(sc => sc.Items)
                .SingleOrDefaultAsync(sc => sc.CustomerId == request.CustomerId, cancellationToken);
        }
    }
}
