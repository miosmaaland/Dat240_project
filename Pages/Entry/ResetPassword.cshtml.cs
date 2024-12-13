using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmaHauJenHoaVij.Infrastructure.Services;
using SmaHauJenHoaVij.Core.Domain.Users.Events;
using SmaHauJenHoaVij.Core.Domain.Users.Handlers;
using SmaHauJenHoaVij.Core.Domain.Users; 
using SmaHauJenHoaVij.Core.Domain.Users.Services;  

public class ResetPasswordModel : PageModel
{
    private readonly IMediator _mediator;
    private readonly IUserService _userService;  // Add service to handle user-specific logic (e.g., Customer or Courier)

    public ResetPasswordModel(IMediator mediator, IUserService userService)
    {
        _mediator = mediator;
        _userService = userService;  // Initialize the user service
    }

    [BindProperty]
    public string Email { get; set; } = null!;

    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        // Check if the user exists in the system (customer/courier)
        var user = await _userService.FindByEmailAsync(Email);

        if (user == null)
        {
            ErrorMessage = "The email provided does not exist in our records.";
            return Page();  // Early return if no user is found
        }

        // If the user is found, publish the password reset event
        await _mediator.Publish(new PasswordResetEvent(Email));

        SuccessMessage = "If this email exists, a password reset link has been sent.";
        return Page();
    }
}
