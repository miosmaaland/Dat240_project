using MediatR;
using SmaHauJenHoaVij.Core.Domain.Users.Events;
using SmaHauJenHoaVij.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Core.Domain.Users.Handlers;

public class CourierApplicationDeclinedHandler : INotificationHandler<CourierApplicationDeclined>
{
    private readonly IEmailService _emailService;
    private readonly IEmailTemplateService _emailTemplateService;

    public CourierApplicationDeclinedHandler(IEmailService emailService, IEmailTemplateService emailTemplateService)
    {
        _emailService = emailService;
        _emailTemplateService = emailTemplateService;
    }

    public async Task Handle(CourierApplicationDeclined notification, CancellationToken cancellationToken)
    {
        // Define placeholders for the template
        var placeholders = new Dictionary<string, string>
        {
            { "Name", notification.Email }
        };

        // Get the email template
        string emailBody = await _emailTemplateService.GetEmailTemplateAsync("CourierApplicationDeclined", placeholders);

        // Send the email
        string subject = "Courier Application Declined";
        await _emailService.SendEmailAsync(notification.Email, subject, emailBody);
    }
}
