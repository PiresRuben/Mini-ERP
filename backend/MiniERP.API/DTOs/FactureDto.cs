namespace MiniERP.API.DTOs;

// === FACTURE ===

// Ce qu'on RENVOIE (avec les lignes et les totaux calculés)
public class FactureDto
{
    public int Id { get; set; }
    public string NumeroFacture { get; set; } = string.Empty;
    public DateTime DateEmission { get; set; }
    public string Statut { get; set; } = string.Empty;
    public int ClientId { get; set; }
    public string NomClient { get; set; } = string.Empty;

    public List<LigneFactureDto> Lignes { get; set; } = new();

    // Totaux calculés automatiquement
    public decimal TotalHT { get; set; }
    public decimal MontantTVA { get; set; }
    public decimal TotalTTC { get; set; }
}

// Ce qu'on REÇOIT pour créer une facture
public class CreateFactureDto
{
    public int ClientId { get; set; }
}

// Ce qu'on REÇOIT pour changer le statut
public class UpdateStatutDto
{
    public string Statut { get; set; } = string.Empty;
}

// === LIGNES DE FACTURE ===

// Ce qu'on RENVOIE (avec le nom du produit et les calculs)
public class LigneFactureDto
{
    public int Id { get; set; }
    public int ProduitId { get; set; }
    public string DesignationProduit { get; set; } = string.Empty;
    public string ReferenceProduit { get; set; } = string.Empty;
    public int Quantite { get; set; }
    public decimal PrixUnitaireHT { get; set; }
    public decimal Remise { get; set; }
    public decimal TotalLigneHT { get; set; }
}

// Ce qu'on REÇOIT pour ajouter/modifier une ligne
public class CreateLigneFactureDto
{
    public int ProduitId { get; set; }
    public int Quantite { get; set; }
    public decimal Remise { get; set; }  // en pourcentage : 10 = 10%
}