using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmaHauJenHoaVij.Core.Domain.Users.Services;
using SmaHauJenHoaVij.Infrastructure.Services;

public class PasswordResetHandler : INotificationHandler<PasswordResetEvent>
{
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;

    public PasswordResetHandler(IUserService userService, IEmailService emailService)
    {
        _userService = userService;
        _emailService = emailService;
    }

    public async Task Handle(PasswordResetEvent notification, CancellationToken cancellationToken)
    {
        var user = await _userService.FindByEmailAsync(notification.Email);

        if (user == null)
        {
            // Optionally, log or track failed attempts, but do not disclose that the user doesn't exist
            return;
        }

        // Generate a new GUID token for password reset
        var resetToken = Guid.NewGuid().ToString();

        // Set the reset token and expiry date (e.g., 1 hour)
        user.PasswordResetToken = resetToken;
        user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);

        // Save the user with the new token and expiry
        await _userService.SaveAsync(user);

        // Explicitly generate the password reset URL with HTTP instead of HTTPS
        var passwordResetUrl = "http://localhost:5268/Entry/PasswordReset?token=" + resetToken;

        // Send email with the reset link
        await _emailService.SendEmailAsync(
            user.Email,
            "Password Reset Request",
            $"Click <a href='{passwordResetUrl}'>here</a> to reset your password.");
    }
}
