// Localização Sugerida: backend/BarCodeAPI/BarCodeAPI/Dtos/ClientCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace BarCodeAPI.Dtos
{
    public class ClientCreateDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(64, ErrorMessage = "O nome deve ter no máximo 64 caracteres.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [StringLength(10000, ErrorMessage = "O e-mail é muito longo.")] // Considere se este tamanho é realmente necessário.
        // Adicionamos [EmailAddress] para uma validação de formato básica pelo ASP.NET Core.
        // Sua validação customizada no serviço IEmailValidation será mais específica.
        [EmailAddress(ErrorMessage = "O formato do e-mail é inválido.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [StringLength(13, ErrorMessage = "O telefone deve ter no máximo 13 caracteres.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(128, ErrorMessage = "O endereço deve ter no máximo 128 caracteres.")]
        public string? Address { get; set; }

        // Se você tiver um campo de senha, adicione-o aqui também com as validações apropriadas.
        // Exemplo:
        // [Required(ErrorMessage = "A senha é obrigatória.")]
        // [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
        // public string? Password { get; set; }
    }
}