using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Core.Domain.Users.Handlers;
public class ExternalLoginHandler
{
    public async Task HandleAsync(HttpContext context, string provider, string returnUrl)
    {
        var redirectUrl = $"/ExternalLoginCallback?returnUrl={returnUrl}";
        var properties = new AuthenticationProperties
        {
            RedirectUri = redirectUrl
        };
        properties.Items["LoginProvider"] = provider;

        await context.ChallengeAsync(provider, properties);
    }
}
