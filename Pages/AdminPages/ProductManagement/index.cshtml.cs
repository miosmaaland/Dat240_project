using Microsoft.AspNetCore.Mvc.RazorPages;
using SmaHauJenHoaVij.Core.Domain.Products.Pipelines;
using SmaHauJenHoaVij.Core.Domain.Products;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;

namespace SmaHauJenHoaVij.Pages.AdminPages.ProductManagement
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<FoodItem> FoodItems { get; set; }

        public async Task OnGetAsync()
        {
            var request = new Get.Request();
            FoodItems = await _mediator.Send(request);
        }
    }
}
