using System.ComponentModel.DataAnnotations;

namespace NeoEvaluation.API.DTOs
{
    public class CompleteActivationDto
    {
        [Required]
        public Guid Token { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}