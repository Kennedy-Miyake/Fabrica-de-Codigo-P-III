namespace BarCode.Infrastructure.Tests.Helpers;

public class FakeHttpMessageHandler : HttpMessageHandler {
    private readonly HttpResponseMessage _response;
    
    public FakeHttpMessageHandler(HttpResponseMessage response) {
        _response = response;
    }
    
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, 
                                                           CancellationToken cancellationToken) 
        => Task.FromResult(_response);
}