using MediatR;

public class PasswordResetEvent : INotification
{
    public string Email { get; }

    public PasswordResetEvent(string email)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
    }
}
