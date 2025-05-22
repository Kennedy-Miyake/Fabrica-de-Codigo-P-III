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