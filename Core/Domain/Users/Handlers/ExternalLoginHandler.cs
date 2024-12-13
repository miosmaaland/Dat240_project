using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SmaHauJenHoaVij.Core.Domain.Users.Services;
using SmaHauJenHoaVij.Infrastructure.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SmaHauJenHoaVij.Core.Domain.Users.Handlers;

namespace SmaHauJenHoaVij.Core.Domain.Users.Handlers;
public class ExternalLoginCallbackHandler
{
    private readonly IUserService _userService;
    private readonly ISessionService _sessionService;
    private readonly ILogger<ExternalLoginCallbackHandler> _logger;

    public ExternalLoginCallbackHandler(IUserService userService, ISessionService sessionService, ILogger<ExternalLoginCallbackHandler> logger)
    {
        _userService = userService;
        _sessionService = sessionService;
        _logger = logger;
    }

    public async Task HandleAsync(HttpContext context, string returnUrl)
    {
        try
        {
            var authResult = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (authResult?.Principal == null)
            {
                // Authentication failed
                context.Response.Redirect($"/ExternalLogin?provider=Facebook&returnUrl={returnUrl}");
                return;
            }

            var claims = authResult.Principal.Identities.FirstOrDefault()?.Claims;
            if (claims == null)
            {
                context.Response.Redirect("/Login");
                return;
            }

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var provider = authResult.Properties.Items["LoginProvider"];
            var providerKey = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _userService.FindOrCreateExternalUserAsync(email, name, provider, providerKey);
            if (user == null)
            {
                context.Response.Redirect("/Login");
                return;
            }

            _sessionService.SetLoggedInUser(user.Id.ToString(), user.GetType().Name);

            var session = context.Session;
            if (!session.Keys.Contains("CartId"))
            {
                session.SetString("CartId", Guid.NewGuid().ToString());
            }

            context.Response.Redirect(returnUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during external login callback.");
            context.Response.Redirect("/Login");
        }
    }
}
