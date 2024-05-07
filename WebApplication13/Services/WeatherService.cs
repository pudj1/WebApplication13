using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using System.Text.Json.Nodes;
using WebApplication13.Response;

namespace WebApplication13.Services
{
    public class WeatherService : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;

        public WeatherService(IHttpClientFactory httpClientFactory, IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Виклик зовнішнього API із кешуванням результатів
                var weather = await GetWeatherAsync();
                Console.WriteLine($"Weather: {weather}");

                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken); // Перевіряти кожні 10 хвилин
            }
        }

        private async Task<string> GetWeatherAsync()
        {
            var cacheKey = "weather_data";
            if (_cache.TryGetValue(cacheKey, out string weather))
            {
                Console.WriteLine("Weather data found in cache.");
                return weather;
            }

            Console.WriteLine("Weather data not found in cache. Calling external API.");

            // Конфігурування HttpClient
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");

            // Параметри запиту
            var queryParams = $"weather?lat=44.34&lon=10.99&appid=d2e95d81119532bb0fd4657f98570407";

            // Виклик зовнішнього API
            var response = await client.GetAsync(queryParams);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var weatherData = JsonSerializer.Deserialize<WeatherResponse>(content, options);

                // Збереження результатів у кеші на 5 хвилин
                _cache.Set(cacheKey, weatherData.ToString(), TimeSpan.FromMinutes(5));

                return weatherData.ToString();
            }
            else
            {
                Console.WriteLine($"Error getting weather data: {response.StatusCode}");
                return null;
            }
        }
    }

}
