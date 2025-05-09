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

    [HttpGet("{id:int}", Name = "GetOrder")]
    public ActionResult<Order> Get(int id) {
        var order = _context.Orders.FirstOrDefault(p => p.OrderId == id);
        if(order is null)
            return NotFound("Pedido não encontrado.");
        return order;
    }

    [HttpPost]
    public ActionResult<Order> Post(Order order) {
        if (order is null)
            return BadRequest();
        
        _context.Orders.Add(order);
        _context.SaveChanges();
        
        return CreatedAtRoute("GetOrder", new { id = order.OrderId }, order);
    }
}