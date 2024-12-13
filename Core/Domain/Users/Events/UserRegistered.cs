using MediatR;

namespace SmaHauJenHoaVij.Core.Domain.Users.Events
{
    public record UserRegistered(string Name, string Email) : INotification;
}
