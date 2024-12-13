using MediatR;

namespace SmaHauJenHoaVij.Core.Domain.Users.Events
{
    public class CourierApplicationApproved : INotification
    {
        public string Email { get; init; }

        public CourierApplicationApproved(string email)
        {
            Email = email;
        }
    }
}
