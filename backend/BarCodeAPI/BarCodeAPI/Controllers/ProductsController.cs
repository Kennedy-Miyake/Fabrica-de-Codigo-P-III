// ReSharper disable all

using BarCodeAPI.Context;
using Microsoft.AspNetCore.Mvc;

namespace BarCodeAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController : ControllerBase {
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context) {
        _context = context;
    }
}