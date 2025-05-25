using BarCode.Domain.Models;
using BarCode.Domain.Services;

namespace BarCode.Infrastructure.Services;

public class CartService : ICartService {
    public decimal CalculateSubtotal(decimal unitaryPrice, int quantity) {
        return unitaryPrice * quantity;
    }

    public decimal CalculateTotal(ref Order order, decimal subtotal) {
        return order.OrderTotal += subtotal;
    }
}