using System.Net;
using BarCode.Infrastructure.Services;
using BarCode.Infrastructure.Tests.Helpers;
using Microsoft.Extensions.Configuration;

namespace BarCode.Infrastructure.Tests;

public class BlueSoftCosmosClientTests {
    HttpClient CreateHttpClient(HttpResponseMessage fakeResponse) {
        var handler = new FakeHttpMessageHandler(fakeResponse);
        var client = new HttpClient(handler) {
            BaseAddress = new Uri("https://api.cosmos.bluesoft.com.br/")
        };
        client.DefaultRequestHeaders
              .UserAgent
              .ParseAdd("BarCodeAPIUnitTests (+https://github.com/Kennedy-Miyake/Fabrica-de-Codigo-P-III.git)");
        return client;
    }

    IConfiguration CreateConfiguration(string token) {
        var inMemorySettings = new Dictionary<string, string> {
            { "COSMOS_TOKEN", token }
        };

        return new ConfigurationBuilder()
               .AddInMemoryCollection(inMemorySettings!)
               .Build();
    }
}