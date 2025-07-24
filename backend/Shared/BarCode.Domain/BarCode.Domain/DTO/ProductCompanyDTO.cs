using BarCode.Domain.Models;

namespace BarCode.Domain.DTO;

public record ProductCompanyDTO(int ProductCompanyId, int ProductId, int CompanyId, decimal Price, int Stock) {
    public ProductCompanyDTO FromModel(ProductCompany model) => new(
                                                                    model.ProductCompanyId,
                                                                    model.ProductId,
                                                                    model.CompanyId,
                                                                    model.Price,
                                                                    model.Stock);
}

public record ProductCompanyByBarcodeDTO(int ProductId, int CompanyId, decimal Price, int Stock) {
    public ProductCompanyByBarcodeDTO FromModel(ProductCompanyByBarcodeDTO model) => new(
         model.ProductId,
         model.CompanyId,
         model.Price,
         model.Stock);
}

public record ProductCompanyWithCompanyDTO(int ProductCompanyId, int ProductId, decimal Price, int Stock, CompanyCardToBuyProductDTO Company) {
    public ProductCompanyWithCompanyDTO FromModel(ProductCompanyWithCompanyDTO model) => new(
         model.ProductCompanyId,
         model.ProductId,
         model.Price,
         model.Stock,
         model.Company);
}