using Microsoft.EntityFrameworkCore;
using MiniERP.API.Models;

namespace MiniERP.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Chaque DbSet = une table dans la BDD
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Produit> Produits => Set<Produit>();
    public DbSet<Facture> Factures => Set<Facture>();
    public DbSet<LigneFacture> LignesFacture => Set<LigneFacture>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Le numéro de facture doit être unique
        modelBuilder.Entity<Facture>()
            .HasIndex(f => f.NumeroFacture)
            .IsUnique();

        // Précision pour les valeurs monétaires
        modelBuilder.Entity<Produit>()
            .Property(p => p.PrixUnitaireHT)
            .HasPrecision(18, 2);  // 18 chiffres max, 2 après la virgule

        modelBuilder.Entity<LigneFacture>()
            .Property(l => l.Remise)
            .HasPrecision(5, 2);   // ex: 100.00% max
            
        modelBuilder.Entity<Client>().HasData(
            new Client
            {
                Id = 1,
                NomEntreprise = "Gaming Campus",
                Siret = "12345678900001",
                Adresse = "85 Rue du Docteur Bauer, 93400 Saint-Ouen",
                DateCreation = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc)
            },
            new Client
            {
                Id = 2,
                NomEntreprise = "Proginov",
                Siret = "33298227200034",
                Adresse = "6 Rue de la Rabotière, 44800 Saint-Herblain",
                DateCreation = new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Client
            {
                Id = 3,
                NomEntreprise = "Studio Pixel",
                Siret = "98765432100012",
                Adresse = "12 Rue de la Paix, 75002 Paris",
                DateCreation = new DateTime(2024, 6, 10, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        modelBuilder.Entity<Produit>().HasData(
            new Produit
            {
                Id = 1,
                Reference = "LIC-UNITY",
                Designation = "Licence Unity Pro (1 an)",
                PrixUnitaireHT = 1800.00m
            },
            new Produit
            {
                Id = 2,
                Reference = "FORM-CSHARP",
                Designation = "Formation C# Avancé (5 jours)",
                PrixUnitaireHT = 3500.00m
            },
            new Produit
            {
                Id = 3,
                Reference = "DEV-WEB",
                Designation = "Développement site web vitrine",
                PrixUnitaireHT = 2500.00m
            },
            new Produit
            {
                Id = 4,
                Reference = "MAINT-ANNUELLE",
                Designation = "Contrat de maintenance annuel",
                PrixUnitaireHT = 1200.00m
            },
            new Produit
            {
                Id = 5,
                Reference = "HOSTING-PRO",
                Designation = "Hébergement Cloud Pro (1 an)",
                PrixUnitaireHT = 600.00m
            }
        );
    }
    
}