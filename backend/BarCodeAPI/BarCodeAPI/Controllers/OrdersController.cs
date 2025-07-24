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
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(AppDbContext context, ILogger<OrdersController> logger) {
        _context = context;
        _logger = logger;
    }

    [HttpGet("orders")]
    public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders() {
        var orders = await _context.Orders.AsNoTracking().Take(10).ToListAsync();
        if (orders is null) {
            _logger.LogWarning($"Pedidos não encontrados.");
            return NotFound($"Pedidos não encontrados.");
        }

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
        if (order is null) {
            _logger.LogWarning($"Pedido com ID {id} não encontrado.");
            return NotFound($"Pedido com ID {id} não encontrado.");
        }
        
        var orderDTO = new OrderDTO(
                                    order.OrderId,
                                    order.DeliveryAddress,
                                    order.OrderDate,
                                    order.OrderTotal,
                                    order.ClientId);

        return Ok(orderDTO);
    }

    [HttpPost("order")]
    public async Task<ActionResult<Order>> CreateOrder(OrderDTO dto) {
        if (dto is null) {
            _logger.LogWarning($"Dados inválidos");           
            return BadRequest($"Dados inválidos.");
        }

        var order = new Order {
            DeliveryAddress = dto.DeliveryAddress,
            OrderDate = dto.OrderDate,
            OrderTotal = dto.OrderTotal,
            ClientId = dto.ClientId
        };
        
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        
        return CreatedAtRoute(nameof(GetOrder), new { id = order.OrderId }, dto);
    }

    [HttpPut("order/{id:int}")]
    public async Task<ActionResult<Order>> UpdateOrder(int id, OrderDTO dto) {
        if (id != dto.OrderId) {
            _logger.LogWarning($"Dados inválidos.");
            return BadRequest($"Dados inválidos.");
        }
        
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == id);
        
        order.DeliveryAddress = dto.DeliveryAddress;
        order.OrderDate = dto.OrderDate;
        order.OrderTotal = dto.OrderTotal;
        order.ClientId = dto.ClientId;

        _context.Entry(order).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        
        var orderDTO = new OrderDTO(
            order.OrderId,
            order.DeliveryAddress,
            order.OrderDate,
            order.OrderTotal,
            order.ClientId);
        
        return Ok(orderDTO);
    }

    [HttpDelete("order/{id:int}")]
    public async Task<ActionResult<Order>> DeleteOrder(int id) {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == id);
        if (order is null) {
            _logger.LogWarning($"Pedido não encontrado.");
            return NotFound($"Pedido não encontrado.");
        }
        
        var orderDTO = new OrderDTO(
            order.OrderId,
            order.DeliveryAddress,
            order.OrderDate,
            order.OrderTotal,
            order.ClientId);
        
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        
        return Ok(orderDTO);
    }
}