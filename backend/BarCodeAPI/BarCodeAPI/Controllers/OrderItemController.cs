using BarCode.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarCodeAPI.Controllers;

[Route("api/v1")]
[ApiController]
public class OrderItemController : ControllerBase {
    private readonly AppDbContext _context;
    
    public OrderItemController(AppDbContext context) {
        _context = context;
    }
}