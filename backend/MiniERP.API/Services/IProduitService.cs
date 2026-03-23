using MiniERP.API.Models;

namespace MiniERP.API.Services;

public interface IProduitService
{
    Task<List<Produit>> GetAllAsync();
}