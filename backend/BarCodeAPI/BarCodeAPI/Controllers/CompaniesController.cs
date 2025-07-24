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
    private readonly ILogger<CompaniesController> _logger;

    public CompaniesController(AppDbContext context, ILogger<CompaniesController> logger) {
        _context = context;
        _logger = logger;
    }

    [HttpGet("companies")]
    public async Task<ActionResult<IEnumerable<Company>>> GetAllCompanies() {
        var companies = await _context.Companies.AsNoTracking().Take(10).ToListAsync();
        if (companies is null) {
            _logger.LogWarning($"Nenhuma empresa encontrada.");
            return NotFound($"Nenhuma empresa encontrada.");
        }

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
        if (company is null) {
            _logger.LogWarning($"Nenhuma empresa com id = {id} encontrada.");   
            return NotFound($"Nenhuma empresa com id = {id} encontrada.");
        }
        
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
    public async Task<ActionResult> CreateCompany(CompanyDTO dto) {
        if (dto is null) {
            _logger.LogWarning($"Dados inválidos.");
            return BadRequest("Dados inválidos.");
        }

        var company = new Company {
            CompanyId = dto.CompanyId,
            Name = dto.Name,
            CNPJ = dto.CNPJ,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address
        };
        
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
        
        return CreatedAtRoute(nameof(GetCompany), new { id = company.CompanyId }, dto);
    }

    [HttpPut("company/{id:int}")]
    public async Task<ActionResult> UpdateCompany(int id, CompanyDTO dto) {
        if (id != dto.CompanyId) {
            _logger.LogWarning($"Dados inválidos.");
            return BadRequest("Dados inválidos.");
        }
        
        var company = await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == id);

        if (company is null) {
            _logger.LogWarning($"Nenhuma empresa encontrada.");   
            return NotFound($"Empresa não encontrada.");
        }
        
        company.Name = dto.Name;
        company.CNPJ = dto.CNPJ;
        company.Email = dto.Email;
        company.Phone = dto.Phone;
        company.Address = dto.Address;

        _context.Entry(company).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        
        var companyDTO = new CompanyDTO(
            company.CompanyId,
            company.Name,
            company.CNPJ,
            company.Email,
            company.Phone,
            company.Address);
        
        return Ok(companyDTO);
    }

    [HttpDelete("company/{id:int}")]
    public async Task<ActionResult> DeleteCompany(int id) {
        var company = await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == id);
        if (company is null) {
            _logger.LogWarning($"Empresa não encontrada.");
            return NotFound($"Empresa não encontrada.");
        }
        
        var companyDTO = new CompanyDTO(
            company.CompanyId,
            company.Name,
            company.CNPJ,
            company.Email,
            company.Phone,
            company.Address);
        
        _context.Companies.Remove(company);
        _context.SaveChanges();
        
        return Ok(companyDTO);
    }
}