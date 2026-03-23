using Microsoft.EntityFrameworkCore;
using MiniERP.API.Data;
using MiniERP.API.DTOs;
using MiniERP.API.Models;

namespace MiniERP.API.Services;

public class FactureService : IFactureService
{
    private readonly AppDbContext _context;
    private const decimal TAUX_TVA = 0.20m;  // 20% de TVA

    public FactureService(AppDbContext context)
    {
        _context = context;
    }

    // Convertit une Facture en FactureDto avec tous les calculs
    private FactureDto ToDto(Facture facture)
    {
        var lignesDto = facture.Lignes.Select(l => new LigneFactureDto
        {
            Id = l.Id,
            ProduitId = l.ProduitId,
            DesignationProduit = l.Produit?.Designation ?? "",
            ReferenceProduit = l.Produit?.Reference ?? "",
            Quantite = l.Quantite,
            PrixUnitaireHT = l.Produit?.PrixUnitaireHT ?? 0,
            Remise = l.Remise,
            // Calcul par ligne : Quantité × Prix HT × (1 - Remise/100)
            TotalLigneHT = l.Quantite * (l.Produit?.PrixUnitaireHT ?? 0)
                           * (1 - l.Remise / 100m)
        }).ToList();

        var totalHT = lignesDto.Sum(l => l.TotalLigneHT);
        var montantTVA = Math.Round(totalHT * TAUX_TVA, 2);
        var totalTTC = totalHT + montantTVA;

        return new FactureDto
        {
            Id = facture.Id,
            NumeroFacture = facture.NumeroFacture,
            DateEmission = facture.DateEmission,
            Statut = facture.Statut,
            ClientId = facture.ClientId,
            NomClient = facture.Client?.NomEntreprise ?? "",
            Lignes = lignesDto,
            TotalHT = Math.Round(totalHT, 2),
            MontantTVA = montantTVA,
            TotalTTC = Math.Round(totalTTC, 2)
        };
    }

    // Charge une facture avec toutes ses relations
    private IQueryable<Facture> GetFullQuery()
    {
        return _context.Factures
            .Include(f => f.Client)
            .Include(f => f.Lignes)
                .ThenInclude(l => l.Produit);
    }

    // Génère un numéro de facture unique : FAC-2026-0001
    private async Task<string> GenerateNumeroAsync()
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"FAC-{year}-";

        var lastNumero = await _context.Factures
            .Where(f => f.NumeroFacture.StartsWith(prefix))
            .OrderByDescending(f => f.NumeroFacture)
            .Select(f => f.NumeroFacture)
            .FirstOrDefaultAsync();

        int nextNumber = 1;
        if (lastNumero != null)
        {
            var parts = lastNumero.Split('-');
            if (parts.Length == 3 && int.TryParse(parts[2], out int current))
            {
                nextNumber = current + 1;
            }
        }

        return $"{prefix}{nextNumber:D4}";
    }

    public async Task<List<FactureDto>> GetAllAsync()
    {
        var factures = await GetFullQuery().ToListAsync();
        return factures.Select(ToDto).ToList();
    }

    public async Task<FactureDto?> GetByIdAsync(int id)
    {
        var facture = await GetFullQuery()
            .FirstOrDefaultAsync(f => f.Id == id);

        return facture == null ? null : ToDto(facture);
    }

    public async Task<FactureDto> CreateAsync(CreateFactureDto dto)
    {
        // Vérifier que le client existe
        var client = await _context.Clients.FindAsync(dto.ClientId);
        if (client == null)
            throw new ArgumentException("Client introuvable");

        var facture = new Facture
        {
            NumeroFacture = await GenerateNumeroAsync(),
            DateEmission = DateTime.UtcNow,
            Statut = "Brouillon",
            ClientId = dto.ClientId
        };

        _context.Factures.Add(facture);
        await _context.SaveChangesAsync();

        // Recharger avec les relations
        var result = await GetFullQuery()
            .FirstAsync(f => f.Id == facture.Id);

        return ToDto(result);
    }

    public async Task<FactureDto?> UpdateStatutAsync(int id, UpdateStatutDto dto)
    {
        var facture = await GetFullQuery()
            .FirstOrDefaultAsync(f => f.Id == id);

        if (facture == null) return null;

        facture.Statut = dto.Statut;
        await _context.SaveChangesAsync();

        return ToDto(facture);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var facture = await _context.Factures
            .Include(f => f.Lignes)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (facture == null) return false;

        // Supprimer les lignes d'abord, puis la facture
        _context.LignesFacture.RemoveRange(facture.Lignes);
        _context.Factures.Remove(facture);
        await _context.SaveChangesAsync();
        return true;
    }

    // === GESTION DES LIGNES ===

    public async Task<FactureDto?> AddLigneAsync(int factureId, CreateLigneFactureDto dto)
    {
        var facture = await GetFullQuery()
            .FirstOrDefaultAsync(f => f.Id == factureId);

        if (facture == null) return null;

        // Vérifier que le produit existe
        var produit = await _context.Produits.FindAsync(dto.ProduitId);
        if (produit == null)
            throw new ArgumentException("Produit introuvable");

        var ligne = new LigneFacture
        {
            FactureId = factureId,
            ProduitId = dto.ProduitId,
            Quantite = dto.Quantite,
            Remise = dto.Remise
        };

        _context.LignesFacture.Add(ligne);
        await _context.SaveChangesAsync();

        // Recharger pour avoir les totaux à jour
        var result = await GetFullQuery()
            .FirstAsync(f => f.Id == factureId);

        return ToDto(result);
    }

    public async Task<FactureDto?> UpdateLigneAsync(int factureId, int ligneId, CreateLigneFactureDto dto)
    {
        var ligne = await _context.LignesFacture
            .FirstOrDefaultAsync(l => l.Id == ligneId && l.FactureId == factureId);

        if (ligne == null) return null;

        ligne.ProduitId = dto.ProduitId;
        ligne.Quantite = dto.Quantite;
        ligne.Remise = dto.Remise;

        await _context.SaveChangesAsync();

        var result = await GetFullQuery()
            .FirstAsync(f => f.Id == factureId);

        return ToDto(result);
    }

    public async Task<FactureDto?> DeleteLigneAsync(int factureId, int ligneId)
    {
        var ligne = await _context.LignesFacture
            .FirstOrDefaultAsync(l => l.Id == ligneId && l.FactureId == factureId);

        if (ligne == null) return null;

        _context.LignesFacture.Remove(ligne);
        await _context.SaveChangesAsync();

        var result = await GetFullQuery()
            .FirstAsync(f => f.Id == factureId);

        return ToDto(result);
    }
}