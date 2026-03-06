using System.ComponentModel.DataAnnotations;

namespace NeoEvaluation.API.Models
{
    public class Utilisateur
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string NomComplet { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? MotDePasseHash { get; set; } // Vide fil loul

        [Required]
        public string Role { get; set; } = "ADMIN_ENTREPRISE";

        public bool EstActif { get; set; } = false;

        // Houni nkhabbiw ism charika direct pour Sprint 1
        public string? NomEntreprise { get; set; } 
    }
}