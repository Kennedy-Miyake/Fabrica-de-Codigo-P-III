// ReSharper disable all
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BarCode.Domain.Models;

[Table("Orders")]
public class Order {
    [Key]
    public int OrderId { get; set; }
    [Required]
    [StringLength(128)]
    public string? DeliveryAddress { get; set; }

    [Required] 
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal OrderTotal { get; set; }
    
    [ForeignKey("ClientId")]
    public int ClientId { get; set; }
    [JsonIgnore]
    public Client? Client { get; set; }
}