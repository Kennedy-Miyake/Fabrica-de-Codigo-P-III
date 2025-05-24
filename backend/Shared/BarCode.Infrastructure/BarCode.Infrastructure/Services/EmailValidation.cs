namespace BarCode.Infrastructure.Services;

public class EmailValidation
{
// Ele herda diretamente a logica do contrato 

//definir a logica por traz do metodo definido lá no contrato

    public bool IsValid(string email)
    {
        // Implementar a lógica de validação de e-mail
        // Exemplo: verificar se contém "@" e "."
        return !string.IsNullOrEmpty(email) && email.Contains("@") && email.Contains(".");
    }
    public bool IsValidDomain(string email)
    {
        // Implementar a lógica de validação de domínio
        // Exemplo simples: verificar se o domínio é "example.com"
        return email.EndsWith("@example.com");
    }
    
}