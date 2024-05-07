using System.Net;
using System.Net.Mail;

namespace WebApplication13.Services
{
    public class EmailNotificationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public EmailNotificationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string logFilePath = "website_checker_log.txt";
            using (var fileSystemWatcher = new FileSystemWatcher("./"))
            {
                fileSystemWatcher.Filter = Path.GetFileName(logFilePath);
                fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
                fileSystemWatcher.Changed += async (sender, e) => await HandleFileChangeAsync(logFilePath);
                fileSystemWatcher.EnableRaisingEvents = true;

                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
                }
            }
        }

        private async Task HandleFileChangeAsync(string filePath)
        {
            try
            {
                var smtpClient = new SmtpClient("your.smtp.server.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("your_email@example.com", "your_password"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("your_email@example.com"),
                    Subject = "File change notification",
                    Body = $"The file {filePath} has been modified at {DateTime.Now}.",
                };
                mailMessage.To.Add("recipient@example.com");
                
                await smtpClient.SendMailAsync(mailMessage);

                Console.WriteLine($"Email send");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email notification: {ex.Message}");
            }
        }
    }
}
