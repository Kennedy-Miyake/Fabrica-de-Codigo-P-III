// ReSharper disable all
using BarCodeAPI.Context;
using Microsoft.AspNetCore.Mvc;

namespace BarCodeAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class OrdersController : ControllerBase {
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context) {
        _context = context;
    }
}