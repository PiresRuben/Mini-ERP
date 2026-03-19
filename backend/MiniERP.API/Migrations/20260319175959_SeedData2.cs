using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiniERP.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Adresse", "DateCreation", "NomEntreprise", "Siret" },
                values: new object[,]
                {
                    { 1, "85 Rue du Docteur Bauer, 93400 Saint-Ouen", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Gaming Campus", "12345678900001" },
                    { 2, "6 Rue de la Rabotière, 44800 Saint-Herblain", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Proginov", "33298227200034" },
                    { 3, "12 Rue de la Paix, 75002 Paris", new DateTime(2024, 6, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Studio Pixel", "98765432100012" }
                });

            migrationBuilder.InsertData(
                table: "Produits",
                columns: new[] { "Id", "Designation", "PrixUnitaireHT", "Reference" },
                values: new object[,]
                {
                    { 1, "Licence Unity Pro (1 an)", 1800.00m, "LIC-UNITY" },
                    { 2, "Formation C# Avancé (5 jours)", 3500.00m, "FORM-CSHARP" },
                    { 3, "Développement site web vitrine", 2500.00m, "DEV-WEB" },
                    { 4, "Contrat de maintenance annuel", 1200.00m, "MAINT-ANNUELLE" },
                    { 5, "Hébergement Cloud Pro (1 an)", 600.00m, "HOSTING-PRO" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Produits",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Produits",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Produits",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Produits",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Produits",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
