using System.ComponentModel.DataAnnotations;

namespace NeoEvaluation.API.DTOs
{
    public class RegisterCompanyDto
    {
        [Required]
        public string NomEntreprise { get; set; } = string.Empty;

        [Required]
        public string NomResponsable { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string EmailResponsable { get; set; } = string.Empty;

        public string? MatriculeFiscale { get; set; }
    }
}