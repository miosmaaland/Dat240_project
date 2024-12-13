public interface IEmailTemplateService
{
    Task<string> GetEmailTemplateAsync(string templateName, Dictionary<string, string> placeholders);
}

public class EmailTemplateService : IEmailTemplateService
{
    // For simplicity, we are assuming that the templates are hardcoded.
    private readonly Dictionary<string, string> _templates = new()
    {
        { "UserRegistered", "Hello,\n\nThank you for registering with us.\nYou are looking mighty fine today!" },
        { "CourierApplicationApproved", "Hello,\n\nYour application has been approved! You can now start delivering orders.\nWelcome to the team!" },
        { "CourierApplicationDeclined", "Hello,\n\nWe regret to inform you that your application has been declined.\nThank you for your interest in joining our team. We wish you the best in your future endeavors." },
        { "OrderPickedUp", "Hello,\n\nYour order has been picked up and is on its way." },
        { "OrderDelivered", "Hello,\n\nYour order has been delivered successfully." }
    };

    public Task<string> GetEmailTemplateAsync(string templateName, Dictionary<string, string> placeholders)
    {
        if (!_templates.ContainsKey(templateName))
            throw new ArgumentException("Template not found", nameof(templateName));

        string template = _templates[templateName];

        // Replace placeholders
        foreach (var placeholder in placeholders)
        {
            template = template.Replace("{" + placeholder.Key + "}", placeholder.Value);
        }

        return Task.FromResult(template);
    }
}
