using System.Text.Json;
using Microsoft.Data.Sqlite;
using BarCode.Domain.Models;
using BarCode.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BarCode.Infrastructure.Tests;

public class AutomaticRegistrationTests {
    const string sampleJson = 
        "{\"description\":\"AÇÚCAR REFINADO ESPECIAL UNIÃO PACOTE 1KG\",\"gtin\":7891910000197,\"thumbnail\":\"https://cdn-cosmos.bluesoft.com.br/products/7891910000197\",\"width\":100.0,\"height\":100.0,\"length\":100.0,\"net_weight\":1000,\"gross_weight\":1050,\"created_at\":\"2014-04-24T11:07:34.000-03:00\",\"updated_at\":\"2025-05-18T18:55:27.000-03:00\",\"release_date\":null,\"price\":null,\"avg_price\":null,\"max_price\":0.0,\"min_price\":0.0,\"gtins\":[{\"gtin\":7891910000197,\"commercial_unit\":{\"type_packaging\":\"Unidade\",\"quantity_packaging\":1,\"ballast\":null,\"layer\":null}},{\"gtin\":7891910000203,\"commercial_unit\":{\"type_packaging\":\"Caixa\",\"quantity_packaging\":10,\"ballast\":11,\"layer\":10}}],\"origin\":\"COSMOS\",\"barcode_image\":\"https://api.cosmos.bluesoft.com.br/products/barcode/D215D0FAC1ACAEF6B65EE7ED9820DD38.png\",\"brand\":{\"name\":\"UNIÃO\",\"picture\":\"https://cdn-cosmos.bluesoft.com.br/brands/brand_uniao.png\"},\"gpc\":{\"code\":\"10000043\",\"description\":\"Açúcar / Substitutos do Açúcar (Não perecível)\"},\"ncm\":{\"code\":\"17019900\",\"description\":\"Outros\",\"full_description\":\"Açúcares e produtos de confeitaria - Açúcares de cana ou de beterraba e sacarose quimicamente pura, no estado sólido. - Outros: - Outros\",\"ex\":null},\"cest\":{\"id\":2154,\"code\":\"1710300\",\"description\":\"Outros tipos de açúcar, em embalagens de conteúdo inferior ou igual a 2 kg, exceto as embalagens contendo envelopes individualizados (sachês) de conteúdo inferior ou igual a 10 g\",\"parent_id\":1671},\"category\":{\"id\":217,\"description\":\"Açucar Refinado\",\"parent_id\":2}}";

    private AppDbContext CreateASAmpleDbContextWithSqlite() {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;
        
        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public void InstantiateProduct_ValidJson_ReturnValidProductAttributes() {
        // Arrange
        var document = JsonDocument.Parse(sampleJson);
        var root = document.RootElement;
        
        // Act
        var product = new Product {
            Name = root.GetProperty("description").ToString(),
            Description = root.GetProperty("ncm")
                              .GetProperty("full_description")
                              .ToString(),
            ImageUrl = root.GetProperty("thumbnail").ToString(),
            BarCode = root.GetProperty("gtin").GetRawText()
        };
        
        // Assert
        Assert.Equal("AÇÚCAR REFINADO ESPECIAL UNIÃO PACOTE 1KG", product.Name);
        Assert.Equal("Açúcares e produtos de confeitaria - Açúcares de cana ou de beterraba e sacarose quimicamente pura, no estado sólido. - Outros: - Outros", product.Description);
        Assert.Equal("https://cdn-cosmos.bluesoft.com.br/products/7891910000197", product.ImageUrl);
        Assert.Equal("7891910000197", product.BarCode);
    }

    [Fact]
    public async Task RegisterProductAsync_ValidProduct_SavesProductToDatabase() {
        // Arrange
        var document = JsonDocument.Parse(sampleJson);
        var root = document.RootElement;
        
        var context = CreateASAmpleDbContextWithSqlite();
        
        var product = new Product {
            Name = root.GetProperty("description").ToString(),
            Description = root.GetProperty("ncm")
                              .GetProperty("full_description")
                              .ToString(),
            ImageUrl = root.GetProperty("thumbnail").ToString(),
            BarCode = root.GetProperty("gtin").GetRawText()
        };
        
        // Act
        context.Add(product);
        await context.SaveChangesAsync();
        
        // Assert
        var savedProduct = await context.Products.FirstOrDefaultAsync(p => p.BarCode == product.BarCode);
        Assert.NotNull(savedProduct);
        Assert.Equal(product.Name, savedProduct.Name);
        Assert.Equal(product.Description, savedProduct.Description);
        Assert.Equal(product.ImageUrl, savedProduct.ImageUrl);
        Assert.Equal(product.BarCode, savedProduct.BarCode);
    }
}