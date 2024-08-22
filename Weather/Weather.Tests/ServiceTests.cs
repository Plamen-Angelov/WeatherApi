using Newtonsoft.Json;
using System.Net;
using System.Text;
using Weather.Api.Services;

namespace Weather.Tests;

public class ServiceTests
{
    private HttpClient _httpClient;
    private WeatherService _weatherService;

    [SetUp]
    public void SetUp()
    {
        _httpClient = new HttpClient(new TestHttpMessageHandler(new HttpResponseMessage()));
        _weatherService = new WeatherService(_httpClient);
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient?.Dispose();
    }

    [Test]
    public async Task GetTemperature_Should_Return_WeatherResponse_When_Api_Returns_Valid_Response()
    {
        var city = "Sofia";

        var jsonResponse = new
        {
            main = new
            {
                temp = 26.26
            }
        };

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(jsonResponse), Encoding.UTF8, "application/json")
        };

        _httpClient = new HttpClient(new TestHttpMessageHandler(httpResponseMessage));
        _weatherService = new WeatherService(_httpClient);

        var result = await _weatherService.GetTemperature(city);

        Assert.IsNotNull(result);
        Assert.That(result.TemperatureCelsius, Is.EqualTo(26.26));
        Assert.That(result.TemperatureFahrenheit, Is.EqualTo(79.27));
        Assert.That(result.TemperatureKelvin, Is.EqualTo(299.41));
    }

    [Test]
    public async Task GetTemperature_Should_Return_Null_When_Api_Returns_Unsuccessful_StatusCode()
    {
        var city = "CityName";
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);

        _httpClient = new HttpClient(new TestHttpMessageHandler(httpResponseMessage));
        _weatherService = new WeatherService(_httpClient);

        var result = await _weatherService.GetTemperature(city);

        Assert.IsNull(result);
    }

    [Test]
    public async Task GetTemperature_Should_Return_Null_When_Api_Returns_InvalidJson()
    {
        var city = "Sofia";

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("", Encoding.UTF8, "application/json")
        };

        _httpClient = new HttpClient(new TestHttpMessageHandler(httpResponseMessage));
        _weatherService = new WeatherService(_httpClient);

        var result = await _weatherService.GetTemperature(city);

        Assert.IsNull(result);
    }
}
