using BarCode.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using BarCode.Domain.DTO;
using BarCode.Domain.Models;
using BarCode.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace BarCodeAPI.Controllers;

[Route("api/v1")]
[ApiController]
public class OrderItemsController : ControllerBase {
    private readonly AppDbContext _context;
    private readonly ILogger<OrderItemsController> _logger;
    private readonly ICartService _cartService;
    
    public OrderItemsController(AppDbContext context, ILogger<OrderItemsController> logger, ICartService cartService) {
        _context = context;
        _logger = logger;
        _cartService = cartService;
    }

    [HttpGet("orderitems")]
    public async Task<ActionResult<IEnumerable<OrderItemDTO>>> GetAllOrderItems() {
        var orderItems = await _context.OrderItems.AsNoTracking().Take(10).ToListAsync();
        if (orderItems is null) {
            _logger.LogWarning($"Itens dos pedidos não encontrados.");
            return NotFound($"Itens dos pedidos não encontrados.");
        }

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

        if (orderItem is null) {
            _logger.LogWarning($"Item do pedido com ID {orderItemId} não encontrado.");
            return NotFound($"Item do pedido com ID {orderItemId} não encontrado.");
        }

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

        if (!orderItems.Any()) {
            _logger.LogWarning($"Itens do pedido com ID {orderId} não encontrados.");
            return NotFound($"Itens do pedido com ID {orderId} não encontrados.");
        }
        
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
        if (dto is null) {
            _logger.LogWarning($"Dados inválidos.");   
            return BadRequest($"Dados inválidos");
        }
        var order = await _context.Orders.FindAsync(dto.OrderId);
        if (order is null) {
            _logger.LogWarning($"Pedido com ID {dto.OrderId} não encontrado.");
            return NotFound($"Pedido com ID {dto.OrderId} não encontrado.");
        }
        
        var productCompany = await _context.ProductCompanies.FindAsync(dto.ProductCompanyId);
        if (productCompany is null) {
            _logger.LogWarning($"Produto com ID {dto.ProductCompanyId} não encontrado.");
            return NotFound($"Produto com ID {dto.ProductCompanyId} não encontrado.");
        }
        
        var orderItem = new OrderItem {
            OrderId = dto.OrderId,
            ProductCompanyId = dto.ProductCompanyId,
            Quantity = dto.Quantity,
            UnitaryPrice = productCompany.Price,
            SubTotal = _cartService.CalculateSubtotal(productCompany.Price, dto.Quantity)
        };
        
        _context.OrderItems.Add(orderItem);
        _cartService.CalculateTotal(ref order, orderItem.SubTotal);
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