using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmaHauJenHoaVij.Core.Domain.Users.Services;

public class RegisterModel : PageModel
{
    private readonly IUserService _userService;

    [BindProperty]
    public string Name { get; set; }

    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Password { get; set; }

    [BindProperty]
    public string Phone { get; set; }

    public RegisterModel(IUserService userService)
    {
        _userService = userService;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var (success, message) = await _userService.Register(Name, Email, Password, Phone);
        if (!success)
        {
            ModelState.AddModelError(string.Empty, message);
            return Page();
        }

        return RedirectToPage("Login");
    }
}
