using BarCode.Infrastructure.Services;

namespace BarCode.Infrastructure.Tests;

public class Ean13BarCodeValidatorTests {
    [Fact]
    public void IsValid_ValidBarCode_ReturnsTrue() {
        // Arrange
        string validBarCode = "7891910010905";
        var barCodeValidation = new Ean13BarCodeValidator();
        
        // Act
        bool isValid = barCodeValidation.IsValid(validBarCode);
        
        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void IsValidBrazilianBarCode_ValidBrazilianBarCode_ReturnsTrue() {
        // Arrange
        string validBrazilianBarCode = "7891910010905";
        var barCodeValidation = new Ean13BarCodeValidator();
        
        // Act
        var isValid = barCodeValidation.IsValidBrazilianBarCode(validBrazilianBarCode);
        
        // Assert
        Assert.True(isValid);
    }
}