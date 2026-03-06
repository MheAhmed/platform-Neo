using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeoEvaluation.API.Data;
using NeoEvaluation.API.DTOs;
using NeoEvaluation.API.Models;
using NeoEvaluation.API.Services;

namespace NeoEvaluation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public AdminController(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET /api/admin/pending
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<InscriptionsEntreprise>>> GetPending()
        {
            var pending = await _context.InscriptionsEntreprises
                .Where(i => i.Statut == 0)
                .ToListAsync();
            return Ok(pending);
        }

        // POST /api/admin/approve/{id}
        [HttpPost("approve/{id}")]
        public async Task<IActionResult> Approve(Guid id)
        {
            var registration = await _context.InscriptionsEntreprises.FindAsync(id);
            if (registration == null) return NotFound("Demande introuvable.");

            registration.Statut = 1; // Approuvé

            // 1. Générer Token d'activation
            var activationToken = new TokensActivation
            {
                IdInscription = registration.Id,
                Token = Guid.NewGuid(),
                DateExpiration = DateTime.UtcNow.AddHours(24),
                Utilise = false
            };

            _context.TokensActivation.Add(activationToken);
            await _context.SaveChangesAsync();

            // 2. Envoyer Email
            var activationLink = $"http://localhost:5173/auth/definir-mot-de-passe?token={activationToken.Token}";
            var body = $@"
                <div style='font-family: sans-serif; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #eee; border-radius: 10px;'>
                    <h2 style='color: #059669;'>Bienvenue chez AppTest</h2>
                    <p>Votre demande pour l'entreprise <b>{registration.NomEntreprise}</b> a été acceptée.</p>
                    <p>Veuillez cliquer sur le bouton ci-dessous pour créer votre mot de passe et finaliser votre compte :</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{activationLink}' style='background-color: #111827; color: white; padding: 12px 24px; text-decoration: none; border-radius: 8px; font-weight: bold; font-size: 14px;'>ACTIVER MON COMPTE</a>
                    </div>
                    <p style='color: #6b7280; font-size: 12px;'>Si le bouton ne fonctionne pas, copiez ce lien : <br> {activationLink}</p>
                </div>";

            try 
            {
                await _emailService.SendEmailAsync(registration.EmailResponsable, "Activation de votre compte AppTest", body);
                return Ok(new { Message = "Demande approuvée et email envoyé avec succès." });
            }
            catch (Exception ex)
            {
                // On retourne quand même un succès partiel mais avec l'erreur pour le debug
                return Ok(new { 
                    Message = "Demande approuvée, mais l'envoi de l'email a échoué.",
                    TechnicalError = ex.Message,
                    FallbackLink = activationLink // Lien de secours pour le SuperAdmin
                });
            }
        }

        // POST /api/admin/reject/{id}
        [HttpPost("reject/{id}")]
        public async Task<IActionResult> Reject(Guid id, [FromBody] ApproveRequestDto dto)
        {
            var registration = await _context.InscriptionsEntreprises.FindAsync(id);
            if (registration == null) return NotFound("Demande introuvable.");

            registration.Statut = 2; // Rejeté
            await _context.SaveChangesAsync();

            // Envoyer Email de refus
            var body = $@"
                <h2>Mise à jour de votre demande EvaluaTech</h2>
                <p>Votre demande pour l'entreprise <b>{registration.NomEntreprise}</b> a été refusée.</p>
                {(string.IsNullOrEmpty(dto.Reason) ? "" : $"<p><b>Raison :</b> {dto.Reason}</p>")}
                <p>Cordialement,<br>L'équipe EvaluaTech</p>";

            await _emailService.SendEmailAsync(registration.EmailResponsable, "Statut de votre demande EvaluaTech", body);

            return Ok(new { Message = "Demande refusée." });
        }
    }
}
