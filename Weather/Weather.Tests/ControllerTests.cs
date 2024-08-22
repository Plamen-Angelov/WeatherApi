using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Weather.Api.Controllers;
using Weather.Api.Interfaces;
using Weather.Api.Models;

namespace Weather.Tests;

public class ControllerTests
{
    private Mock<IWeatherService> mockWeatherService;
    private WeatherController controller;
    private GetWeatherResponse response;

    private const string city = "Sofia";

    [SetUp]
    public void Setup()
    {
        mockWeatherService = new Mock<IWeatherService>();
        controller = new WeatherController(mockWeatherService.Object);

        response = new GetWeatherResponse
        {
            TemperatureCelsius = 26.26,
            TemperatureFahrenheit = 79.27,
            TemperatureKelvin = 299.41
        };
    }

    [Test]
    public async Task GetTemperature_Should_Return_OK_When_No_Errors()
    {
        mockWeatherService.Setup(x => x.GetTemperature(city))
            .Returns(Task.FromResult(response)!);

        var result = await controller.GetTemperature(city);

        Assert.IsNotNull(result);
        Assert.True(((OkObjectResult)result!).StatusCode == StatusCodes.Status200OK);
        Assert.True(((OkObjectResult)result!).Value!.Equals(response));
    }

    [Test]
    public async Task GetTemperature_Should_Return_NotFound_When_City_Was_Not_Found()
    {
        mockWeatherService.Setup(x => x.GetTemperature(city))
            .Returns(Task.FromResult(default(GetWeatherResponse)));

        var result = await controller.GetTemperature(city);

        Assert.IsNotNull(result);
        Assert.True(((NotFoundObjectResult)result!).StatusCode == StatusCodes.Status404NotFound);
    }

    [Test]
    public async Task GetTemperature_Should_Return_StatuCode_500_When_Exeption_Is_Thrown()
    {
        mockWeatherService.Setup(x => x.GetTemperature(city))
            .ThrowsAsync(new Exception());

        var result = await controller.GetTemperature(city);

        Assert.IsNotNull(result);
        Assert.True(((ObjectResult)result!).StatusCode == StatusCodes.Status500InternalServerError);
    }
}