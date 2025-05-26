// ReSharper disable all

using BarCode.Domain.DTO;
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
    public async Task<ActionResult<IEnumerable<Company>>> GetAllCompanies() {
        var companies = await _context.Companies.AsNoTracking().Take(10).ToListAsync();
        if (companies is null)
            return NotFound("Empresas não encontradas...");

        var companiesDTO = companies.Select(company => new CompanyDTO(
                                                                      company.CompanyId,
                                                                      company.Name,
                                                                      company.CNPJ,
                                                                      company.Email,
                                                                      company.Phone,
                                                                      company.Address)).ToList();
        
        return Ok(companiesDTO);
    }

    [HttpGet("company/{id:int}", Name = "GetCompany")]
    public async Task<ActionResult<Company>> GetCompany(int id) {
        var company = await _context.Companies.AsNoTracking().FirstOrDefaultAsync(p => p.CompanyId == id);
        if (company is null)
            return NotFound("Empresa não encontrada.");
        
        var companyDTO = new CompanyDTO(
                                        company.CompanyId,
                                        company.Name,
                                        company.CNPJ,
                                        company.Email,
                                        company.Phone,
                                        company.Address);

        return Ok(companyDTO);
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