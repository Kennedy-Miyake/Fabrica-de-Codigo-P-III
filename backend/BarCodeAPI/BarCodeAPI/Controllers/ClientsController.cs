// ReSharper disable all

using BarCode.Domain.DTO;
using BarCode.Infrastructure.Context;
using BarCode.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarCodeAPI.Controllers;

[Route("api/v1")]
[ApiController]
public class ClientsController : ControllerBase {
    private readonly AppDbContext _context;

    public ClientsController(AppDbContext context) {
        _context = context;
    }

    [HttpGet("clients")]
    public async Task<ActionResult<IEnumerable<Client>>> GetAllClients() {
        var clients = await _context.Clients.AsNoTracking().Take(10).ToListAsync();
        if (clients is null)
            return NotFound("Clientes não encontrados...");

        var clientDTOs = clients.Select(client => new ClientDTO(
                                                                client.ClientId,
                                                                client.Name,
                                                                client.Email,
                                                                client.Phone,
                                                                client.Address)).ToList();

        return Ok(clientDTOs);
    }

    [HttpGet("client/{id:int}", Name = "GetClient")]
    public async Task<ActionResult<Client>> GetClient(int id) {
        var client = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(p => p.ClientId == id);
        if (client is null)
            return NotFound("Cliente não encontrado.");
        
        var clientDTO = new ClientDTO(
                                      client.ClientId,
                                      client.Name,
                                      client.Email,
                                      client.Phone,
                                      client.Address);

        return Ok(clientDTO);
    }

    [HttpPost("client")]
    public ActionResult Post(Client client) {
        if (client is null)
            return BadRequest();

        _context.Clients.Add(client);
        _context.SaveChanges();

        return new CreatedAtRouteResult("GetClient", new { id = client.ClientId }, client);
    }

    [HttpPut("client/{id:int}")]
    public ActionResult Put(int id, Client client) {
        if (id != client.ClientId)
            return BadRequest();

        _context.Entry(client).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(client);
    }

    [HttpDelete("client/{id:int}")]
    public ActionResult Delete(int id) {
        var client = _context.Clients.FirstOrDefault(p => p.ClientId == id);
        if (client is null)
            return NotFound("Cliente não encontrado...");

        _context.Clients.Remove(client);
        _context.SaveChanges();
        
        return Ok(client);
    }
}