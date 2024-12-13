using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmaHauJenHoaVij.Core.Domain.Users.Pipelines;
using System;
using System.Threading.Tasks;
using SmaHauJenHoaVij.Models;

namespace SmaHauJenHoaVij.Pages.CustomerPages
{
    public class CustomerProfileModel : PageModel
    {
        private readonly UpdateCustomerProfilePipeline _pipeline;

        public CustomerProfileModel(UpdateCustomerProfilePipeline pipeline)
        {
            _pipeline = pipeline;
        }

        [BindProperty]
        public CustomerInputModel Customer { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Entry/Login");
            }

            var customerId = Guid.Parse(userId);

            // Fetch customer details from the pipeline
            var customer = await _pipeline.GetCustomerByIdAsync(customerId);
            if (customer == null)
            {
                return NotFound();
            }

            // Initialize the CustomerInputModel
            Customer = new CustomerInputModel
            {
                Name = customer.Name,
                Phone = customer.Phone
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Entry/Login");
            }

            var customerId = Guid.Parse(userId);
            var (success, message) = await _pipeline.ExecuteAsync(customerId, Customer.Name, Customer.Phone);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                return Page();
            }

            TempData["Message"] = message;
            return RedirectToPage();
        }


    }
}
