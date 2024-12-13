using MediatR;
using SmaHauJenHoaVij.Core.Domain.Users.Events;
using SmaHauJenHoaVij.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Core.Domain.Users.Handlers;

public class UserRegisteredHandler : INotificationHandler<UserRegistered>
{
    private readonly IEmailService _emailService;
    private readonly IEmailTemplateService _emailTemplateService;

    public UserRegisteredHandler(IEmailService emailService, IEmailTemplateService emailTemplateService)
    {
        _emailService = emailService;
        _emailTemplateService = emailTemplateService;
    }

    public async Task Handle(UserRegistered notification, CancellationToken cancellationToken)
    {
        // Define placeholders for the template
        var placeholders = new Dictionary<string, string>
        {
            { "Name", notification.Name }
        };

        // Get the email template
        string emailBody = await _emailTemplateService.GetEmailTemplateAsync("UserRegistered", placeholders);

        // Send the email
        string subject = "Welcome to our Service!";
        await _emailService.SendEmailAsync(notification.Email, subject, emailBody);
    }
}
