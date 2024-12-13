using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;
using SmaHauJenHoaVij.Core.Domain.Users;
using SmaHauJenHoaVij.Core.Domain.Users.Pipelines;
using SmaHauJenHoaVij.Core.Domain.Users.Couriers;
using MediatR; // Add this using for MediatR
using SmaHauJenHoaVij.Core.Domain.Users.Events; // Add this for events
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace SmaHauJenHoaVij.Pages.AdminPages
{
    public class CourierApplicationsModel : PageModel
    {
        private readonly ShopContext _context;
        private readonly IMediator _mediator; // Add IMediator

        // Inject IMediator into the constructor
        public CourierApplicationsModel(ShopContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator; // Set the mediator
        }

        public List<CustomerViewModel> PendingApplications { get; set; } = new List<CustomerViewModel>();

        public async Task OnGetAsync()
        {
            PendingApplications = await _context.Customers
                .Where(c => c.IsApplyingForCourier)
                .Select(c => new CustomerViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email
                })
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostApproveAsync(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer != null)
            {
                // Create a courier from the customer
                var courier = new Courier
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    PasswordHash = customer.PasswordHash,
                    IsApproved = true
                };

                // Remove the customer and add the courier
                _context.Customers.Remove(customer);
                _context.Couriers.Add(courier);
                await _context.SaveChangesAsync();

                // Publish the CourierApplicationApproved event
                await _mediator.Publish(new CourierApplicationApproved(customer.Email));
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeclineAsync(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer != null)
            {
                customer.IsApplyingForCourier = false;
                await _context.SaveChangesAsync();

                // Publish the CourierApplicationDeclined event
                await _mediator.Publish(new CourierApplicationDeclined(customer.Email));
            }

            return RedirectToPage();
        }

        public class CustomerViewModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
        }
    }
}
