using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmaHauJenHoaVij.Infrastructure.Services;

public class LogoutModel : PageModel
{
    private readonly ISessionService _sessionService;

    public LogoutModel(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public IActionResult OnGet()
    {
        _sessionService.ClearSession(); // Clear all session data
        return RedirectToPage("/Index"); // Redirect to the login page
    }
}
