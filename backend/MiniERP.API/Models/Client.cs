namespace MiniERP.API.Models;

public class Client
{
    public int Id { get; set; }
    public string NomEntreprise { get; set; } = string.Empty;
    public string Siret { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;
    
    public List<Facture> Factures { get; set; } = new();
}