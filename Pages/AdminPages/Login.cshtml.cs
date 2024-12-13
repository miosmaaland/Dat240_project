using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmaHauJenHoaVij.Infrastructure.Data;
using SmaHauJenHoaVij.Core.Domain.Users;
using System.Linq;
using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Pages.AdminPages
{
    public class AdminLoginModel : PageModel
    {
        private readonly ShopContext _context;

        public AdminLoginModel(ShopContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Email and Password are required.";
                return Page();
            }

            // Ensure the user is an admin
            var admin = _context.Admins.SingleOrDefault(a => a.Email == Email);

            if (admin == null || !VerifyPassword(Password, admin.PasswordHash))
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }

            // Check if password reset is required
            if (admin.PasswordNeedsReset)
            {
                // Store minimal session data for change password flow
                HttpContext.Session.SetString("UserId", admin.Id.ToString());
                HttpContext.Session.SetString("UserType", "Administrator");
                return RedirectToPage("/AdminPages/ChangePassword");
            }

            // Store admin session
            HttpContext.Session.SetString("UserId", admin.Id.ToString());
            HttpContext.Session.SetString("UserType", "Administrator");
            HttpContext.Session.SetString("UserName", admin.Name);

            return RedirectToPage("/AdminPages/AllUsers");
        }


        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var enteredHash = Convert.ToBase64String(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(enteredPassword)));
            return enteredHash == storedPasswordHash;
        }
    }
}
