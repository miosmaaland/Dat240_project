using Microsoft.AspNetCore.Mvc.RazorPages;
using SmaHauJenHoaVij.Core.Domain.Ordering.Pipelines;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SmaHauJenHoaVij.Models;

namespace SmaHauJenHoaVij.Pages.AdminPages.Orders
{
    public class OrdersModel : PageModel
    {
        private readonly IMediator _mediator;

        public OrdersModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<AdminOrderViewModel> Orders { get; set; } = new();

        public async Task OnGetAsync()
        {
            var result = await _mediator.Send(new GetAllOrders.Request());

            // Map GetAllOrders.Response to OrderViewModel
            Orders = result.Select(order => new AdminOrderViewModel
            {
                OrderId = order.OrderId,
                CustomerName = order.CustomerName,
                Status = order.Status,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate
            }).ToList();
        }

    }
}
