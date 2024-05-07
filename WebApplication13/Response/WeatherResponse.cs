namespace WebApplication13.Response
{
    public class WeatherResponse
    {
        public Coord Coord { get; set; }
        public Weather[] Weather { get; set; }
        public Main Main { get; set; }
        public Wind Wind { get; set; }
        public Rain Rain { get; set; }
        public Clouds Clouds { get; set; }
        public int Visibility { get; set; }
        public Sys Sys { get; set; }
        public int Dt { get; set; }
        public int Timezone { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cod { get; set; }
        public override string ToString()
        {
            return $"Coordinates: {Coord.Lon}, {Coord.Lat}\n" +
                   $"Weather: {Weather[0].Main} - {Weather[0].Description}\n" +
                   $"Temperature: {Main.Temp}°C\n" +
                   $"Feels like: {Main.Feels_like}°C\n" +
                   $"Pressure: {Main.Pressure} hPa\n" +
                   $"Humidity: {Main.Humidity}%\n" +
                   $"Wind: {Wind.Speed} m/s, {Wind.Deg}°\n" +
                   $"Rain: {Rain?.Rain1h} mm\n" +
                   $"Cloudiness: {Clouds.All}%\n" +
                   $"Visibility: {Visibility} meters\n" +
                   $"City ID: {Id}\n" +
                   $"City Name: {Name}\n" +
                   $"Response Code: {Cod}";
        }

    }

    public class Coord
    {
        public float Lon { get; set; }
        public float Lat { get; set; }
    }

    public class Weather
    {
        public int Id { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    public class Main
    {
        public float Temp { get; set; }
        public float Feels_like { get; set; }
        public float Temp_min { get; set; }
        public float Temp_max { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public int Sea_level { get; set; }
        public int Grnd_level { get; set; }
    }

    public class Wind
    {
        public float Speed { get; set; }
        public int Deg { get; set; }
        public float Gust { get; set; }
    }

    public class Rain
    {
        public float Rain1h { get; set; }
    }

    public class Clouds
    {
        public int All { get; set; }
    }

    public class Sys
    {
        public int Type { get; set; }
        public int Id { get; set; }
        public string Country { get; set; }
        public int Sunrise { get; set; }
        public int Sunset { get; set; }
    }
}
