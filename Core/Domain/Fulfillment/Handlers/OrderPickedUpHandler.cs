using SmaHauJenHoaVij.Core.Domain.Users.Couriers; // Assuming you have a Courier entity
using SmaHauJenHoaVij.Infrastructure.Services;
using SmaHauJenHoaVij.Infrastructure.Data; // Assuming you have access to ShopContext
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Core.Domain.Fulfillment.Events;
using MediatR;

public class OrderPickedUpHandler : INotificationHandler<OrderPickedUp>
{
    private readonly IEmailService _emailService;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly ShopContext _dbContext;

    // Inject the ShopContext to access the database
    public OrderPickedUpHandler(IEmailService emailService, IEmailTemplateService emailTemplateService, ShopContext dbContext)
    {
        _emailService = emailService;
        _emailTemplateService = emailTemplateService;
        _dbContext = dbContext;
    }

    public async Task Handle(OrderPickedUp notification, CancellationToken cancellationToken)
    {
        // Retrieve the courier's email from the database
        var courier = await _dbContext.Couriers
            .Where(c => c.Id == notification.CourierId)
            .Select(c => c.Email)
            .FirstOrDefaultAsync(cancellationToken);

        // Check if we found the email
        if (string.IsNullOrEmpty(courier))
        {
            // Log the error or throw exception if the email is not found
            throw new InvalidOperationException("Courier email not found.");
        }

        // Define placeholders for the template
        var placeholders = new Dictionary<string, string>
        {
            { "OrderId", notification.OrderId.ToString() },
            { "CourierId", notification.CourierId.ToString() }
        };

        // Get the email template
        string emailBody = await _emailTemplateService.GetEmailTemplateAsync("OrderPickedUp", placeholders);

        // Send the email
        string subject = "Your Order Has Been Picked Up!";
        await _emailService.SendEmailAsync(courier, subject, emailBody);
    }
}
