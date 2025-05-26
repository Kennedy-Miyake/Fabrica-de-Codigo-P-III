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
    public async Task<ActionResult> CreateClient(ClientDTO? dto) {
        if (dto is null)
            return BadRequest();

        var client = new Client {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        
        var clientDTO = new ClientDTO(
                                  client.ClientId,
                                  client.Name,
                                  client.Email,
                                  client.Phone,
                                  client.Address);

        return new CreatedAtRouteResult(nameof(GetClient), new { id = client.ClientId }, clientDTO);
    }

    [HttpPut("client/{id:int}")]
    public async Task<ActionResult> Put(int id, ClientDTO dto) {
        if (id != dto.ClientId)
            return BadRequest();
        
        var client = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.ClientId == id);
        
        if (client is null)
            return NotFound("Cliente não encontrado...");

        client.Name = dto.Name;
        client.Email = dto.Email;
        client.Phone = dto.Phone;
        client.Address = dto.Address;
        
        _context.Entry(client).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        var clientDTO = new ClientDTO(
                                  client.ClientId,
                                  client.Name,
                                  client.Email,
                                  client.Phone,
                                  client.Address);
        
        return Ok(clientDTO);
    }

    [HttpDelete("client/{id:int}")]
    public async Task<ActionResult> Delete(int id) {
        var client = await _context.Clients.FirstOrDefaultAsync(p => p.ClientId == id);
        if (client is null)
            return NotFound("Cliente não encontrado...");
        
        var clientDTO = new ClientDTO(
                                  client.ClientId,
                                  client.Name,
                                  client.Email,
                                  client.Phone,
                                  client.Address);

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        
        return Ok(clientDTO);
    }
}