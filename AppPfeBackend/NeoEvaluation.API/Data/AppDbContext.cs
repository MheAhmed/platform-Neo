using Microsoft.EntityFrameworkCore;
using NeoEvaluation.API.Models;

namespace NeoEvaluation.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Houni n9ouloulou chnouwa l'tables illi 3andna
        public DbSet<InscriptionsEntreprise> InscriptionsEntreprises { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<TokensActivation> TokensActivation { get; set; }
    }
}