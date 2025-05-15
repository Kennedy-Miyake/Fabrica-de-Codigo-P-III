// ReSharper disable all
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BarCode.Domain.Models;

[Table("Companies")]
public class Company {
    [Key]
    public int CompanyId { get; set; }
    [Required]
    [StringLength(64)]
    public string? Name { get; set; }
    [Required]
    [StringLength(14)]
    public string? CNPJ { get; set; }
    [Required]
    [StringLength(64)]
    public string? Email { get; set; }
    [Required]
    [StringLength(13)]
    public string? Phone { get; set; }
    [Required]
    [StringLength(128)]
    public string? Address { get; set; }
}