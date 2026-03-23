namespace MiniERP.API.DTOs;

// Ce qu'on RENVOIE au front-end (réponse)
public class ClientDto
{
    public int Id { get; set; }
    public string NomEntreprise { get; set; } = string.Empty;
    public string Siret { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public DateTime DateCreation { get; set; }
}

// Ce qu'on REÇOIT du front-end (requête de création ou modification)
public class CreateClientDto
{
    public string NomEntreprise { get; set; } = string.Empty;
    public string Siret { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
}