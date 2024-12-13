using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using SmaHauJenHoaVij.Core.Domain.Ordering.Pipelines;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmaHauJenHoaVij.Core.Domain.Ordering.Services;

namespace SmaHauJenHoaVij.Pages.CustomerPages.Orders.MyOrder
{
    public class IndexModel : PageModel
    {
        private readonly IOrderingService _orderingService;
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator, IOrderingService orderingService)
        {
            _mediator = mediator;
            _orderingService = orderingService ?? throw new ArgumentNullException(nameof(orderingService));
        }

        public List<GetOrderByCustomerId.Response> Orders { get; set; } = new();

        public async Task OnGetAsync()
        {
            var customerId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            Orders = await _mediator.Send(new GetOrderByCustomerId.Request(customerId, OnlyActive: true));
        }


        public async Task<IActionResult> OnPostCancelOrderAsync(Guid orderId)
        {
            try
            {
                await _orderingService.CancelOrder(orderId);
                TempData["Message"] = "Order canceled successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Failed to cancel order: {ex.Message}";
            }
            return RedirectToPage();
        }
    }
}
