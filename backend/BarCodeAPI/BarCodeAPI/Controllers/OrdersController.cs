// ReSharper disable all

using BarCode.Domain.DTO;
using BarCode.Infrastructure.Context;
using BarCode.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarCodeAPI.Controllers;

[Route("api/v1")]
[ApiController]
public class OrdersController : ControllerBase {
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context) {
        _context = context;
    }

    [HttpGet("orders")]
    public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders() {
        var orders = await _context.Orders.AsNoTracking().Take(10).ToListAsync();
        if(orders is null)
            return NotFound("Pedidos não encontrados...");

        var orderDTOs = orders.Select(order => new OrderDTO(
                                                         order.ClientId,
                                                         order.DeliveryAddress,
                                                         order.OrderDate,
                                                         order.OrderTotal,
                                                         order.ClientId)).ToList();
        
        return Ok(orderDTOs);
    }

    [HttpGet("order/{id:int}", Name = "GetOrder")]
    public async Task<ActionResult<Order>> GetOrder(int id) {
        var order = await _context.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.OrderId == id);
        if(order is null)
            return NotFound("Pedido não encontrado.");
        
        var orderDTO = new OrderDTO(
                                    order.OrderId,
                                    order.DeliveryAddress,
                                    order.OrderDate,
                                    order.OrderTotal,
                                    order.ClientId);

        return Ok(orderDTO);
    }

    [HttpPost("order")]
    public ActionResult<Order> Post(Order order) {
        if (order is null)
            return BadRequest();
        
        _context.Orders.Add(order);
        _context.SaveChanges();
        
        return CreatedAtRoute("GetOrder", new { id = order.OrderId }, order);
    }

    [HttpPut("order/{id:int}")]
    public ActionResult<Order> Put(int id, Order order) {
        if (id != order.OrderId)
            return BadRequest();

        _context.Entry(order).State = EntityState.Modified;
        _context.SaveChanges();
        
        return Ok(order);
    }

    [HttpDelete("order/{id:int}")]
    public ActionResult<Order> Delete(int id) {
        var order = _context.Orders.FirstOrDefault(p => p.OrderId == id);
        if (order is null)
            return NotFound("Pedido não encontrado.");
        
        _context.Orders.Remove(order);
        _context.SaveChanges();
        
        return Ok(order);
    }
}