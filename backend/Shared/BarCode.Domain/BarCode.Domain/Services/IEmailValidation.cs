namespace BarCode.Domain.Services;

public interface IEmailValidation
{
    //Define apenas o contrato 
    //metodos que irei utilizar
    public interface IEmailValidation
    {
        bool IsValidEmail(string email);
        bool IsValidDomain(string email);
        Task<bool> IsEmailUniqueAsync(string email);
    }
    
}