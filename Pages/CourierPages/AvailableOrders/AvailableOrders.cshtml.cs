using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SmaHauJenHoaVij.Core.Domain.Fulfillment.Pipelines;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain.Fulfillment.Pipelines;

namespace SmaHauJenHoaVij.Pages.CourierPages
{
    public class AvailableOrdersModel : PageModel
    {
        private readonly IMediator _mediator;

        public AvailableOrdersModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<GetAvailableOffers.Response> Offers { get; set; } = new();

        public async Task OnGetAsync()
        {
            Offers = await _mediator.Send(new GetAvailableOffers.Request());
        }

        public async Task<IActionResult> OnPostAcceptOfferAsync(Guid OfferId)
        {
            var courierId = Guid.Parse(HttpContext.Session.GetString("UserId")!);
            await _mediator.Send(new AssignCourierToOffer.Request(OfferId, courierId));
            return RedirectToPage();
        }
    }
}