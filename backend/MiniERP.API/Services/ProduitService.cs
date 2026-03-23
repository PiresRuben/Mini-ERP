using Microsoft.EntityFrameworkCore;
using MiniERP.API.Data;
using MiniERP.API.Models;

namespace MiniERP.API.Services;

public class ProduitService : IProduitService
{
    private readonly AppDbContext _context;

    public ProduitService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Produit>> GetAllAsync()
    {
        return await _context.Produits.ToListAsync();
    }
}