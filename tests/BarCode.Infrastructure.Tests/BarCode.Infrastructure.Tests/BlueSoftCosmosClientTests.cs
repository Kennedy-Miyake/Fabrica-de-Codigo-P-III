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
    
    [Fact]
    public async Task GetProductJsonAsync_ValidBarcode_ReturnsJson() {
        // Arrange
        var expectedJsonReturn =
            "{\"description\":\"AÇÚCAR REFINADO ESPECIAL UNIÃO PACOTE 1KG\",\"gtin\":7891910000197,\"thumbnail\":\"https://cdn-cosmos.bluesoft.com.br/products/7891910000197\",\"width\":100.0,\"height\":100.0,\"length\":100.0,\"net_weight\":1000,\"gross_weight\":1050,\"created_at\":\"2014-04-24T11:07:34.000-03:00\",\"updated_at\":\"2025-05-18T18:55:27.000-03:00\",\"release_date\":null,\"price\":null,\"avg_price\":null,\"max_price\":0.0,\"min_price\":0.0,\"gtins\":[{\"gtin\":7891910000197,\"commercial_unit\":{\"type_packaging\":\"Unidade\",\"quantity_packaging\":1,\"ballast\":null,\"layer\":null}},{\"gtin\":7891910000203,\"commercial_unit\":{\"type_packaging\":\"Caixa\",\"quantity_packaging\":10,\"ballast\":11,\"layer\":10}}],\"origin\":\"COSMOS\",\"barcode_image\":\"https://api.cosmos.bluesoft.com.br/products/barcode/D215D0FAC1ACAEF6B65EE7ED9820DD38.png\",\"brand\":{\"name\":\"UNIÃO\",\"picture\":\"https://cdn-cosmos.bluesoft.com.br/brands/brand_uniao.png\"},\"gpc\":{\"code\":\"10000043\",\"description\":\"Açúcar / Substitutos do Açúcar (Não perecível)\"},\"ncm\":{\"code\":\"17019900\",\"description\":\"Outros\",\"full_description\":\"Açúcares e produtos de confeitaria - Açúcares de cana ou de beterraba e sacarose quimicamente pura, no estado sólido. - Outros: - Outros\",\"ex\":null},\"cest\":{\"id\":2154,\"code\":\"1710300\",\"description\":\"Outros tipos de açúcar, em embalagens de conteúdo inferior ou igual a 2 kg, exceto as embalagens contendo envelopes individualizados (sachês) de conteúdo inferior ou igual a 10 g\",\"parent_id\":1671},\"category\":{\"id\":217,\"description\":\"Açucar Refinado\",\"parent_id\":2}}";
        var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK) {
            Content = new StringContent(expectedJsonReturn)
        };
        var client = CreateHttpClient(fakeResponse);
        var configuration = CreateConfiguration("COSMOS_TOKEN");
        var sut = new BlueSoftCosmosClient(client, configuration);
        
        // Act
        var json = await sut.GetProductJsonAsync("7891910000197");
        
        // Assert
        Assert.Equal(expectedJsonReturn, json);
    }
}