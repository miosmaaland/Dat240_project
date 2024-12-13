using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmaHauJenHoaVij.Core.Domain.Products.Pipelines;
using System.Threading.Tasks;
using MediatR;

namespace SmaHauJenHoaVij.Pages.AdminPages.ProductManagement
{
    public class DeleteModel : PageModel
    {
        private readonly IMediator _mediator;

        public DeleteModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Id { get; set; }

        public async Task OnGetAsync(Guid id)
        {
            var foodItem = await _mediator.Send(new GetById.Request(id));
            Id = foodItem.Id;
            Name = foodItem.Name;
            Description = foodItem.Description;
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            await _mediator.Send(new Delete.Request(id));
            return RedirectToPage("Index");
        }
    }
}
