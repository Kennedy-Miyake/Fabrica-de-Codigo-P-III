namespace BarCode.Domain.DTO;

public record ClientDTO(
    int ClientId,
    string Name,
    string Email,
    string Phone,
    string Address) {
    public ClientDTO FromModel(ClientDTO model) => new(
                                                       model.ClientId, 
                                                       model.Name, 
                                                       model.Email,
                                                       model.Phone,
                                                       model.Address);
}