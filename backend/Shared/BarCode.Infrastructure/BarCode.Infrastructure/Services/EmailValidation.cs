using System.Text.RegularExpressions;
using BarCode.Domain.Services;
using BarCode.Infrastructure.Context; 
using Microsoft.EntityFrameworkCore;   

namespace BarCode.Infrastructure.Services;

public class EmailValidation : IEmailValidation
{
    private readonly AppDbContext _context; 
    
    public EmailValidation(AppDbContext context)
    {
        _context = context;
    }

    public bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        return regex.IsMatch(email);
    }

    // Esta validação de domínio é muito específica.
    // Avalie se é um requisito real ou se precisa ser mais genérica/removida.
    public bool IsValidDomain(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        // Exemplo: verifica se o domínio é "example.com"
        return email.EndsWith("@example.com", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            // Considerar lançar ArgumentNullException se o e-mail não puder ser nulo ou vazio.
            return false;
        }
        // Verifica se já existe algum cliente com o mesmo e-mail.
        // Certifique-se que sua entidade Client (BarCode.Domain.Models.Client) possui a propriedade Email.
        var existingClient = await _context.Clients
                                           .AsNoTracking() // Boa prática para consultas somente leitura.
                                           .FirstOrDefaultAsync(c => c.Email == email);
        return existingClient == null; // Retorna true se o e-mail for único (nenhum cliente existente encontrado).
    }
}