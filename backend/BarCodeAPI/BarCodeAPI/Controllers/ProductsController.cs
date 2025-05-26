// ReSharper disable all

using BarCode.Domain.DTO;
using BarCode.Infrastructure.Context;
using BarCode.Domain.Models;
using BarCode.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarCodeAPI.Controllers;

[Route("api/v1")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProductsController> _logger;
    private readonly IAutomaticRegistration _automaticRegistration;
    private readonly IBarCodeValidation _barCodeValidation;

    public ProductsController(AppDbContext context,
                              ILogger<ProductsController> logger,
                              IAutomaticRegistration automaticRegistration, 
                              IBarCodeValidation barCodeValidation) {
        _context = context;
        _logger = logger;
        _automaticRegistration = automaticRegistration;
        _barCodeValidation = barCodeValidation;
    }

    [HttpGet("products")]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts() {
        var products = await _context.Products.AsNoTracking().Take(10).ToListAsync();
        if (products is null) {
            _logger.LogWarning($"Produtos não encontrados.");
            return NotFound($"Produtos não encontrados.");
        }
        
        var productDTOs = products.Select(product => new ProductDTO(
                                                                    product.ProductId,
                                                                    product.Name,
                                                                    product.Description,
                                                                    product.ImageUrl,
                                                                    product.BarCode)).ToList();
        
        return Ok(productDTOs);
    }

    [HttpGet("product/{id:int}", Name = "GetProduct")]
    public async Task<ActionResult<Product>> GetProduct(int id) {
        var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
        if (product is null) {
            _logger.LogWarning($"Produto com ID {id} não encontrado.");
            return NotFound($"Produto com ID {id} não encontrado.");
        }
       
        var productDTO = new ProductDTO(
                                        product.ProductId,
                                        product.Name,
                                        product.Description,
                                        product.ImageUrl,
                                        product.BarCode);

        return Ok(productDTO);
    }

    [HttpGet("lookup/{barcode}")]
    public async Task<ActionResult<Product>> Lookup(string barcode) {
        return await _automaticRegistration.FillInProductInformationAsync(barcode);
    }

    [HttpGet("product/{barcode}")]
    public async Task<ActionResult<Product>> GetProductByBarcode(string barcode) {
        var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.BarCode == barcode);

        if (product is null) {
            _logger.LogWarning($"Produto com código de barras {barcode} não encontrado.");
            return NotFound($"Produto com código de barras {barcode} não encontrado.");
        }
        
        var productDTO = new ProductDTO(
                                        product.ProductId,
                                        product.Name,
                                        product.Description,
                                        product.ImageUrl,
                                        product.BarCode);

        return Ok(productDTO);
    }

    [HttpPost("product")]
    public async Task<ActionResult<Product>> CreateProduct(ProductDTO dto) {
        var product = new Product();
        if (dto is null) {
            _logger.LogWarning($"Dados inválidos.");
            return BadRequest($"Dados inválidos.");
        }
        if (_barCodeValidation.IsValid(dto.BarCode!) &&
            _barCodeValidation.IsValidBrazilianBarCode(dto.BarCode!)) {

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.ImageUrl = dto.ImageUrl;
            product.BarCode = dto.BarCode;
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        else {
            _logger.LogWarning($"Código de barras inválido.");
            return BadRequest("Código de barras inválido.");
        }
        
        return new CreatedAtRouteResult(nameof(GetProduct), new { id = product.ProductId }, dto);
    }

    [HttpPut("product/{id:int}")]
    public async Task<ActionResult<Product>> UpdateProduct(int id, ProductDTO dto) {
        if (id != dto.ProductId) {
            _logger.LogWarning($"Dados inválidos.");
            return BadRequest($"Dados inválidos.");
        }
        
        var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
        
        product.Name = dto.Name;
        product.Description = dto.Description;
        product.ImageUrl = dto.ImageUrl;
        if(_barCodeValidation.IsValid(dto.BarCode) &&
           _barCodeValidation.IsValidBrazilianBarCode(dto.BarCode)) {
            product.BarCode = dto.BarCode;
        }
        else {
            _logger.LogWarning($"Código de barras inválido.");
            return BadRequest("Código de barras inválido.");
        }

        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        
        var productDTO = new ProductDTO(
                                        product.ProductId,
                                        product.Name,
                                        product.Description,
                                        product.ImageUrl,
                                        product.BarCode);
        
        return Ok(productDTO);
    }

    [HttpDelete("product/{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id) {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
        if (product is null) {
            _logger.LogWarning($"Produto com ID {id} não encontrado.");
            return NotFound($"Produto com ID {id} não encontrado.");
        }
        
        var productDTO = new ProductDTO(
                                        product.ProductId,
                                        product.Name,
                                        product.Description,
                                        product.ImageUrl,
                                        product.BarCode);

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return Ok(productDTO);
    }
}