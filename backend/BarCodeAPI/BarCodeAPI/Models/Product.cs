// ReSharper disable all
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BarCodeAPI.Models;

[Table("Products")]
public class Product {
    [Key]
    public int ProductId { get; set; }
    [Required]
    [StringLength(64)]
    public string? Name { get; set; } 
    [Required]
    [StringLength(10000)]
    public string? Description { get; set; }
    [Required]
    [StringLength(255)]
    public string? ImageUrl { get; set; }
    [Required]
    [StringLength(13)]
    public string? BarCode { get; set; }
}