// ReSharper disable All
namespace PopulaDbWorker.Services;

public interface IBlueSoftCosmosClient {
    Task<string?> GetProductJsonAsync(string barcode, CancellationToken cancellationToken = default);
}