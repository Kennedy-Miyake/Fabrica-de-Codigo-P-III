namespace BarCode.Domain.Services;

public interface ICartService {
    decimal CalculateSubtotal(decimal unitaryPrice, int quantity);
}