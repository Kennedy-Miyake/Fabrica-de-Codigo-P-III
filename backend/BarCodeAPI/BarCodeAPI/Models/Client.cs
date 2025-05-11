// ReSharper disable all
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BarCodeAPI.Models;

[Table("Clients")]
public class Client {
    [Key]
    public int ClientId { get; set; }
    [Required]
    [StringLength(64)]
    public string? Name { get; set; }
    [Required]
    [StringLength(10000)]
    public string? Email { get; set; }
    [Required]
    [StringLength(13)]
    public string? Phone { get; set; }
    [Required]
    [StringLength(128)]
    public string? Address { get; set; }
}