using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;

public class CustomUserIdProvider : IUserIdProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomUserIdProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserId(HubConnectionContext connection)
    {
        // Retrieve UserId from session
        var httpContext = _httpContextAccessor.HttpContext;
        return httpContext?.Session.GetString("UserId");
    }
}
