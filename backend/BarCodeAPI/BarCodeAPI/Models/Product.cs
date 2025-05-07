// ReSharper disable all
namespace BarCodeAPI.Models;

public class Product {
    public int ProductId { get; set; }
    public string? Name { get; set; } 
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? BarCode { get; set; }
}