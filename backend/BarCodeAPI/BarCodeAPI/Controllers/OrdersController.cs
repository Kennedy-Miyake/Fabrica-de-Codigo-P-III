// ReSharper disable all
using BarCode.Infrastructure.Context;
using BarCode.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        var orders = _context.Orders.AsNoTracking().Take(10).ToList();
        if(orders is null)
            return NotFound("Pedidos não encontrados...");
        return orders;
    }

    [HttpGet("{id:int}", Name = "GetOrder")]
    public ActionResult<Order> Get(int id) {
        var order = _context.Orders.AsNoTracking().FirstOrDefault(p => p.OrderId == id);
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

    [HttpPut("{id:int}")]
    public ActionResult<Order> Put(int id, Order order) {
        if (id != order.OrderId)
            return BadRequest();

        _context.Entry(order).State = EntityState.Modified;
        _context.SaveChanges();
        
        return Ok(order);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<Order> Delete(int id) {
        var order = _context.Orders.FirstOrDefault(p => p.OrderId == id);
        if (order is null)
            return NotFound("Pedido não encontrado.");
        
        _context.Orders.Remove(order);
        _context.SaveChanges();
        
        return Ok(order);
    }
}