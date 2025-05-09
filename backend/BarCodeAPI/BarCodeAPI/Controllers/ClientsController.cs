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

    [HttpGet("{id:int}", Name = "GetClient")]
    public ActionResult<Client> Get(int id) {
        var client = _context.Clients.FirstOrDefault(p => p.ClientId == id);
        if (client is null)
            return NotFound("Cliente não encontrado.");
        return client;
    }

    [HttpPost]
    public ActionResult Post(Client client) {
        if (client is null)
            return BadRequest();

        _context.Clients.Add(client);
        _context.SaveChanges();

        return new CreatedAtRouteResult("GetClient", new { id = client.ClientId }, client);
    }
}