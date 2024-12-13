using Microsoft.AspNetCore.Http;

namespace SmaHauJenHoaVij.Infrastructure.Services
{
    public interface ISessionService
    {
        string GetLoggedInUserId();
        string GetLoggedInUserType();
        string GetLoggedInUserName();
        string GetCartId();
        void SetLoggedInUser(string userId, string userType);
        void ClearSession();
        void InitializeCart();
        void SetCartId(string cartId);
    }

}