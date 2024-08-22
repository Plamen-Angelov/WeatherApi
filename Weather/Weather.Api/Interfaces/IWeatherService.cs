using Weather.Api.Models;

namespace Weather.Api.Interfaces;

public interface IWeatherService
{
    Task<GetWeatherResponse?> GetTemperature(string city);
}
