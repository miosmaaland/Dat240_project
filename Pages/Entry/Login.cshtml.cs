using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmaHauJenHoaVij.Core.Domain.Cart.Pipelines;
using SmaHauJenHoaVij.Core.Domain.Users.Services;
using SmaHauJenHoaVij.Infrastructure.Services;
using MediatR;

public class LoginModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ISessionService _sessionService;
    private readonly IMediator _mediator;

    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public LoginModel(IUserService userService, ISessionService sessionService, IMediator mediator)
    {
        _userService = userService;
        _sessionService = sessionService;
        _mediator = mediator;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var loginSuccess = await _userService.Login(Email, Password);
        if (!loginSuccess)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return Page();
        }

        var userId = _sessionService.GetLoggedInUserId();
        var userType = _sessionService.GetLoggedInUserType();

        // Prevent admin users from logging in via this form
        if (userType == "Admin")
        {
            await _userService.Logout(); // Log out the admin user
            ModelState.AddModelError(string.Empty, "Admins must log in using the admin login page.");
            return Page();
        }

        // Fetch or initialize the user's cart
        var cart = await _mediator.Send(new GetCartByCustomerId.Request(Guid.Parse(userId)));
        if (cart != null)
        {
            _sessionService.SetCartId(cart.Id.ToString());
        }
        else
        {
            // Create a new cart if none exists
            var newCartId = Guid.NewGuid();
            _sessionService.SetCartId(newCartId.ToString());

            await _mediator.Send(new CreateCart.Request(newCartId, Guid.Parse(userId)));
        }

        // Redirect based on user type
        if (userType == "Courier")
        {
            return RedirectToPage("/Index");
        }
        else
        {
            return RedirectToPage("/Index");
        }
    }
}
