using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;



namespace SmaHauJenHoaVij.Core.Domain.Products.Pipelines;

public class Get
{
    public record Request : IRequest<List<FoodItem>> { }

    public class Handler : IRequestHandler<Request, List<FoodItem>>
    {
        private readonly ShopContext _db;

        public Handler(ShopContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<List<FoodItem>> Handle(Request request, CancellationToken cancellationToken)
            => await _db.FoodItems.OrderBy(i => i.Name).ToListAsync(cancellationToken: cancellationToken);
    }
}
