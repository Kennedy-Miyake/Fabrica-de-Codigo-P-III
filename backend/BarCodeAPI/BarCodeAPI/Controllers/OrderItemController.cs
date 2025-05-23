using BarCode.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using BarCode.Domain.DTO;
using BarCode.Domain.Models;
using BarCode.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace BarCodeAPI.Controllers;

[Route("api/v1")]
[ApiController]
public class OrderItemController : ControllerBase {
    private readonly AppDbContext _context;
    private readonly ICartService _cartService;
    
    public OrderItemController(AppDbContext context, ICartService cartService) {
        _context = context;
        _cartService = cartService;
    }

    [HttpGet("orderitems")]
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

    [HttpGet("orderitem/{orderItemId:int}", Name = "GetOrderItem")]
    public async Task<ActionResult<OrderItemDTO>> GetOrderItemById(int orderItemId) {
        var orderItem = await _context.OrderItems.AsNoTracking().FirstOrDefaultAsync(p => p.OrderItemId == orderItemId);
        
        if (orderItem is null)
            return NotFound("Item do pedido não encontrado...");

        var orderItemDTO = new OrderItemDTO(
                                            orderItem.OrderItemId,
                                            orderItem.OrderId,
                                            orderItem.ProductCompanyId,
                                            orderItem.Quantity,
                                            orderItem.UnitaryPrice,
                                            orderItem.SubTotal);
        return Ok(orderItemDTO);
    }

    [HttpGet("orderitems/order/{orderId:int}")]
    public async Task<ActionResult<OrderItemDTO>> GetAllOrderItemsByOrderId(int orderId) {
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
    public async Task<ActionResult> PostOrderItem([FromBody] OrderItemCreateDTO? dto) {
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
            UnitaryPrice = productCompany.Price,
            SubTotal = _cartService.CalculateSubtotal(productCompany.Price, dto.Quantity)
        };
        
        _context.Add(orderItem);
        await _context.SaveChangesAsync();
        
        var orderItemDTO = new OrderItemDTO(
            orderItem.OrderItemId,
            orderItem.OrderId,
            orderItem.ProductCompanyId,
            orderItem.Quantity,
            orderItem.UnitaryPrice,
            orderItem.SubTotal);
        
        return CreatedAtAction(nameof(GetOrderItemById), new { orderItemId = orderItem.OrderItemId }, orderItemDTO);
    }
}