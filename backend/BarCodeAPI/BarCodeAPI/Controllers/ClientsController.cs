// ReSharper disable all
using BarCodeAPI.Context;
using BarCodeAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BarCodeAPI.Controllers;

public class ClientsController : ControllerBase {
    private readonly AppDbContext _context;

    public ClientsController(AppDbContext context) {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Client>> Get() {
        var clients = _context.Clients.ToList();
        if (clients is null)
            return NotFound("Clientes não encontrados...");
        return clients;
    }
}