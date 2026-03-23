using Microsoft.AspNetCore.Mvc;
using MiniERP.API.DTOs;
using MiniERP.API.Models;
using MiniERP.API.Services;

namespace MiniERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]  // L'URL sera /api/clients
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    // GET /api/clients
    [HttpGet]
    public async Task<ActionResult<List<ClientDto>>> GetAll()
    {
        var clients = await _clientService.GetAllAsync();

        var result = clients.Select(c => new ClientDto
        {
            Id = c.Id,
            NomEntreprise = c.NomEntreprise,
            Siret = c.Siret,
            Adresse = c.Adresse,
            DateCreation = c.DateCreation
        }).ToList();

        return Ok(result);
    }

    // GET /api/clients/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ClientDto>> GetById(int id)
    {
        var client = await _clientService.GetByIdAsync(id);
        if (client == null) return NotFound();

        var result = new ClientDto
        {
            Id = client.Id,
            NomEntreprise = client.NomEntreprise,
            Siret = client.Siret,
            Adresse = client.Adresse,
            DateCreation = client.DateCreation
        };

        return Ok(result);
    }

    // POST /api/clients
    [HttpPost]
    public async Task<ActionResult<ClientDto>> Create(CreateClientDto dto)
    {
        var client = new Client
        {
            NomEntreprise = dto.NomEntreprise,
            Siret = dto.Siret,
            Adresse = dto.Adresse
        };

        var created = await _clientService.CreateAsync(client);

        var result = new ClientDto
        {
            Id = created.Id,
            NomEntreprise = created.NomEntreprise,
            Siret = created.Siret,
            Adresse = created.Adresse,
            DateCreation = created.DateCreation
        };

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // PUT /api/clients/5
    [HttpPut("{id}")]
    public async Task<ActionResult<ClientDto>> Update(int id, CreateClientDto dto)
    {
        var client = new Client
        {
            NomEntreprise = dto.NomEntreprise,
            Siret = dto.Siret,
            Adresse = dto.Adresse
        };

        var updated = await _clientService.UpdateAsync(id, client);
        if (updated == null) return NotFound();

        var result = new ClientDto
        {
            Id = updated.Id,
            NomEntreprise = updated.NomEntreprise,
            Siret = updated.Siret,
            Adresse = updated.Adresse,
            DateCreation = updated.DateCreation
        };

        return Ok(result);
    }

    // DELETE /api/clients/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _clientService.DeleteAsync(id);
        if (!success) return NotFound();

        return NoContent();  // 204 = supprimé avec succès
    }
}