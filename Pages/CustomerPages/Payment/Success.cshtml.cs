using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using SmaHauJenHoaVij.Core.Domain.Invoicing.Pipelines;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmaHauJenHoaVij.Core.Domain.Invoicing.Events;

namespace SmaHauJenHoaVij.Pages.CustomerPages.Payment
{
    public class SuccessModel : PageModel
    {
        private readonly IMediator _mediator;

        public SuccessModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync(Guid invoiceId)
        {
            if (invoiceId == Guid.Empty)
            {
                return BadRequest("Invalid Invoice ID.");
            }

            // Publish the InvoicePaid event
            await _mediator.Publish(new InvoicePaid(invoiceId));

            // Return a success confirmation page
            return Page();
        }


    } 
}
