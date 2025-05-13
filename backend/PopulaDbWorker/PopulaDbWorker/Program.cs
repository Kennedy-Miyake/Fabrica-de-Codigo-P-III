// ReSharper disable All
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using PopulaDbWorker.Services;

namespace PopulaDbWorker;

public class Program {
    public static async Task Main(string[] args) {
        var builder = Host.CreateApplicationBuilder(args);
        
        builder.Configuration.AddEnvironmentVariables();
        
        builder.Services.AddHttpClient<IBlueSoftCosmosClient, BlueSoftCosmosClient>((sp, http) => {
            var config = sp.GetRequiredService<IConfiguration>();
            http.BaseAddress = new Uri("https://api.cosmos.bluesoft.com.br/");
            http.DefaultRequestHeaders.UserAgent.ParseAdd("PopulaDbWorker/1.0 (+https://github.com/Kennedy-Miyake/Fabrica-de-Codigo-P-III.git)");
            http.DefaultRequestHeaders.Add("X-Cosmos-Token", config["COSMOS_TOKEN"] ?? throw new InvalidOperationException("COSMOS_TOKEN not set"));
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

        using var host = builder.Build();
        
        using var scope = host.Services.CreateScope();
        var cosmos = scope.ServiceProvider.GetRequiredService<IBlueSoftCosmosClient>();
        
        var json = await cosmos.GetProductJsonAsync("7891910000197");
        Console.WriteLine(json ?? "Produto não encontrado ou HTTP erro.");
    }
}