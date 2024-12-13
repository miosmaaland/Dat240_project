using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmaHauJenHoaVij.Infrastructure.Data;
using System.Threading.Tasks;
using System;

namespace SmaHauJenHoaVij.Pages.AdminPages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly ShopContext _context;

        public ChangePasswordModel(ShopContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/AdminPages/Login");
            }

            var adminId = Guid.Parse(userId);
            var admin = await _context.Admins.FindAsync(adminId);

            if (admin == null)
            {
                return RedirectToPage("/AdminPages/Login");
            }

            // Allow access to this page regardless of PasswordNeedsReset
            return Page();
        }



        public async Task<IActionResult> OnPostAsync()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/AdminPages/Login");
            }

            if (string.IsNullOrEmpty(NewPassword) || NewPassword != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match or are empty.";
                return Page();
            }

            // Update admin password
            var adminId = Guid.Parse(userId);
            var admin = await _context.Admins.FindAsync(adminId);

            if (admin == null)
            {
                return RedirectToPage("/Admin/Login");
            }

            // Update the password
            admin.PasswordHash = HashPassword(NewPassword);
            admin.PasswordNeedsReset = false;

            await _context.SaveChangesAsync();

            // Clear session and redirect to login
            HttpContext.Session.Clear();
            return RedirectToPage("/AdminPages/Login");
        }

        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
        }
    }
}
