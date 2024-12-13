using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SmaHauJenHoaVij.Core.Domain.Invoicing.Pipelines;
using SmaHauJenHoaVij.Core.Domain.Ordering.Pipelines;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmaHauJenHoaVij.Core.Domain.Ordering.Dtos;
using SmaHauJenHoaVij.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace SmaHauJenHoaVij.Pages.CustomerPages.Payment
{
    public class DetailsModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly ShopContext _db;

        public DetailsModel(IMediator mediator, ShopContext db)
        {
            _mediator = mediator;
            _db = db;
        }

        public GetInvoiceDetails.Response Invoice { get; set; }
        public List<OrderLineDto> OrderLines { get; set; } = new();
        public decimal DeliveryFee { get; set; }
        [BindProperty]
        public decimal Tip { get; set; }
        

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Invoice = await _mediator.Send(new GetInvoiceDetails.Request(id));
            if (Invoice == null)
            {
                return NotFound("Invoice not found.");
            }

            var orderDetails = await _mediator.Send(new GetOrderDetails.Request(Invoice.OrderId));
            if (orderDetails == null)
            {
                OrderLines = new List<OrderLineDto>();
            }
            else
            {
                OrderLines = orderDetails.OrderLines;

                // Fetch the delivery fee from the associated order
                var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == Invoice.OrderId);
                if (order != null)
                {
                    DeliveryFee = order.DeliveryFee.Value;
                }
            }

            return Page();
        }


        public async Task<IActionResult> OnPostPayWithStripe(Guid invoiceId)
        {
            if (Tip < 0)
            {
                ModelState.AddModelError("Tip", "Tip cannot be negative.");
                return Page();
            }

            var invoice = await _db.Invoices.FindAsync(invoiceId);

            if (invoice == null)
            {
                ModelState.AddModelError("", "Invoice not found.");
                return Page();
            }

            invoice.UpdateTip(Tip);
            await _db.SaveChangesAsync();

            return Page();
        }
    }
}
