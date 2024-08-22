using Microsoft.AspNetCore.Mvc;
using Weather.Api.Interfaces;
using Weather.Api.Models;

namespace Weather.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("getweather/{city}")]
    public async Task<IActionResult> GetTemperature(string city)
    {
        try
        {
            var result = await _weatherService.GetTemperature(city);

            if (result is null)
            {
                return NotFound($"City with name {city} was not fount");
            }

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Error fetching data from the weather service.");
        }

    }
}
