namespace MiniERP.API.DTOs;

public class ProduitDto
{
    public int Id { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public decimal PrixUnitaireHT { get; set; }
}