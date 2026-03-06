using System.ComponentModel.DataAnnotations;

namespace NeoEvaluation.API.Models
{
    public class InscriptionsEntreprise
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string NomEntreprise { get; set; } = string.Empty;

        public string? MatriculeFiscale { get; set; } 

        [Required]
        public string NomResponsable { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string EmailResponsable { get; set; } = string.Empty;

        // 0 = En attente, 1 = Approuvé, 2 = Rejeté
        public int Statut { get; set; } = 0; 

        public DateTime CreeLe { get; set; } = DateTime.UtcNow;
    }
}