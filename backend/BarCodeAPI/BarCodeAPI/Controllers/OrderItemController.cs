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
}