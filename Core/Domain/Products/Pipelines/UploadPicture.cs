using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;using Microsoft.AspNetCore.Hosting;

namespace SmaHauJenHoaVij.Core.Domain.Products.Pipelines
{
    public class UploadPicture
    {
        public record Request(Guid FoodItemId, IFormFile Picture) : IRequest<Response>;

        public record Response(bool Success, string FilePath, string[] Errors);

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IWebHostEnvironment _environment;

            public Handler(IWebHostEnvironment environment)
            {
                _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (request.Picture == null || request.Picture.Length == 0)
                {
                    return new Response(false, string.Empty, new[] { "No file provided" });
                }

                try
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "fooditems");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = $"{request.FoodItemId}{Path.GetExtension(request.Picture.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    await request.Picture.CopyToAsync(fileStream);

                    return new Response(true, $"/images/fooditems/{fileName}", Array.Empty<string>());
                }
                catch (Exception ex)
                {
                    return new Response(false, string.Empty, new[] { ex.Message });
                }
            }
        }
    }
}
