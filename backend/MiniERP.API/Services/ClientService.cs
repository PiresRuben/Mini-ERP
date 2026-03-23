using Microsoft.EntityFrameworkCore;
using MiniERP.API.Data;
using MiniERP.API.Models;

namespace MiniERP.API.Services;

// La classe qui implémente le contrat : elle dit COMMENT faire
public class ClientService : IClientService
{
    private readonly AppDbContext _context;

    // Le DbContext est injecté automatiquement par .NET (injection de dépendances)
    public ClientService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Client>> GetAllAsync()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task<Client?> GetByIdAsync(int id)
    {
        return await _context.Clients.FindAsync(id);
    }

    public async Task<Client> CreateAsync(Client client)
    {
        client.DateCreation = DateTime.UtcNow;
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return client;
    }

    public async Task<Client?> UpdateAsync(int id, Client client)
    {
        var existing = await _context.Clients.FindAsync(id);
        if (existing == null) return null;

        existing.NomEntreprise = client.NomEntreprise;
        existing.Siret = client.Siret;
        existing.Adresse = client.Adresse;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null) return false;

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return true;
    }
}