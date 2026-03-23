using MiniERP.API.Models;

namespace MiniERP.API.Services;

// L'interface = le "contrat" : elle dit QUOI faire, pas COMMENT
public interface IClientService
{
    Task<List<Client>> GetAllAsync();
    Task<Client?> GetByIdAsync(int id);
    Task<Client> CreateAsync(Client client);
    Task<Client?> UpdateAsync(int id, Client client);
    Task<bool> DeleteAsync(int id);
}