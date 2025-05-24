namespace BarCode.Domain.Services;

public interface IEmailValidation
{
    bool IsValidEmail(string email);
    bool IsValidDomain(string email);
    Task<bool> IsEmailUniqueAsync(string email);
}
