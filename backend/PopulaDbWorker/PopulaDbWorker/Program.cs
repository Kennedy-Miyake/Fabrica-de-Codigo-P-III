// ReSharper disable All
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using PopulaDbWorker.Services;

namespace PopulaDbWorker;

public class Program {

    enum ProductAttributes {
        NAME,
        DESCRIPTION,
        IMAGE_URL,
        BARCODE,
        LENGTH_ENUM
    }
    
    public static Task Main(string[] args) {
        //  Cria o HostBuilder simplificado
        var builder = Host.CreateApplicationBuilder(args);
        
        //  Adiciona váriaveis de ambiente na hierarquia IConfiguration
        builder.Configuration.AddEnvironmentVariables();
        
        /*  Configura e registra o client HTTP paara a API Cosmos:
            - Define o URL base
            - Define o User-Agent
            - Define o Token de autenticação
            - Especifica que a aplicação aceita respostas em JSON */
        builder.Services.AddHttpClient<IBlueSoftCosmosClient, BlueSoftCosmosClient>((sp, http) => {
            var config = sp.GetRequiredService<IConfiguration>();
            http.BaseAddress = new Uri("https://api.cosmos.bluesoft.com.br/");
            http.DefaultRequestHeaders.UserAgent.ParseAdd("PopulaDbWorker/1.0 (+https://github.com/Kennedy-Miyake/Fabrica-de-Codigo-P-III.git)");
            http.DefaultRequestHeaders.Add("X-Cosmos-Token", config["COSMOS_TOKEN"] ?? throw new InvalidOperationException("COSMOS_TOKEN not set"));
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

        //  Constrói o host
        using var host = builder.Build();
        
        /*  - Cria um scopo DI
        using var scope = host.Services.CreateScope();
        var cosmos = scope.ServiceProvider.GetRequiredService<IBlueSoftCosmosClient>();
        
        // Invoca o método assíncrono que faz GET /gtins/código-de-barras.json
        var json = await cosmos.GetProductJsonAsync("7891910000197");
        
         List<string?> productAttributes = new List<string?>();
         for(int i = 0; i < (int)ProductAttributes.LENGTH_ENUM; i++) productAttributes.Add((Convert.ToString((ProductAttributes)i)));
         foreach (var attribute in productAttributes) Console.WriteLine(attribute);
         return Task.CompletedTask;
    }
}