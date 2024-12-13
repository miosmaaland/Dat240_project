using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using SmaHauJenHoaVij.Core.Domain.Fulfillment.Pipelines;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Pages.CourierPages.OrderHistory
{
    public class OrderHistoryModel : PageModel
    {
        private readonly IMediator _mediator;

        public OrderHistoryModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<GetOrderHistory.Response> Orders { get; set; } = new();

        public async Task OnGetAsync()
        {
            var courierId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            Orders = await _mediator.Send(new GetOrderHistory.Request(courierId));
        }
    }
}
