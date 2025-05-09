// ReSharper disable all
using BarCodeAPI.Context;
using BarCodeAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BarCodeAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class OrdersController : ControllerBase {
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context) {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Order>> Get() {
        var orders = _context.Orders.ToList();
        if(orders is null)
            return NotFound("Pedidos não encontrados...");
        return orders;
    }
}