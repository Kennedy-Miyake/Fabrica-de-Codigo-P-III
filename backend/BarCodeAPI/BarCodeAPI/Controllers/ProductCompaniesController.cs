using BarCode.Domain.DTO;
using BarCode.Domain.Models;
using BarCode.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarCodeAPI.Controllers;

[Route("api/v1/company/{companyId:int}")]
[ApiController]
public class ProductCompaniesController : ControllerBase {
    private readonly AppDbContext _context;
    
    public ProductCompaniesController(AppDbContext context) {
        _context = context;
    }

    [HttpGet("products", Name = "GetProductCompanies")]
    public ActionResult<IEnumerable<Product>> Get(int companyId) {
        var products = _context.ProductCompanies
                           .AsNoTracking()
                           .Where(pc => pc.CompanyId == companyId)
                           .Include(pc => pc.Product)
                           .Take(10)
                           .ToList();
        
        if (products is null)
            return NotFound("Produtos não encontrados...");
        return Ok(products);
    }
}