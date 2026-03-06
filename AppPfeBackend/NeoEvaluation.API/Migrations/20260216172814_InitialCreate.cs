using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeoEvaluation.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NomEntreprise = table.Column<string>(type: "text", nullable: false),
                    MatriculeFiscale = table.Column<string>(type: "text", nullable: true),
                    NomResponsable = table.Column<string>(type: "text", nullable: false),
                    EmailResponsable = table.Column<string>(type: "text", nullable: false),
                    Statut = table.Column<int>(type: "integer", nullable: false),
                    DateDemande = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TokensActivation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmailUtilisateur = table.Column<string>(type: "text", nullable: false),
                    Token = table.Column<Guid>(type: "uuid", nullable: false),
                    DateExpiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EstUtilise = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokensActivation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateurs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NomComplet = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    MotDePasseHash = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: false),
                    EstActif = table.Column<bool>(type: "boolean", nullable: false),
                    NomEntreprise = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateurs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inscriptions");

            migrationBuilder.DropTable(
                name: "TokensActivation");

            migrationBuilder.DropTable(
                name: "Utilisateurs");
        }
    }
}
