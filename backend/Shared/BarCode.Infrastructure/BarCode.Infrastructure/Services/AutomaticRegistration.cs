using System.Text.Json;
using BarCode.Domain.Models;
using BarCode.Domain.Services;
using BarCode.Infrastructure.Context;

namespace BarCode.Infrastructure.Services;

public class AutomaticRegistration : IAutomaticRegistration {
    private readonly IBlueSoftCosmosClient _cosmosClient;
    private readonly AppDbContext _dbContext;
    
    public AutomaticRegistration(IBlueSoftCosmosClient cosmosClient, AppDbContext dbContext) {
        _cosmosClient = cosmosClient;
        _dbContext = dbContext;
    }
    public Product InstantiateProduct(string json) {
        var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        var product = new Product {
            Name = root.GetProperty("description").ToString(),
            Description = root.GetProperty("ncm")
                              .GetProperty("full_description")
                              .ToString(),
            ImageUrl = root.GetProperty("thumbnail").ToString(),
            BarCode = root.GetProperty("gtin").ToString()
        };

        return product;
    }
    public async Task<Product> FillInProductInformationAsync(string barcode, CancellationToken cancellationToken = default) {
        var json = await _cosmosClient.GetProductJsonAsync(barcode, cancellationToken);
        if (json is null) throw new ArgumentException("Produto não encontrado.");
        
        var product = InstantiateProduct(json.ToString());
        
        return product;
    }

    public async Task RegiterProductAsync(Product product) {
        _dbContext.Add(product);
        await _dbContext.SaveChangesAsync();
    }
}