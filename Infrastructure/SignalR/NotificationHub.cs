using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SmaHauJenHoaVij.Infrastructure.SignalR
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userType = Context.GetHttpContext()?.User?.FindFirst("UserType")?.Value;
            if (userType == "Courier")
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Couriers");
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userType = Context.GetHttpContext()?.User?.FindFirst("UserType")?.Value;
            if (userType == "Courier")
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Couriers");
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
