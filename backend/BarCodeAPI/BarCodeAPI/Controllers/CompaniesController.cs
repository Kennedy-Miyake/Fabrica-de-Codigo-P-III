// ReSharper disable all
using BarCodeAPI.Context;
using Microsoft.AspNetCore.Mvc;

namespace BarCodeAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class CompaniesController : ControllerBase {
    private readonly AppDbContext _context;

    public CompaniesController(AppDbContext context) {
        _context = context;
    }
}