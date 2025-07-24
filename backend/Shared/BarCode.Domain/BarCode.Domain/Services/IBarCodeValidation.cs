namespace BarCode.Domain.Services;

public interface IBarCodeValidation {
    bool IsValid(string barcode);
    bool IsValidBrazilianBarCode(string barcode);
}