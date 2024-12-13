using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SmaHauJenHoaVij.Core.Domain.Invoicing.Pipelines;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Pages.CustomerPages.Payment
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<GetCustomerInvoices.Response> Invoices { get; set; } = new();

        public async Task OnGetAsync()
        {
            var customerId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            Invoices = await _mediator.Send(new GetCustomerInvoices.Request(customerId));
        }
    }
}
