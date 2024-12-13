using Microsoft.AspNetCore.Http;
using SmaHauJenHoaVij.Infrastructure.Services;

namespace SmaHauJenHoaVij.Infrastructure.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetLoggedInUserId()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            return session?.GetString("UserId");
        }

        public string GetLoggedInUserType()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            return session?.GetString("UserType");
        }

        public string GetLoggedInUserName()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            return session?.GetString("UserName");
        }

        public string GetCartId()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            return session?.GetString("CartId");
        }

        public void SetLoggedInUser(string userId, string userType)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.SetString("UserId", userId);
            session?.SetString("UserType", userType);
        }

        public void InitializeCart()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (string.IsNullOrEmpty(session?.GetString("CartId")))
            {
                session?.SetString("CartId", Guid.NewGuid().ToString());
            }
        }

        public void ClearSession()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.Clear();
        }

        public void SetCartId(string cartId)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.SetString("CartId", cartId);
        }


    }
}
