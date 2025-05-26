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
    private readonly ILogger<ProductCompaniesController> _logger;
    
    public ProductCompaniesController(AppDbContext context, ILogger<ProductCompaniesController> logger) {
        _context = context;
        _logger = logger;
    }

    [HttpGet("products", Name = "GetProductCompanies")]
    public ActionResult<IEnumerable<Product>> Get(int companyId) {
        var products = _context.ProductCompanies
                           .AsNoTracking()
                           .Where(pc => pc.CompanyId == companyId)
                           .Include(pc => pc.Product)
                           .Take(10)
                           .ToList();

        if (products is null) {
            _logger.LogWarning($"Produtos não encontrados para a empresa com ID {companyId}");
            return NotFound($"Produtos não encontrados para a empresa com ID {companyId}");
        }
        return Ok(products);
    }

    [HttpGet("product/{productId:int}")]
    public ActionResult<ProductCompanyDTO> Get(int companyId, int productId) {
        var product = _context.ProductCompanies
                              .AsNoTracking()
                              .FirstOrDefault(pc => pc.CompanyId == companyId && pc.ProductId == productId);

        if (product is null) {
            _logger.LogWarning($"Produto com ID {productId} não encontrado para a empresa com ID {companyId}");
            return NotFound($"Produto com ID {productId} não encontrado para a empresa com ID {companyId}");
        }

        return new ProductCompanyDTO(product.ProductCompanyId, product.ProductId, product.CompanyId, product.Price, product.Stock);
    }

    [HttpPost("product")]
    public ActionResult Post(int companyId, [FromBody] ProductCompanyDTO? dto) {
        if (dto is null || dto.CompanyId != companyId) {
            _logger.LogWarning($"Dados inválidos.");
            return BadRequest($"Dados inválidos.");
        }
        
        var company = _context.Companies.Find(companyId);
        if (company is null) {
            _logger.LogWarning($"Empresa com ID {companyId} não encontrada.");
            return NotFound($"Empresa com ID {companyId} não encontrada.");
        }
        
        var product = _context.Products.Find(dto.ProductId);
        if (product is null) {
            _logger.LogWarning($"Produto com ID {dto.ProductId} não encontrado.");
            return NotFound($"PRoduto com ID {dto.ProductId} não encontrado.");
        }
        
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