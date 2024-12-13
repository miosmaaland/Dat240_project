using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using SmaHauJenHoaVij.Core.Domain.Ordering.Pipelines;
using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Pages.AdminPages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public GetOrderStats.Response Stats { get; set; }

        public async Task OnGetAsync()
        {
            Stats = await _mediator.Send(new GetOrderStats.Request());
        }
    }
}
