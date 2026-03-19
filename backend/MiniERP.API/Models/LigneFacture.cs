namespace MiniERP.API.Models;

public class LigneFacture
{
    public int Id { get; set; }
    public int Quantite { get; set; }
    public decimal Remise { get; set; } // en pourcentage, ex: 10 pour 10%

    // Clés étrangères
    public int FactureId { get; set; }
    public int ProduitId { get; set; }

    // Navigation
    public Facture Facture { get; set; } = null!;
    public Produit Produit { get; set; } = null!;
}