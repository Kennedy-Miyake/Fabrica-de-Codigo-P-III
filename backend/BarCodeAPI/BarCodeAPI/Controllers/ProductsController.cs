// ReSharper disable all

using BarCodeAPI.Context;
using BarCodeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarCodeAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController : ControllerBase {
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context) {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> Get() {
        var products = _context.Products.ToList();
        if (products is null)
            return NotFound("Produtos não encontrados...");
        return products;
    }

    [HttpGet("{id:int}")]
    public ActionResult<Product> Get(int id) {
        var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
        if(product is null)
            return NotFound("Produto não encontrado.");
        return product;
    }

    [HttpPost]
    public ActionResult<Product> Post(Product product) {
        if(product is null)
            return BadRequest();
        
        _context.Products.Add(product);
        _context.SaveChanges();
        
        return new CreatedAtRouteResult("products", new { id = product.ProductId }, product);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Product> Put(int id, Product product) {
        if(id != product.ProductId)
            return BadRequest();

        _context.Entry(product).State = EntityState.Modified;
        _context.SaveChanges();
        
        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id) {
        var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
        if(product is null)
            return NotFound("Produto não encontrado...");
        
        _context.Products.Remove(product);
        _context.SaveChanges();

        return Ok(product);
    }
}