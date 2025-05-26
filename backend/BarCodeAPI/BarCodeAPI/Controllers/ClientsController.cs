// backend/BarCodeAPI/BarCodeAPI/Controllers/ClientsController.cs
// ReSharper disable all

using BarCode.Domain.DTO;
using BarCode.Infrastructure.Context;
using BarCode.Domain.Models;
using BarCode.Domain.Services; // Adicionar o namespace dos seus serviços de domínio
using BarCodeAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks; // Adicionar para Task

namespace BarCodeAPI.Controllers;

[Route("api/v1")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<ClientsController> _logger;
    private readonly IEmailValidation _emailValidationService; 

    
    public ClientsController(AppDbContext context, ILogger<ClientsController> logger, IEmailValidation emailValidationService)
    {
        _context = context;
        _logger = logger;
        _emailValidationService = emailValidationService; 
    }

    [HttpGet("clients")]
    public async Task<ActionResult<IEnumerable<Client>>> GetAllClients() {
        var clients = await _context.Clients.AsNoTracking().Take(10).ToListAsync();
        if (clients is null) {
            _logger.LogWarning($"Clientes não encontrados.");
            return NotFound($"Clientes não encontrados.");
        }

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
        if (client is null) {
            _logger.LogWarning($"Cliente com id = {id} não encontrado."); 
            return NotFound($"Cliente com id = {id} não encontrado.");
        }
        
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
        if (dto is null) {
            _logger.LogWarning($"Dados inválidos.");            
            return BadRequest($"Dados inválidos.");
        }

        var client = new Client();

        if (_emailValidationService.IsValidEmail(dto.Email) && await _emailValidationService.IsEmailUniqueAsync(dto.Email))
        {
            client.Email = dto.Email;
        }
        else
        {
            _logger.LogWarning($"E-mail inválido.");
            return BadRequest($"E-mail inválido.");
        }

        client.Name = dto.Name;
        client.Phone = dto.Phone;
        client.Address = dto.Address;

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
        if (id != dto.ClientId) {
            _logger.LogWarning($"Dados inválidos.");
            return BadRequest($"Dados inválidos.");
        }
        
        var client = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.ClientId == id);

        if (client is null) {
            _logger.LogWarning($"Cliente não encontrado.");
            return NotFound($"Cliente não encontrado.");
        }


        if (_emailValidationService.IsValidEmail(dto.Email) && await _emailValidationService.IsEmailUniqueAsync(dto.Email))
        {
            client.Email = dto.Email;
        }
        else
        {
            _logger.LogWarning($"E-mail inválido.");
            return BadRequest($"E-mail inválido.");
        }

        client.Name = dto.Name;
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
        if (client is null) {
            _logger.LogWarning($"Cliente com id = {id} não encontrado.");
            return NotFound($"Cliente com id = {id} não encontrado.");
        }
        
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
