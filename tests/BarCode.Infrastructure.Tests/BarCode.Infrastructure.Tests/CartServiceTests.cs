using BarCode.Infrastructure.Services;

namespace BarCode.Infrastructure.Tests;

public class CartServiceTests {
    [Fact]
    public void CalculateSubtotal_ReturnsCorrectSubtotal() {
        // Arrange
        var cartService = new CartService();
        decimal unitaryPrice = 10.00m;
        int quantity = 5;
        decimal expectedSubtotal = 50.00m;
        
        // Act
        decimal result = cartService.CalculateSubtotal(unitaryPrice, quantity);
        
        // Assert
        Assert.Equal(expectedSubtotal, result);
    }
}