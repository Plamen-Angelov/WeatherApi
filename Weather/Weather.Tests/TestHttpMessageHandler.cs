namespace Weather.Tests;

public class TestHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpResponseMessage _responseMessage;

    public TestHttpMessageHandler(HttpResponseMessage responseMessage)
    {
        _responseMessage = responseMessage;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_responseMessage);
    }
}
