using System.Text.Json;
using BarCode.Domain.Models;
using BarCode.Domain.Services;

namespace BarCode.Infrastructure.Services;

public class AutomaticRegistration : IAutomaticRegistration {
    public Product InstantiateProduct(string json) {
    }
    public Product FillInProductInformation(Product product) {
        throw new NotImplementedException();
    }
}