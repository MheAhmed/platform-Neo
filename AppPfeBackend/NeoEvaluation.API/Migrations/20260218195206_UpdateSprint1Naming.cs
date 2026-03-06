using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeoEvaluation.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSprint1Naming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TokensActivation",
                table: "TokensActivation");

            migrationBuilder.DropColumn(
                name: "EmailUtilisateur",
                table: "TokensActivation");

            migrationBuilder.RenameColumn(
                name: "EstUtilise",
                table: "TokensActivation",
                newName: "Utilise");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TokensActivation",
                newName: "IdInscription");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TokensActivation",
                table: "TokensActivation",
                column: "Token");

            migrationBuilder.CreateTable(
                name: "InscriptionsEntreprises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NomEntreprise = table.Column<string>(type: "text", nullable: false),
                    MatriculeFiscale = table.Column<string>(type: "text", nullable: true),
                    NomResponsable = table.Column<string>(type: "text", nullable: false),
                    EmailResponsable = table.Column<string>(type: "text", nullable: false),
                    Statut = table.Column<int>(type: "integer", nullable: false),
                    CreeLe = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InscriptionsEntreprises", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InscriptionsEntreprises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TokensActivation",
                table: "TokensActivation");

            migrationBuilder.RenameColumn(
                name: "Utilise",
                table: "TokensActivation",
                newName: "EstUtilise");

            migrationBuilder.RenameColumn(
                name: "IdInscription",
                table: "TokensActivation",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "EmailUtilisateur",
                table: "TokensActivation",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TokensActivation",
                table: "TokensActivation",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Inscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DateDemande = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EmailResponsable = table.Column<string>(type: "text", nullable: false),
                    MatriculeFiscale = table.Column<string>(type: "text", nullable: true),
                    NomEntreprise = table.Column<string>(type: "text", nullable: false),
                    NomResponsable = table.Column<string>(type: "text", nullable: false),
                    Statut = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscriptions", x => x.Id);
                });
        }
    }
}
