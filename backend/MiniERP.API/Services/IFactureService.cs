using MiniERP.API.DTOs;

namespace MiniERP.API.Services;

public interface IFactureService
{
    Task<List<FactureDto>> GetAllAsync();
    Task<FactureDto?> GetByIdAsync(int id);
    Task<FactureDto> CreateAsync(CreateFactureDto dto);
    Task<FactureDto?> UpdateStatutAsync(int id, UpdateStatutDto dto);
    Task<bool> DeleteAsync(int id);

    // Gestion des lignes
    Task<FactureDto?> AddLigneAsync(int factureId, CreateLigneFactureDto dto);
    Task<FactureDto?> UpdateLigneAsync(int factureId, int ligneId, CreateLigneFactureDto dto);
    Task<FactureDto?> DeleteLigneAsync(int factureId, int ligneId);
}