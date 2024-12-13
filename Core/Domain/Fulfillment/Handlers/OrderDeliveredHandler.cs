using MediatR; // Ensure this is included
using SmaHauJenHoaVij.Core.Domain.Fulfillment.Events;
using SmaHauJenHoaVij.Infrastructure.Services;
using SmaHauJenHoaVij.Infrastructure.Data; // Assuming ShopContext is in use
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class OrderDeliveredHandler : INotificationHandler<OrderDelivered>
{
    private readonly IEmailService _emailService;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly ShopContext _dbContext;

    public OrderDeliveredHandler(IEmailService emailService, IEmailTemplateService emailTemplateService, ShopContext dbContext)
    {
        _emailService = emailService;
        _emailTemplateService = emailTemplateService;
        _dbContext = dbContext;
    }

    public async Task Handle(OrderDelivered notification, CancellationToken cancellationToken)
    {
        // Retrieve the courier's email from the database
        var courierEmail = await _dbContext.Couriers
            .Where(c => c.Id == notification.CourierId)
            .Select(c => c.Email)
            .FirstOrDefaultAsync(cancellationToken);

        // If email is not found, throw an exception or log the error
        if (string.IsNullOrEmpty(courierEmail))
        {
            throw new InvalidOperationException("Courier email not found.");
        }

        // Define placeholders for the template
        var placeholders = new Dictionary<string, string>
        {
            { "OrderId", notification.OrderId.ToString() },
            { "CourierId", notification.CourierId.ToString() }
        };

        // Get the email template
        string emailBody = await _emailTemplateService.GetEmailTemplateAsync("OrderDelivered", placeholders);

        // Send the email
        string subject = "Your Order Has Been Delivered!";
        await _emailService.SendEmailAsync(courierEmail, subject, emailBody);
    }
}
