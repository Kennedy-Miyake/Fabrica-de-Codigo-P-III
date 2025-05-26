namespace BarCode.Domain.DTO;

public record OrderDTO(
    int OrderId,
    string DeliveryAddress,
    DateTime OrderDate,
    decimal OrderTotal,
    int ClientId) {

    public OrderDTO FromModel(OrderDTO model) => new(
        model.OrderId,
        model.DeliveryAddress,
        model.OrderDate,
        model.OrderTotal,
        model.ClientId);
}