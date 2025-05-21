using BarCode.Infrastructure.Services;

namespace BarCode.Infrastructure.Tests;

public class BarCodeValidationTests {
    private int SumTotalMultiple(string barCode, int position = 0) {
        int length = barCode.Length - 1;
        if (position == length) return 0;
        if (position % 2 == 0) {
            return ((int)char.GetNumericValue(barCode[position]) * 1) + SumTotalMultiple(barCode, position + 1);
        }
        return ((int)char.GetNumericValue(barCode[position]) * 3) + SumTotalMultiple(barCode, position + 1);
    }
    [Fact]
    public void IsValid_ValidBarCode_ReturnsTrue() {
        // Arrange
        string validBarCode = "7891910010905";
        var barCodeValidation = new BarCodeValidation();
        
        // Act
        bool isValid = barCodeValidation.IsValid(validBarCode);
        
        // Assert
        Assert.True(isValid);
    }
}