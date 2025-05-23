using BarCode.Domain.Services;

namespace BarCode.Infrastructure.Services;

public class CartService : ICartService {
    public decimal CalculateSubtotal(decimal unitaryPrice, int quantity) {
        return unitaryPrice * quantity;
    }
}