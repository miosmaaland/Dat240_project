using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmaHauJenHoaVij.Infrastructure.Data;
using SmaHauJenHoaVij.Core.Domain.Users;
using System.Threading.Tasks;
using System;

namespace SmaHauJenHoaVij.Pages.CustomerPages
{
    public class ApplyToCourierModel : PageModel
    {
        private readonly ShopContext _context;

        public ApplyToCourierModel(ShopContext context)
        {
            _context = context;
        }

        public bool IsApplying { get; set; }

        public async Task OnGetAsync()
        {
            // Assume user is authenticated, retrieve their record
            var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            var customer = await _context.Customers.FindAsync(userId);

            if (customer != null)
            {
                IsApplying = customer.IsApplyingForCourier;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            var customer = await _context.Customers.FindAsync(userId);

            if (customer != null)
            {
                customer.IsApplyingForCourier = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
