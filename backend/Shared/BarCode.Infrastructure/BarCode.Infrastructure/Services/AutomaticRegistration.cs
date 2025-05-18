using System.Text.Json;
using BarCode.Domain.Models;
using BarCode.Domain.Services;

namespace BarCode.Infrastructure.Services;

public class AutomaticRegistration : IAutomaticRegistration {
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
    public Product FillInProductInformation(Product product) {
        throw new NotImplementedException();
    }
}