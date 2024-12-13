using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Core.Domain.Users.Services
{
    public interface IUserService
    {
        Task<bool> Login(string email, string password);
        Task<(bool Success, string Message)> ApproveCourier(Guid courierId);
        Task<(bool Success, string Message)> DenyCourier(Guid courierId);
        Task Logout();
        Task<(bool Success, string Message)> Register(string name, string email, string password, string phone);
        Task<(bool Success, string Message)> ApplyToBecomeCourier(Guid customerId);
        Task<User> FindOrCreateExternalUserAsync(string email, string name, string provider, string providerKey);

        Task<bool> ResetPasswordAsync(string token, string newPassword);
        Task<User?> FindByEmailAsync(string email);
        Task<User?> FindByPasswordResetToken(string token);
        
        // Add this method
        Task SaveAsync(User user);  // Ensure to save user after updates



        Task<(bool Success, string Message)> ChangeUserRole(Guid userId, string newRole);
    }
}
