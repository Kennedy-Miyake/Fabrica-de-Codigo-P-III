// ReSharper disable All
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PopulaDbWorker.Services;

public class BlueSoftCosmosClient : IBlueSoftCosmosClient {
    private readonly HttpClient _http;
    private readonly string _token;

    public BlueSoftCosmosClient(HttpClient http, IConfiguration configuration) {
        _http = http;
        _token = configuration["COSMOS_TOKEN"] ?? throw new InvalidOperationException("COSMOS_TOKEN not set");
    }
    
    public async Task<string?> GetProductJsonAsync(string barcode, CancellationToken cancellationToken = default) {
        var request = new HttpRequestMessage(HttpMethod.Get, $"gtins/{barcode}.json");
        request.Headers.Add("X-Cosmos-Token", _token);
            
        using var response = await _http.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode) return null;
            
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}