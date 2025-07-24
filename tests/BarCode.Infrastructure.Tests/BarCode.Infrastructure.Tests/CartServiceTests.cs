using BarCode.Domain.Models;
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

    [Fact]
    public void CalculateTotal_AddsSubtotals_ReturnCorrectTotal() {
        // Arrange
        var cartService = new CartService();
        var order = new Order { OrderTotal = 0 };
        decimal subtotal = 50.00m;
        decimal expectedTotal = 50.00m;
        
        // Act
        decimal result = cartService.CalculateTotal(ref order, subtotal);
        
        // Assert
        Assert.Equal(expectedTotal, result);
    }
}