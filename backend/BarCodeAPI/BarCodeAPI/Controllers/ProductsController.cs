// ReSharper disable all
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
    public ActionResult<IEnumerable<Product>> Get() {
        var products = _context.Products.AsNoTracking().Take(10).ToList();
        if (products is null)
            return NotFound("Produtos não encontrados...");
        return products;
    }

    [HttpGet("product/{id:int}", Name = "GetProduct")]
    public ActionResult<Product> Get(int id) {
        var product = _context.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == id);
        if (product is null)
            return NotFound("Produto não encontrado.");
        return product;
    }

    [HttpGet("lookup/{barcode}")]
    public async Task<ActionResult<Product>> Lookup(string barcode) {
        return await _automaticRegistration.FillInProductInformationAsync(barcode);
    }

    [HttpGet("product/{barcode}")]
    public ActionResult<Product> GetByBarcode(string barcode) {
        var product = _context.Products.AsNoTracking()
            .FirstOrDefault(p => p.BarCode == barcode);

        if (product is null)
            return NotFound("Produto não encontrado.");

        return product;
    }

    [HttpPost("product")]
    public ActionResult<Product> Post(Product product) {
        if (product is null)
            return BadRequest();
        if (_barCodeValidation.IsValid(product.BarCode!) &&
            _barCodeValidation.IsValidBrazilianBarCode(product.BarCode!)) {
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        else {
            return BadRequest("Código de barras inválido.");
        }
        
        return new CreatedAtRouteResult("GetProduct", new { id = product.ProductId }, product);
    }

    [HttpPut("product/{id:int}")]
    public ActionResult<Product> Put(int id, Product product) {
        if (id != product.ProductId)
            return BadRequest();

        _context.Entry(product).State = EntityState.Modified;
        _context.SaveChanges();
        return Ok(product);
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