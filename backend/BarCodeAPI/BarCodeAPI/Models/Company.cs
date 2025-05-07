// ReSharper disable all
namespace BarCodeAPI.Models;

public class Company {
    public int CompanyId { get; set; }
    public string? Name { get; set; }
    public string? CNPJ { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
}