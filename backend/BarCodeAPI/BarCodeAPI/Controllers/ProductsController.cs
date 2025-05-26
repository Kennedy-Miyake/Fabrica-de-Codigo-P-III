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
    private readonly IAutomaticRegistration _automaticRegistration;
    private readonly IBarCodeValidation _barCodeValidation;

    public ProductsController(AppDbContext context, 
                              IAutomaticRegistration automaticRegistration, 
                              IBarCodeValidation barCodeValidation) {
        _context = context;
        _automaticRegistration = automaticRegistration;
        _barCodeValidation = barCodeValidation;
    }

    [HttpGet("products")]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts() {
        var products = await _context.Products.AsNoTracking().Take(10).ToListAsync();
        if (products is null)
            return NotFound("Produtos não encontrados...");
        
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
        if (product is null)
            return NotFound("Produto não encontrado.");
       
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

        if (product is null)
            return NotFound("Produto não encontrado.");
        
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
        if (dto is null)
            return BadRequest();
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
            return BadRequest("Código de barras inválido.");
        }
        
        return new CreatedAtRouteResult(nameof(GetProduct), new { id = product.ProductId }, dto);
    }

    [HttpPut("product/{id:int}")]
    public async Task<ActionResult<Product>> UpdateProduct(int id, ProductDTO dto) {
        if (id != dto.ProductId)
            return BadRequest();
        
        var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
        
        product.Name = dto.Name;
        product.Description = dto.Description;
        product.ImageUrl = dto.ImageUrl;
        if(_barCodeValidation.IsValid(dto.BarCode) &&
           _barCodeValidation.IsValidBrazilianBarCode(dto.BarCode)) {
            product.BarCode = dto.BarCode;
        }
        else
            return BadRequest("Código de barras inválido.");

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
    public ActionResult Delete(int id) {
        var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
        if (product is null)
            return NotFound("Produto não encontrado...");

        _context.Products.Remove(product);
        _context.SaveChanges();

        return Ok(product);
    }
}