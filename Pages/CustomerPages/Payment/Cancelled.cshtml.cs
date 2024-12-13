using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using SmaHauJenHoaVij.Core.Domain.Invoicing.Pipelines;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmaHauJenHoaVij.Core.Domain.Invoicing.Events;

namespace SmaHauJenHoaVij.Pages.CustomerPages.Payment
{
    public class CancelledModel : PageModel
    {
        private readonly IMediator _mediator;

        public CancelledModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync(Guid invoiceId)
        {
            if (invoiceId == Guid.Empty)
            {
                return BadRequest("Invalid Invoice ID.");
            }

            // Publish the InvoiceCancelled event
            await _mediator.Publish(new InvoiceCancelled(invoiceId));

            // Return a cancellation confirmation page
            return Page();
        }
    }
}