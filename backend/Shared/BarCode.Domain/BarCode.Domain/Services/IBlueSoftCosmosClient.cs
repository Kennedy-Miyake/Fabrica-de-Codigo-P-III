namespace BarCode.Domain.Services;

public interface IBlueSoftCosmosClient {
    Task<string?> GetProductJsonAsync(string barcode, CancellationToken cancellationToken = default);
}