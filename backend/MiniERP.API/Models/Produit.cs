namespace MiniERP.API.Models;

public class Produit
{
    public int Id { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;

    // IMPORTANT : decimal pour l'argent, JAMAIS float ou double
    public decimal PrixUnitaireHT { get; set; }

    // Navigation : un produit peut être dans plusieurs lignes
    public List<LigneFacture> LignesFacture { get; set; } = new();
}