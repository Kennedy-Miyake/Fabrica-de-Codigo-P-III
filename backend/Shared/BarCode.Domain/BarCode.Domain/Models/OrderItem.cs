using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BarCode.Domain.Models;

[Table("OrderItems")]
public class OrderItem {
    [Key]
    public int OrderItemId { get; set; }
    
    [ForeignKey("OrderId")]
    public int OrderId { get; set; }
    [JsonIgnore]
    public Order? Order { get; set; }
    
    [ForeignKey("ProductCompanyId")]
    public int ProductCompanyId { get; set; }
    [JsonIgnore]
    public ProductCompany? ProductCompany { get; set; }
    
    [Required]
    public int Quantity { get; set; }
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitaryPrice { get; set; }
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal { get; set; }
}