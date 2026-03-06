using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeoEvaluation.API.Data;
using NeoEvaluation.API.DTOs;
using NeoEvaluation.API.Models;
using System.Security.Cryptography;
using System.Text;

namespace NeoEvaluation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ActivationController(AppDbContext context)
        {
            _context = context;
        }

        // POST /api/activation/complete
        [HttpPost("complete")]
        public async Task<IActionResult> Complete([FromBody] CompleteActivationDto dto)
        {
            var token = await _context.TokensActivation.FirstOrDefaultAsync(t => t.Token == dto.Token);

            if (token == null) return BadRequest("Token invalide.");
            if (token.Utilise) return BadRequest("Token déjà utilisé.");
            if (token.DateExpiration < DateTime.UtcNow) return BadRequest("Token expiré.");

            var registration = await _context.InscriptionsEntreprises.FindAsync(token.IdInscription);
            if (registration == null) return BadRequest("Données d'inscription introuvables.");

            // 1. Créer l'utilisateur
            var user = new Utilisateur
            {
                Id = Guid.NewGuid(),
                NomComplet = registration.NomResponsable,
                Email = registration.EmailResponsable,
                NomEntreprise = registration.NomEntreprise,
                Role = "AdminEntreprise",
                EstActif = true,
                MotDePasseHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Utilisateurs.Add(user);

            // 2. Marquer le token comme utilisé
            token.Utilise = true;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Compte activé avec succès. Vous pouvez maintenant vous connecter." });
        }
    }
}
