using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SmaHauJenHoaVij.Core.Domain.Fulfillment.Pipelines;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Pages.CourierPages
{
    public class MyOrdersModel : PageModel
    {
        private readonly IMediator _mediator;

        public MyOrdersModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<GetMyOrders.Response> Orders { get; set; } = new();

        public async Task OnGetAsync()
        {
            var courierId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            Orders = await _mediator.Send(new GetMyOrders.Request(courierId));
        }

        public async Task<IActionResult> OnPostMarkAsPickedUpAsync(Guid OfferId)
        {
            await _mediator.Send(new PickUpOrder.Request(OfferId));
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostMarkAsDeliveredAsync(Guid OfferId)
        {
            await _mediator.Send(new DeliverOrder.Request(OfferId));
            return RedirectToPage();
        }
    }
}
