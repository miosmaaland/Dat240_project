using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using SmaHauJenHoaVij.Core.Domain.Users.Services; // Namespace for IUserService

public class PasswordResetModel : PageModel
{
    private readonly IUserService _userService;

    public PasswordResetModel(IUserService userService)
    {
        _userService = userService;
    }

    [BindProperty]
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    public string NewPassword { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public string Token { get; set; } = string.Empty;

    public async Task<IActionResult> OnGet()
    {
        if (string.IsNullOrWhiteSpace(Token))
        {
            return BadRequest("Invalid or missing token.");
        }

        // Validate the token
        var user = await _userService.FindByPasswordResetToken(Token);

        if (user == null || user.PasswordResetTokenExpiry < DateTime.UtcNow)
        {
            return BadRequest("Invalid or expired token.");
        }

        return Page();
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Get the user by token
        var user = await _userService.FindByPasswordResetToken(Token);  // Await the async call
        if (user == null || user.PasswordResetTokenExpiry < DateTime.UtcNow)
        {
            ModelState.AddModelError(string.Empty, "Invalid or expired token.");
            return Page();
        }

        // Reset the password and clear the token
        var resetResult = await _userService.ResetPasswordAsync(Token, NewPassword);

        if (!resetResult)
        {
            ModelState.AddModelError(string.Empty, "Error resetting password.");
            return Page();
        }

        TempData["SuccessMessage"] = "Your password has been reset successfully.";
        return RedirectToPage("Login");
    }
}
