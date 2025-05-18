using BarCode.Domain.Models;

namespace BarCode.Domain.Services;

public interface IAutomaticRegistration {
    Product InstantiateProduct(string json);
    Product FillInProductInformation(Product product);
}