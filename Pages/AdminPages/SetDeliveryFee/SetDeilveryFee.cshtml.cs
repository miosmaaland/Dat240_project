using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmaHauJenHoaVij.Core.Domain.Ordering.Services;

namespace SmaHauJenHoaVij.Pages.AdminPages.SetDeliveryFee
{
    public class SetDeliveryFeeModel : PageModel
    {
        private readonly DeliveryFeeService _deliveryFeeService;

        public SetDeliveryFeeModel(DeliveryFeeService deliveryFeeService)
        {
            _deliveryFeeService = deliveryFeeService;
        }

        [BindProperty]
        public decimal Fee { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _deliveryFeeService.SetFee(Fee);
            TempData["Message"] = $"Delivery fee set to {Fee}.";
            return RedirectToPage("/Index");
        }
    }
}
