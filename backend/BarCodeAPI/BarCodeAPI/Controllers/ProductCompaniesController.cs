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
    
    public ProductCompaniesController(AppDbContext context) {
        _context = context;
    }
}