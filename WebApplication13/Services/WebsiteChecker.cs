using System.Net;

namespace WebApplication13.Services
{
    public class WebsiteChecker : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    string url = "https://www.youtube.com/";
                    bool isAvailable = await CheckWebsiteAvailability(url);
                    LogResult(url, isAvailable);
                }
                catch (Exception ex)
                {
                    LogException(ex);
                }

                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }

        private async Task<bool> CheckWebsiteAvailability(string url)
        {
            try
            {
                var request = WebRequest.Create(url);
                request.Timeout = 10000;
                using (var response = await request.GetResponseAsync())
                {
                    return true;
                }
            }
            catch (WebException)
            {
                return false;
            }
        }

        private void LogResult(string url, bool isAvailable)
        {
            string logFilePath = "website_checker_log.txt";
            string logMessage = $"{DateTime.Now}: Website {url} is {(isAvailable ? "available" : "not available")}";
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }

        private void LogException(Exception ex)
        {
            string logFilePath = "website_checker_log.txt";
            string logMessage = $"{DateTime.Now}: Exception occurred - {ex.Message}";
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }
    }
}
