using Microsoft.AspNetCore.SignalR;

namespace WebApplication13.Services
{
    public class NotificationHub : Hub
    {
        public async Task ReceiveMessage(string message)
        {
            Console.WriteLine(message);
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
