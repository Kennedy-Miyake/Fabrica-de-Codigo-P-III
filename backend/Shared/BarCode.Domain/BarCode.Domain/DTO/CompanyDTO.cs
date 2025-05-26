namespace BarCode.Domain.DTO;

public record CompanyDTO(
    int CompanyId,
    string Name,
    string CNPJ,
    string Email,
    string Phone,
    string Address) {
    
    public CompanyDTO FromModel(CompanyDTO model) => new(
        model.CompanyId,
        model.Name,
        model.CNPJ,
        model.Email,
        model.Phone,
        model.Address);
}