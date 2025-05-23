using BarCode.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using BarCode.Domain.DTO;
using BarCode.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BarCodeAPI.Controllers;

[Route("api/v1")]
[ApiController]
public class OrderItemController : ControllerBase {
    private readonly AppDbContext _context;
    
    public OrderItemController(AppDbContext context) {
        _context = context;
    }

    [HttpGet("orderitems", Name = "GetOrderItems")]
    public async Task<ActionResult<IEnumerable<OrderItemDTO>>> GetAllOrderItems() {
        var orderItems = await _context.OrderItems.AsNoTracking().Take(10).ToListAsync();
        if (orderItems is null)
            return NotFound("Itens dos pedidos não encontrados...");

        var orderItemDTOs = orderItems.Select(item => new OrderItemDTO(
                                                                                      item.OrderItemId,
                                                                                      item.OrderId,
                                                                                      item.ProductCompanyId,
                                                                                      item.Quantity,
                                                                                      item.UnitaryPrice,
                                                                                      item.SubTotal)).ToList();
        return Ok(orderItemDTOs);
    }

    [HttpGet("orderitems/order")]
    public async Task<ActionResult<OrderItemDTO>> GetAllOrderItemsByOrderId([FromQuery] int orderId) {
        var orderItems = await _context.OrderItems
                                       .AsNoTracking()
                                       .Where(oi => oi.OrderId == orderId)
                                       .ToListAsync();

        if (!orderItems.Any())
            return NotFound("Itens dos pedidos não encontrados...");
        
        var orderItemDTOs = orderItems.Select(item => new OrderItemDTO(
                                                                                      item.OrderItemId,
                                                                                      item.OrderId,
                                                                                      item.ProductCompanyId,
                                                                                      item.Quantity,
                                                                                      item.UnitaryPrice,
                                                                                      item.SubTotal)).ToList();
        return Ok(orderItemDTOs);
    }

    [HttpPost("orderitem")]
    public async Task<ActionResult> PostOrderItem([FromBody] OrderItemDTO? dto) {
        if(dto is null)
            return BadRequest();
        var order = await _context.Orders.FindAsync(dto.OrderId);
        if (order is null)
            return NotFound("Pedido não encontrado");
        
        var productCompany = await _context.ProductCompanies.FindAsync(dto.ProductCompanyId);
        if (productCompany is null)
            return NotFound("Produto não encontrado");
        
        var orderItem = new OrderItem {
            OrderId = dto.OrderId,
            ProductCompanyId = dto.ProductCompanyId,
            Quantity = dto.Quantity,
            UnitaryPrice = dto.UnitaryPrice,
            SubTotal = dto.SubTotal
        };
        
        _context.Add(orderItem);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction("GetOrderItem", new { id = orderItem.OrderId }, orderItem);
    }
}