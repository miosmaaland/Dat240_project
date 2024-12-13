using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmaHauJenHoaVij.Core.Domain.Products.Pipelines;
using SmaHauJenHoaVij.Core.Domain.Cart.Pipelines;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using SmaHauJenHoaVij.Core.Domain.Products;

namespace SmaHauJenHoaVij.Pages.CustomerPages.Shopping
{
    public class BrowseItemsModel : PageModel
    {
        private readonly IMediator _mediator;

        public BrowseItemsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<FoodItem> FoodItems { get; set; }

        public async Task OnGetAsync()
        {
            var request = new Get.Request();
            FoodItems = await _mediator.Send(request);
        }

        public async Task<IActionResult> OnPostAddToCartAsync(Guid ItemId, string ItemName, decimal ItemPrice)
        {
            var cartId = Guid.Parse(HttpContext.Session.GetString("CartId"));
            var customerId = Guid.Parse(HttpContext.Session.GetString("UserId"));

            // Use ItemId directly as it is already a Guid
            await _mediator.Send(new AddItem.Request(cartId, customerId, ItemId, ItemName, ItemPrice));

            TempData["Message"] = "Item added to cart!";
            return RedirectToPage();
        }

    }
}
