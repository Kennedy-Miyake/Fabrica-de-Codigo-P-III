// ReSharper disable all
using BarCode.Infrastructure.Context;
using BarCode.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarCodeAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class ClientsController : ControllerBase {
    private readonly AppDbContext _context;

    public ClientsController(AppDbContext context) {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Client>> Get() {
        var clients = _context.Clients.AsNoTracking().Take(10).ToList();
        if (clients is null)
            return NotFound("Clientes não encontrados...");
        return clients;
    }

    [HttpGet("{id:int}", Name = "GetClient")]
    public ActionResult<Client> Get(int id) {
        var client = _context.Clients.AsNoTracking().FirstOrDefault(p => p.ClientId == id);
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

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Client client) {
        if (id != client.ClientId)
            return BadRequest();

        _context.Entry(client).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(client);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id) {
        var client = _context.Clients.FirstOrDefault(p => p.ClientId == id);
        if (client is null)
            return NotFound("Cliente não encontrado...");

        _context.Clients.Remove(client);
        _context.SaveChanges();
        
        return Ok(client);
    }
}