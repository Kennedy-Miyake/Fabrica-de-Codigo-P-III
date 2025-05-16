// ReSharper disable all
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BarCode.Domain.Models;

[Table("Orders")]
public class Order {
    [Key]
    public int OrderId { get; set; }
    [Required]
    [StringLength(128)]
    public string? DeliveryAddress { get; set; }
    [Required]
    public DateTime OrderDate { get; set; }
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal OrderTotal { get; set; }
    
    [ForeignKey("ClientId")]
    public int ClientId { get; set; }
    public Client? Client { get; set; }
}