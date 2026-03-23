using Microsoft.AspNetCore.Mvc;
using MiniERP.API.DTOs;
using MiniERP.API.Services;

namespace MiniERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProduitsController : ControllerBase
{
    private readonly IProduitService _produitService;

    public ProduitsController(IProduitService produitService)
    {
        _produitService = produitService;
    }

    // GET /api/produits
    [HttpGet]
    public async Task<ActionResult<List<ProduitDto>>> GetAll()
    {
        var produits = await _produitService.GetAllAsync();

        var result = produits.Select(p => new ProduitDto
        {
            Id = p.Id,
            Reference = p.Reference,
            Designation = p.Designation,
            PrixUnitaireHT = p.PrixUnitaireHT
        }).ToList();

        return Ok(result);
    }
}