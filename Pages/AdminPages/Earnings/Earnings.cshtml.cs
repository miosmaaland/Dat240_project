using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using SmaHauJenHoaVij.Core.Domain.Fulfillment.Pipelines;
using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Pages.AdminPages.Earnings
{
    public class EarningsModel : PageModel
    {
        private readonly IMediator _mediator;

        public EarningsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public decimal ServiceEarnings { get; set; }

        public async Task OnGetAsync()
        {
            var response = await _mediator.Send(new GetServiceEarnings.Request());
            ServiceEarnings = response.ServiceEarnings;
        }
    }
}
