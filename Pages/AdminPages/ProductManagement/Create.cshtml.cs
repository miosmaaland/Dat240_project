using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmaHauJenHoaVij.Core.Domain.Products.Pipelines;
using System.Threading.Tasks;
using MediatR;

namespace SmaHauJenHoaVij.Pages.AdminPages.ProductManagement
{
    public class CreateModel : PageModel
    {
        private readonly IMediator _mediator;

        public CreateModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public decimal Price { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var request = new Create.Request(Name, Description, Price);
            var response = await _mediator.Send(request);

            if (!response.Success)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return Page();
            }

            return RedirectToPage("Index");
        }
    }
}
