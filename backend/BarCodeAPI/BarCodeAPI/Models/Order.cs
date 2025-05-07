// ReSharper disable all
namespace BarCodeAPI.Models;

public class Order {
    public int OrderId { get; set; }
    public string? DeliveryAddress { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal OrderTotal { get; set; }
    
    public int ClientId { get; set; }
    public Client? Client { get; set; }
}