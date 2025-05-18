using BarCode.Domain.Models;

namespace BarCode.Domain.Services;

public interface IAutomaticRegistration {
    Product InstantiateProduct(string json);
    Task<Product> FillInProductInformationAsync(string barcode, CancellationToken cancellationToken = default);
    Task RegiterProductAsync(Product product);
}