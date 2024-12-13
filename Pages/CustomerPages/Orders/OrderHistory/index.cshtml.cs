using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using SmaHauJenHoaVij.Core.Domain.Ordering.Pipelines;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Pages.CustomerPages.Orders.OrderHistory
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<GetOrderByCustomerId.Response> Orders { get; set; } = new();

        public async Task OnGetAsync()
        {
            var customerId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            Orders = await _mediator.Send(new GetOrderByCustomerId.Request(customerId, OnlyActive: false));
        }
    }
}
