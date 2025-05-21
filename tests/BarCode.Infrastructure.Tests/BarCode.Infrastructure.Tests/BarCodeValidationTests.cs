namespace BarCode.Infrastructure.Tests;

public class BarCodeValidationTests {
    [Fact]
    public void IsValid_ValidBarCode_ReturnsTrue() {
        // Arrange
        string validBarCode = "7891910010905";
        int expectedCheckDigit = 5;
        
        // Act
        // Posições ímpares devem ser multiplicados por 1 e somados
        int firstResult = 0;
        for (int i = 0; i < (validBarCode.Length)-1; i += 2) {
            firstResult += (int)char.GetNumericValue(validBarCode[i]) * 1;
        }
        // Posições pares devem ser multiplicados por 3 e somados
        int secondResult = 0;
        for (int i = 1; i < validBarCode.Length; i += 2) {
            secondResult += (int)char.GetNumericValue(validBarCode[i]) * 3;
        }
        // Soma total
        int totalSum = firstResult + secondResult; 
        // Próximo múltiplo de 10
        int nextMultipleOfTen = (totalSum + 9) / 10 * 10;
        // Digito verificador
        int checkDigit = nextMultipleOfTen - totalSum;
        
        // Assert
        Assert.Equal(expectedCheckDigit, checkDigit);
    }
}