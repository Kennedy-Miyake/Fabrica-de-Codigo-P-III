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

    [HttpGet("product/{productId:int}")]
    public ActionResult<ProductCompanyDTO> Get(int companyId, int productId) {
        var product = _context.ProductCompanies
                              .AsNoTracking()
                              .FirstOrDefault(pc => pc.CompanyId == companyId && pc.ProductId == productId);

        if (product is null)
            return NotFound("Produto não encontrado.");

        return new ProductCompanyDTO(product.ProductCompanyId, product.ProductId, product.CompanyId, product.Price, product.Stock);
    }

    [HttpPost("product")]
    public ActionResult Post(int companyId, [FromBody] ProductCompanyDTO? dto) {
        if (dto is null || dto.CompanyId != companyId)
            return BadRequest();
        
        var company = _context.Companies.Find(companyId);
        if (company is null)
            return NotFound("Empresa não encontrada");
        
        var product = _context.Products.Find(dto.ProductId);
        if (product is null)
            return NotFound("Produto não encontrado");
        
        var productCompany = new ProductCompany {
            CompanyId = companyId,
            ProductId = dto.ProductId,
            Price = dto.Price,
            Stock = dto.Stock
        };
        
        _context.Add(productCompany);
        _context.SaveChanges();
        
        return CreatedAtRoute("GetProductCompanies", new { companyId = productCompany.CompanyId }, productCompany);
    }
}