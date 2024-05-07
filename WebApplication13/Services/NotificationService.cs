using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication13.Services
{
    public class NotificationService : BackgroundService
    {
        private readonly HubConnection _hubConnection;

        public NotificationService()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7208/notificationHub")
                .WithAutomaticReconnect()
                .Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _hubConnection.StartAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                var message = GetMessageToSend();

                await _hubConnection.SendAsync("SendMessage", message, stoppingToken);

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private string GetMessageToSend()
        {
            return "message";
        }
    }
}
