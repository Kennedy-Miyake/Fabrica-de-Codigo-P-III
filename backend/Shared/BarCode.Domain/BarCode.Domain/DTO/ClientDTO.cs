namespace BarCode.Domain.DTO;

public record ClientDTO(
    int ClientID,
    string Name,
    string Email,
    string Phone,
    string Address) {
    public ClientDTO FromModel(ClientDTO model) => new(
                                                       model.ClientID, 
                                                       model.Name, 
                                                       model.Email,
                                                       model.Phone,
                                                       model.Address);
}