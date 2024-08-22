namespace Weather.Api.Models;

public class GetWeatherResponse
{
    public double TemperatureCelsius { get; set; }

    public double TemperatureFahrenheit { get; set; }

    public double TemperatureKelvin { get; set; }
}
