namespace MiniERP.API.Models;

public class Facture
{
    public int Id { get; set; }
    public string NumeroFacture { get; set; } = string.Empty;
    public DateTime DateEmission { get; set; } = DateTime.UtcNow;
    public string Statut { get; set; } = "Brouillon";

    // Clé étrangère vers Client
    public int ClientId { get; set; }

    // Navigation
    public Client Client { get; set; } = null!;
    public List<LigneFacture> Lignes { get; set; } = new();
}