using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using SmaHauJenHoaVij.Core.Domain.Products.Pipelines;
using SmaHauJenHoaVij.Core.Domain.Products;
using System.IO;
using System.Threading.Tasks;
using MediatR;

namespace SmaHauJenHoaVij.Pages.AdminPages.ProductManagement
{
    public class EditModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _environment;

        public EditModel(IMediator mediator, IWebHostEnvironment environment)
        {
            _mediator = mediator;
            _environment = environment;
        }

        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public decimal Price { get; set; }
        [BindProperty]
        public IFormFile Picture { get; set; }
        public string PicturePath { get; set; }
        public Guid Id { get; set; }

        public async Task OnGetAsync(Guid id)
        {
            var foodItem = await _mediator.Send(new GetById.Request(id));
            Id = foodItem.Id;
            Name = foodItem.Name;
            Description = foodItem.Description;
            Price = foodItem.Price;
            PicturePath = foodItem.Picture.Path;
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid)
                return Page();

            var editRequest = new Edit.Request(id, Name, Description, Price);
            var editResponse = await _mediator.Send(editRequest);

            if (!editResponse.Success)
            {
                foreach (var error in editResponse.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return Page();
            }

            if (Picture != null)
            {
                // Use UploadPicture pipeline
                var uploadResponse = await _mediator.Send(new UploadPicture.Request(id, Picture));
                if (!uploadResponse.Success)
                {
                    foreach (var error in uploadResponse.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                    return Page();
                }

                // Use UpdatePicture pipeline
                var updateResponse = await _mediator.Send(new UpdatePicture.Request(id, uploadResponse.FilePath));
                if (!updateResponse.Success)
                {
                    foreach (var error in updateResponse.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                    return Page();
                }
            }

            return RedirectToPage("Index");
        }

    }
}