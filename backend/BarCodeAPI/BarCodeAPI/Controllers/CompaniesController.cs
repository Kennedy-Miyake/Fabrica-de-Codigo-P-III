// ReSharper disable all
using BarCodeAPI.Context;
using BarCodeAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BarCodeAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class CompaniesController : ControllerBase {
    private readonly AppDbContext _context;

    public CompaniesController(AppDbContext context) {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Company>> Get() {
        var companies = _context.Companies.ToList();
        if (companies is null)
            return NotFound("Empresas não encontradas...");
        return companies;
    }

    [HttpGet("{id:int}", Name = "GetCompany")]
    public ActionResult<Company> Get(int id) {
        var company = _context.Companies.FirstOrDefault(p => p.CompanyId == id);
        if (company is null)
            return NotFound("Empresa não encontrada.");
        return company;
    }
}