using System;
using System.Threading;
using System.Threading.Tasks;
using SmaHauJenHoaVij.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Core.Exceptions;

namespace SmaHauJenHoaVij.Core.Domain.Products.Pipelines
{
    public class Delete
    {
        public record Request(Guid Id) : IRequest<Unit>; // Explicitly specify IRequest<Unit>

        public class Handler : IRequestHandler<Request, Unit>
        {
            private readonly ShopContext _db;

            public Handler(ShopContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var item = await _db.FoodItems.SingleOrDefaultAsync(fi => fi.Id == request.Id, cancellationToken);
                if (item is null) 
                    throw new EntityNotFoundException($"FoodItem with Id {request.Id} was not found in the database");

                _db.FoodItems.Remove(item);
                await _db.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
