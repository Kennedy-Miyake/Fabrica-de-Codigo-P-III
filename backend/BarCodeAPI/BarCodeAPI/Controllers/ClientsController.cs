// backend/BarCodeAPI/BarCodeAPI/Controllers/ClientsController.cs
// ReSharper disable all
using BarCode.Infrastructure.Context;
using BarCode.Domain.Models;
using BarCode.Domain.Services; // Adicionar o namespace dos seus serviços de domínio
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks; // Adicionar para Task



namespace BarCodeAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IEmailValidation _emailValidationService; 

    
    public ClientsController(AppDbContext context, IEmailValidation emailValidationService)
    {
        _context = context;
        _emailValidationService = emailValidationService; 
    }

    [HttpGet]
    public ActionResult<IEnumerable<Client>> Get()
    {
        var clients = _context.Clients.AsNoTracking().Take(10).ToList();
        if (clients is null || !clients.Any()) 
            return NotFound("Clientes não encontrados...");
        return clients;
    }

    [HttpGet("{id:int}", Name = "GetClient")]
    public ActionResult<Client> Get(int id)
    {
        var client = _context.Clients.AsNoTracking().FirstOrDefault(p => p.ClientId == id);
        if (client is null)
            return NotFound("Cliente não encontrado.");
        return client;
    }

    [HttpPost]
    public async Task<ActionResult> Post(Client client) 
    {
        if (client is null || client.Email is null) // client.Email não pode ser nulo para validação
            return BadRequest("Dados do cliente inválidos.");

        // Validar o formato do e-mail
        if (!_emailValidationService.IsValidEmail(client.Email))
        {
            return BadRequest("Formato de e-mail inválido.");
        }
        
        if (!await _emailValidationService.IsEmailUniqueAsync(client.Email))
        {
            return BadRequest("Este e-mail já está cadastrado.");
        }

        _context.Clients.Add(client);
        await _context.SaveChangesAsync(); // Usar await para operações assíncronas

        return new CreatedAtRouteResult("GetClient", new { id = client.ClientId }, client);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, Client client) // Alterado para async Task<ActionResult>
    {
        if (id != client.ClientId)
            return BadRequest("ID do cliente inconsistente.");

        if (client.Email is not null) // Só validar se o email for fornecido e modificado
        {
             if (!_emailValidationService.IsValidEmail(client.Email))
            {
                return BadRequest("Formato de e-mail inválido.");
            }
        }


        _context.Entry(client).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync(); // Usar await
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ClientExists(id))
            {
                return NotFound("Cliente não encontrado.");
            }
            else
            {
                throw;
            }
        }
        return Ok(client);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id) // Alterado para async Task<ActionResult>
    {
        var client = await _context.Clients.FindAsync(id); // Usar FindAsync
        if (client is null)
            return NotFound("Cliente não encontrado...");

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync(); // Usar await

        return Ok(client);
    }

    private bool ClientExists(int id)
    {
        return _context.Clients.Any(e => e.ClientId == id);
    }
}
