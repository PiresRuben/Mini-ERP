using Microsoft.AspNetCore.Mvc;
using MiniERP.API.DTOs;
using MiniERP.API.Services;

namespace MiniERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FacturesController : ControllerBase
{
    private readonly IFactureService _factureService;

    public FacturesController(IFactureService factureService)
    {
        _factureService = factureService;
    }

    // GET /api/factures
    [HttpGet]
    public async Task<ActionResult<List<FactureDto>>> GetAll()
    {
        return Ok(await _factureService.GetAllAsync());
    }

    // GET /api/factures/5
    [HttpGet("{id}")]
    public async Task<ActionResult<FactureDto>> GetById(int id)
    {
        var facture = await _factureService.GetByIdAsync(id);
        if (facture == null) return NotFound();
        return Ok(facture);
    }

    // POST /api/factures
    [HttpPost]
    public async Task<ActionResult<FactureDto>> Create(CreateFactureDto dto)
    {
        try
        {
            var facture = await _factureService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = facture.Id }, facture);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT /api/factures/5/statut
    [HttpPut("{id}/statut")]
    public async Task<ActionResult<FactureDto>> UpdateStatut(int id, UpdateStatutDto dto)
    {
        var facture = await _factureService.UpdateStatutAsync(id, dto);
        if (facture == null) return NotFound();
        return Ok(facture);
    }

    // DELETE /api/factures/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if (!await _factureService.DeleteAsync(id)) return NotFound();
        return NoContent();
    }

    // === LIGNES DE FACTURE ===

    // POST /api/factures/5/lignes
    [HttpPost("{factureId}/lignes")]
    public async Task<ActionResult<FactureDto>> AddLigne(int factureId, CreateLigneFactureDto dto)
    {
        try
        {
            var facture = await _factureService.AddLigneAsync(factureId, dto);
            if (facture == null) return NotFound();
            return Ok(facture);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT /api/factures/5/lignes/3
    [HttpPut("{factureId}/lignes/{ligneId}")]
    public async Task<ActionResult<FactureDto>> UpdateLigne(
        int factureId, int ligneId, CreateLigneFactureDto dto)
    {
        var facture = await _factureService.UpdateLigneAsync(factureId, ligneId, dto);
        if (facture == null) return NotFound();
        return Ok(facture);
    }

    // DELETE /api/factures/5/lignes/3
    [HttpDelete("{factureId}/lignes/{ligneId}")]
    public async Task<ActionResult<FactureDto>> DeleteLigne(int factureId, int ligneId)
    {
        var facture = await _factureService.DeleteLigneAsync(factureId, ligneId);
        if (facture == null) return NotFound();
        return Ok(facture);
    }
}