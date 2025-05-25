using BarCode.Domain.Models;

namespace BarCode.Domain.Services;

public interface ICartService {
    decimal CalculateSubtotal(decimal unitaryPrice, int quantity);
    decimal CalculateTotal(ref Order order, decimal subtotal);
}