using System;
using System.Threading;
using System.Threading.Tasks;
using SmaHauJenHoaVij.Core.Exceptions;
using SmaHauJenHoaVij.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Core.Domain.Products;

namespace SmaHauJenHoaVij.Core.Domain.Products.Pipelines;

public class GetById
{
    public record Request(Guid Id) : IRequest<FoodItem?>;

    public class Handler : IRequestHandler<Request, FoodItem?>
    {
        private readonly ShopContext _db;

        public Handler(ShopContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

        public async Task<FoodItem?> Handle(Request request, CancellationToken cancellationToken)
        {
            var item = await _db.FoodItems.SingleOrDefaultAsync(fi => fi.Id == request.Id, cancellationToken);
            if (item is null) throw new EntityNotFoundException($"FoodItem with Id {request.Id} was not found in the database");
            return item;
        }
    }
}
