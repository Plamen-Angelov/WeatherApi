using Newtonsoft.Json;
using Weather.Api.Interfaces;
using Weather.Api.Models;

namespace Weather.Api.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private const string weatherApiKey = "a6dda73bff726fbf097c8138dfc09aaf";

    public WeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromSeconds(360);
    }

    public async Task<GetWeatherResponse?> GetTemperature(string city)
    {
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={weatherApiKey}&units=metric";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return null!;
        }

        var data = await response.Content.ReadAsStringAsync();
        var weather = JsonConvert.DeserializeObject<dynamic>(data);

        if (weather == null)
        {
            return null!;
        }

        double temperatureCelsius = weather.main.temp;

        return new GetWeatherResponse
        {
            TemperatureCelsius = Math.Round(temperatureCelsius, 2),
            TemperatureFahrenheit = Math.Round(temperatureCelsius * 9 / 5 + 32, 2),
            TemperatureKelvin = Math.Round(temperatureCelsius + 273.15, 2),
        };
    }
}
