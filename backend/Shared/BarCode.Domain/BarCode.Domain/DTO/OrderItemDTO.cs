using BarCode.Domain.Models;

namespace BarCode.Domain.DTO;

public record OrderItemDTO(
    int OrderItemId,
    int OrderId,
    int ProductCompanyId,
    int Quantity,
    decimal UnitaryPrice,
    decimal SubTotal) {
    public OrderItemDTO FromModel(OrderItemDTO model) => new(
                                                          model.OrderItemId,
                                                          model.OrderId,
                                                          model.ProductCompanyId,
                                                          model.Quantity,
                                                          model.UnitaryPrice,
                                                          model.SubTotal);
}

public record OrderItemCreateDTO(
    int OrderId,
    int ProductCompanyId,
    int Quantity) {
    public OrderItemCreateDTO FromModel(OrderItemCreateDTO model) => new(
                                                                         model.OrderId,
                                                                         model.ProductCompanyId,
                                                                         model.Quantity);
}