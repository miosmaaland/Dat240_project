using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;
using SmaHauJenHoaVij.Core.Exceptions;

namespace SmaHauJenHoaVij.Core.Domain.Products.Pipelines
{
    public class UpdatePicture
    {
        public record Request(Guid FoodItemId, string PicturePath) : IRequest<Response>;

        public record Response(bool Success, string[] Errors);

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ShopContext _db;

            public Handler(ShopContext db)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var foodItem = await _db.FoodItems.SingleOrDefaultAsync(fi => fi.Id == request.FoodItemId, cancellationToken);

                if (foodItem == null)
                {
                    return new Response(false, new[] { $"FoodItem with Id {request.FoodItemId} was not found" });
                }

                try
                {
                    foodItem.UpdatePicture(Picture.FromPath(request.PicturePath));
                    await _db.SaveChangesAsync(cancellationToken);

                    return new Response(true, Array.Empty<string>());
                }
                catch (Exception ex)
                {
                    return new Response(false, new[] { ex.Message });
                }
            }
        }
    }
}
