namespace BarCode.Domain.DTO;

public record ProductDTO(
    int ProductId,
    string Name,
    string Description,
    string ImageUrl,
    string BarCode) {
    public ProductDTO FromModel(ProductDTO model) => new(
                                                         model.ProductId,
                                                         model.Name,
                                                         model.Description,
                                                         model.ImageUrl,
                                                         model.BarCode);
}

public record ProductCreateDTO(
    string Name,
    string Description,
    string ImageUrl,
    string BarCode) {
    public ProductCreateDTO FromModel(ProductCreateDTO model) => new(
                                                                      model.Name,
                                                                      model.Description,
                                                                      model.ImageUrl,
                                                                      model.BarCode);
}