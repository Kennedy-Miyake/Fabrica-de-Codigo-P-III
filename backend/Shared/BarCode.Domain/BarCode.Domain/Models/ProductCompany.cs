using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BarCode.Domain.Models;

[Table("ProductCompanies")]
public class ProductCompany {
    [Key]
    public int ProductCompanyId { get; set; }
    
    [ForeignKey("ProductId")]
    public int ProductId { get; set; }
    [JsonIgnore]
    public Product? Product { get; set; }
    
    [ForeignKey("CompanyId")]
    public int CompanyId { get; set; }
    [JsonIgnore]
    public Company? Company { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    [Required]
    public int Stock { get; set; }
}