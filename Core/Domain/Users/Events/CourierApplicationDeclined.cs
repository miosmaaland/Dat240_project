using MediatR;

namespace SmaHauJenHoaVij.Core.Domain.Users.Events
{
    public class CourierApplicationDeclined : INotification
    {
        public string Email { get; init; }

        public CourierApplicationDeclined(string email)
        {
            Email = email;
        }
    }
}
