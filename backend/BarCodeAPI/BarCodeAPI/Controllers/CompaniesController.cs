// ReSharper disable all
using BarCode.Infrastructure.Context;
using BarCode.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarCodeAPI.Controllers;

[Route("api/v1")]
[ApiController]
public class CompaniesController : ControllerBase {
    private readonly AppDbContext _context;

    public CompaniesController(AppDbContext context) {
        _context = context;
    }

    [HttpGet("companies")]
    public ActionResult<IEnumerable<Company>> Get() {
        var companies = _context.Companies.AsNoTracking().Take(10).ToList();
        if (companies is null)
            return NotFound("Empresas não encontradas...");
        return companies;
    }

    [HttpGet("company/{id:int}", Name = "GetCompany")]
    public ActionResult<Company> Get(int id) {
        var company = _context.Companies.AsNoTracking().FirstOrDefault(p => p.CompanyId == id);
        if (company is null)
            return NotFound("Empresa não encontrada.");
        return company;
    }

    [HttpPost("company")]
    public ActionResult Post(Company company) {
        if (company is null)
            return BadRequest();
        
        _context.Companies.Add(company);
        _context.SaveChanges();
        
        return CreatedAtRoute("GetCompany", new { id = company.CompanyId }, company);
    }

    [HttpPut("company/{id:int}")]
    public ActionResult Put(int id, Company company) {
        if (id != company.CompanyId)
            return BadRequest();

        _context.Entry(company).State = EntityState.Modified;
        _context.SaveChanges();
        
        return Ok(company);
    }

    [HttpDelete("company/{id:int}")]
    public ActionResult Delete(int id) {
        var company = _context.Companies.FirstOrDefault(p => p.CompanyId == id);
        if (company is null)
            return NotFound("Empresa não encontrada...");
        
        _context.Companies.Remove(company);
        _context.SaveChanges();
        
        return Ok(company);
    }
}