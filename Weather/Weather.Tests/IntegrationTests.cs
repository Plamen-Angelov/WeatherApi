using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Weather.Api.Controllers;
using Weather.Api.Interfaces;
using Weather.Api.Models;
using Weather.Api.Services;

namespace Weather.Tests;

public class IntegrationTests
{
    private IWeatherService weatherService;
    private WeatherController controller;

    private const string city = "Sofia";

    [SetUp]
    public void Setup()
    {
        weatherService = new WeatherService(new HttpClient());
        controller = new WeatherController(weatherService);
    }

    [Test]
    public async Task GetTemperature_Should_Return_OK_When_No_Errors()
    {
        var result = await controller.GetTemperature(city);

        var expectedResponse = await weatherService.GetTemperature(city);
        var actualResponse = (GetWeatherResponse)((OkObjectResult)result!).Value!;

        Assert.IsNotNull(result);
        Assert.True(((OkObjectResult)result!).StatusCode == StatusCodes.Status200OK);
        Assert.True(actualResponse.TemperatureCelsius.Equals(expectedResponse!.TemperatureCelsius));
        Assert.True(actualResponse.TemperatureFahrenheit.Equals(expectedResponse.TemperatureFahrenheit));
        Assert.True(actualResponse.TemperatureKelvin.Equals(expectedResponse.TemperatureKelvin));
    }
}
