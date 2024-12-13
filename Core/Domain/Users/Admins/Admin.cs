namespace SmaHauJenHoaVij.Core.Domain.Users.Admins
{
    public class Admin : User
    {
        public bool PasswordNeedsReset { get; set; } = true;

        // Add this property to your Admin class
        public string PasswordResetToken { get; set; } = string.Empty;
    }
}
