// ReSharper disable all
using BarCode.Infrastructure.Context;
using BarCode.Domain.Models;
using BarCode.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarCodeAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IAutomaticRegistration _automaticRegistration;

    public ProductsController(AppDbContext context, IAutomaticRegistration automaticRegistration) {
        _context = context;
        _automaticRegistration = automaticRegistration;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> Get()
    {
        var products = _context.Products.AsNoTracking().Take(10).ToList();
        if (products is null)
            return NotFound("Produtos não encontrados...");
        return products;
    }

    [HttpGet("{id:int}", Name = "GetProduct")]
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

    [HttpGet("{barcode}")]
    public ActionResult<Product> GetByBarcode(string barcode)
    {
        var product = _context.Products.AsNoTracking()
            .FirstOrDefault(p => p.BarCode == barcode);

        if (product is null)
            return NotFound("Produto não encontrado.");

        return product;
    }

    [HttpPost]
    public ActionResult<Product> Post(Product product)
    {
        if (product is null)
            return BadRequest();

        _context.Products.Add(product);
        _context.SaveChanges();
        
        return new CreatedAtRouteResult("GetProduct", new { id = product.ProductId }, product);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Product> Put(int id, Product product)
    {
        if (id != product.ProductId)
            return BadRequest();

        _context.Entry(product).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
        if (product is null)
            return NotFound("Produto não encontrado...");

        _context.Products.Remove(product);
        _context.SaveChanges();

        return Ok(product);
    }
}