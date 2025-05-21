using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarCode.Domain.Models;

[Table("OrderItems")]
public class OrderItem {
    [Key]
    public int OrderItemId { get; set; }
    
    [ForeignKey("OrderId")]
    public int OrderId { get; set; }
    public Order? Order { get; set; }
    
    [ForeignKey("ProductCompanyId")]
    public int ProductCompanyId { get; set; }
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