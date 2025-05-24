using System.Text.RegularExpressions;
using BarCode.Domain.Services;

namespace BarCode.Infrastructure.Services;

public class EmailValidation : IEmailValidation
{
    // Validação de e-mail usando expressão regular
    public bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        // Regex simples para validação de e-mail
        var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        return regex.IsMatch(email);
    }

    public bool IsValidDomain(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        // Exemplo: verifica se o domínio é "example.com"
        return email.EndsWith("@example.com", StringComparison.OrdinalIgnoreCase);
    }

    public Task<bool> IsEmailUniqueAsync(string email)
    {
        // Implementação fictícia, deve ser substituída por lógica real de verificação
        return Task.FromResult(true);
    }
}
