using BarCode.Domain.Services;

namespace BarCode.Infrastructure.Services;

public class Ean13BarCodeValidator : IBarCodeValidation {
    private int SumTotalMultiple(string barcode, int position = 0) {
        int length = barcode.Length - 1;
        if (position == length) return 0;
        if (position % 2 == 0) {
            return ((int)char.GetNumericValue(barcode[position]) * 1) + SumTotalMultiple(barcode, position + 1);
        }
        return ((int)char.GetNumericValue(barcode[position]) * 3) + SumTotalMultiple(barcode, position + 1);
    }
    
    public bool IsValid(string barcode) {
        int checkDigit = (int)char.GetNumericValue(barcode[barcode.Length - 1]);
        int totalSum = SumTotalMultiple(barcode);
        int nextMultipleOfTen = (totalSum + 9) / 10 * 10;
        
        return (nextMultipleOfTen - totalSum) == checkDigit;
    }

    public bool IsValidBrazilianBarCode(string barcode) {
        // Códigos verificadores brasileiros
        string checkDigit = barcode.Substring(0, 3);
        List<string> brazilianVerificationDigits = new List<string>() {
            "789", "790"
        };
        
        return brazilianVerificationDigits.Contains(checkDigit);

    }
}