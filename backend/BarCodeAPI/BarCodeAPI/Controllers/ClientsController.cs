// ReSharper disable all
using BarCodeAPI.Context;
using Microsoft.AspNetCore.Mvc;

namespace BarCodeAPI.Controllers;

public class ClientsController : ControllerBase {
    private readonly AppDbContext _context;

    public ClientsController(AppDbContext context) {
        _context = context;
    }
}