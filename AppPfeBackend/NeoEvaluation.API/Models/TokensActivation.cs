using System.ComponentModel.DataAnnotations;

namespace NeoEvaluation.API.Models
{
    public class TokensActivation
    {
        [Key]
        public Guid Token { get; set; } = Guid.NewGuid();

        [Required]
        public Guid IdInscription { get; set; } 

        public DateTime DateExpiration { get; set; }

        public bool Utilise { get; set; } = false;
    }
}