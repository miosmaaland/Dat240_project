using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using SmaHauJenHoaVij.Core.Domain.Fulfillment.Pipelines;
using System;
using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Pages.CourierPages.Earnings
{
    public class EarningsModel : PageModel
    {
        private readonly IMediator _mediator;

        public EarningsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public GetCourierEarnings.Response Earnings { get; set; }

        public async Task OnGetAsync()
        {
            var courierId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            Earnings = await _mediator.Send(new GetCourierEarnings.Request(courierId));
        }
    }
}
