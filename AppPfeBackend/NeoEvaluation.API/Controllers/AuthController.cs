using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeoEvaluation.API.Data;
using NeoEvaluation.API.DTOs;
using NeoEvaluation.API.Models;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Google.Apis.Auth;

namespace NeoEvaluation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        // 2. configuration Constructeur
        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }



        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            // 1. Nlawjou 3al utilisateur
            var user = _context.Utilisateurs.FirstOrDefault(u => u.Email == loginDto.Email);

            if (user == null)
            {
                return Unauthorized("Email incorrect.");
            }

            // 2. Nthabtou l'mot de passe
            if (user.MotDePasseHash == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.MotDePasseHash))
            {
                return Unauthorized("Mot de passe incorrect.");
            }

            // 3. Nthabtou kenou Actif
            if (!user.EstActif)
            {
                return BadRequest("Votre compte n'est pas encore activé.");
            }

            // 4. Génération du Token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            
            // Taw _configuration 
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("email", user.Email),
                    new Claim("role", user.Role),
                    new Claim("nom", user.NomComplet)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtString = tokenHandler.WriteToken(token);

            // 5. Nraj3ou l'Resultat
            return Ok(new 
            { 
                Token = jwtString, 
                Nom = user.NomComplet,
                Role = user.Role 
            });
        }

        // --- NOUVEAU : GOOGLE LOGIN ---
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto googleDto)
        {
            try
            {
                // 1. VERIFIER LE TOKEN GOOGLE
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _configuration["GoogleAuthSettings:ClientId"]! }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(googleDto.IdToken, settings);

                // 2. CHERCHER L'UTILISATEUR (Par Email)
                var user = await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Email == payload.Email);

                if (user == null)
                {
                    // SCENARIO : Nouvel Utilisateur (Inscription Auto)
                    // On le crée comme CANDIDAT par défaut
                    user = new Utilisateur
                    {
                        Id = Guid.NewGuid(),
                        NomComplet = payload.Name,
                        Email = payload.Email,
                        Role = "Candidat", 
                        EstActif = true, // Google = Email déjà vérifié
                        NomEntreprise = "Google User",
                        MotDePasseHash = null // Pas de mot de passe local
                    };
                    _context.Utilisateurs.Add(user);
                    await _context.SaveChangesAsync();
                }

                // 3. GENERER NOTRE TOKEN JWT (Comme le Login Classique)
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!);
var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        // Salla7na houni: Zidna 'ToString()' w sta3malna const
                        new Claim("id", user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role),
                        // Thabbet illi NomComplet mahouch null
                        new Claim("nom", user.NomComplet ?? "Utilisateur")
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtString = tokenHandler.WriteToken(token);

                return Ok(new 
                { 
                    Token = jwtString, 
                    Nom = user.NomComplet,
                    Role = user.Role 
                });
            }
            catch (InvalidJwtException ex)
            {
                // AFFICHER L'ERREUR EXACTE
                Console.WriteLine($"ERREUR JWT GOOGLE: {ex.Message}");
                return Unauthorized($"Token Google invalide: {ex.Message}");
            }
            catch (Exception ex)
            {
                // AFFICHER L'ERREUR EXACTE
                Console.WriteLine($"ERREUR GENERALE: {ex.ToString()}"); // ToString ya3ti details akther
                return StatusCode(500, "Erreur interne: " + ex.Message);
            }
        }

    }
}