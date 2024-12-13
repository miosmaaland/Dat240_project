using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.SharedKernel;
using SmaHauJenHoaVij.Infrastructure.Data;
using SmaHauJenHoaVij.Core.Exceptions;


namespace SmaHauJenHoaVij.Core.Domain.Products.Pipelines;

public class Edit
{
    public record Request(Guid Id, string Name, string Description, decimal Price) : IRequest<Response>;

    public record Response(bool Success, string[] Errors);

    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ShopContext _db;
        private readonly IEnumerable<IValidator<FoodItem>> _validators;

        public Handler(ShopContext db, IEnumerable<IValidator<FoodItem>> validators)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var existingItem = await _db.FoodItems.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
            if (existingItem is null) throw new EntityNotFoundException($"FoodItem with Id {request.Id} was not found in the database");

            existingItem.Name = request.Name;
            existingItem.Description = request.Description;
            existingItem.Price = request.Price;

            var errors = _validators.Select(v => v.IsValid(existingItem))
                        .Where(result => !result.IsValid)
                        .Select(result => result.Error)
                        .ToArray();
            if (errors.Length > 0)
            {
                return new Response(Success: false, errors);
            }

            await _db.SaveChangesAsync(cancellationToken);
            return new Response(true, Array.Empty<string>());
        }
    }
}
