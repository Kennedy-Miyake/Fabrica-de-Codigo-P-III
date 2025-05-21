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
        int expectedCheckDigit = 5;
        
        // Act
        int checkDigit = ((Func<int>)(() => {
            var totalSum = SumTotalMultiple(validBarCode);
            var nextMultipleOfTen = (totalSum + 9) / 10 * 10;
            return nextMultipleOfTen - totalSum;
        }))();
        
        // Assert
        Assert.Equal(expectedCheckDigit, checkDigit);
    }
}