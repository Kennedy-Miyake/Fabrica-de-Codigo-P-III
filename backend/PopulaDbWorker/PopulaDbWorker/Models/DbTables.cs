namespace PopulaDbWorker.Models;

public enum ProductAttributes {
    NAME,
    DESCRIPTION,
    IMAGE_URL,
    BARCODE,
    LENGTH_ENUM
}

public class DbTables {
    private List<string?> productAttributes = new List<string?>();

    public DbTables() {
        for(int i = 0; i < (int)ProductAttributes.LENGTH_ENUM; i++) productAttributes.Add((Convert.ToString((ProductAttributes)i)));
    }

    public string? GetProductAttribute(ProductAttributes attribute) {
        return productAttributes[(int)attribute];
    }
}