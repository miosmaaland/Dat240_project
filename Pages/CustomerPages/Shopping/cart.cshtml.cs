using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmaHauJenHoaVij.Core.Domain.Cart.Pipelines;
using SmaHauJenHoaVij.Core.Domain.Products.Pipelines;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SmaHauJenHoaVij.Models;
using SmaHauJenHoaVij.Core.Domain.Ordering.Services;
using SmaHauJenHoaVij.Core.Domain.Ordering;

namespace SmaHauJenHoaVij.Pages.CustomerPages.Shopping
{
    public class CartModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly DeliveryFeeService _deliveryFeeService;

        public CartModel(IMediator mediator, DeliveryFeeService deliveryFeeService)
        {
            _mediator = mediator;
            _deliveryFeeService = deliveryFeeService;
        }

        public List<CartItemViewModel> CartItems { get; set; } = new();

        public int TotalItems => CartItems.Sum(i => i.Count);
        public decimal SubTotalPrice => CartItems.Sum(i => i.TotalPrice);
        public decimal DeliveryFee => _deliveryFeeService.GetCurrentFee().Value;
        public decimal TotalPrice => SubTotalPrice + DeliveryFee;

        public async Task OnGetAsync()
        {
            var cartId = Guid.Parse(HttpContext.Session.GetString("CartId")!);
            var cart = await _mediator.Send(new GetCart.Request(cartId));

            if (cart?.Items != null)
            {
                foreach (var cartItem in cart.Items)
                {
                    var foodItem = await _mediator.Send(new GetById.Request(cartItem.Sku));
                    CartItems.Add(new CartItemViewModel
                    {
                        Sku = cartItem.Sku,
                        Name = foodItem!.Name,
                        Price = foodItem.Price,
                        Count = cartItem.Count,
                        PicturePath = foodItem.Picture.Path
                    });
                }
            }
        }

        public async Task<IActionResult> OnPostIncreaseItemCountAsync(Guid Sku)
        {
            var cartId = Guid.Parse(HttpContext.Session.GetString("CartId")!);

            await _mediator.Send(new AddItem.Request(cartId, Guid.Empty, Sku, string.Empty, 0));
            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostDecreaseItemCountAsync(Guid Sku)
        {
            var cartId = Guid.Parse(HttpContext.Session.GetString("CartId")!);

            // Use the new DecreaseItem pipeline
            await _mediator.Send(new DecreaseItem.Request(cartId, Sku));

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveItemAsync(Guid Sku)
        {
            var cartId = Guid.Parse(HttpContext.Session.GetString("CartId")!);

            // Use the new RemoveAllItems pipeline
            await _mediator.Send(new RemoveItem.Request(cartId, Sku));

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCheckoutAsync(string Building, string RoomNumber, string? LocationNotes, string? OrderNotes)
        {
            var cartId = Guid.Parse(HttpContext.Session.GetString("CartId")!);
            var customerId = Guid.Parse(HttpContext.Session.GetString("UserId")!);

            var location = new Location(Building, RoomNumber, LocationNotes ?? string.Empty);
            var orderId = await _mediator.Send(new CartCheckout.Request(customerId, location));

            TempData["Message"] = $"Order placed successfully! Order ID: {orderId}";
            return RedirectToPage("/index");
        }

    }
}
