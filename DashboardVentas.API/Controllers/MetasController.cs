using DashboardVentas.API.Data;
using DashboardVentas.API.DTOs;
using DashboardVentas.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DashboardVentas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetasController : ControllerBase
{
    private readonly AppDbContext _context;

    public MetasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] GuardarMetaDto dto)
    {
        if (dto.Mes < 1 || dto.Mes > 12)
        {
            return BadRequest("Mes inválido.");
        }

        if (dto.Meta <= 0)
        {
            return BadRequest("La meta debe ser mayor que cero.");
        }

        var metaExistente = await _context.MetasMensuales
            .FirstOrDefaultAsync(m => m.Anio == dto.Anio && m.Mes == dto.Mes);

        if (metaExistente == null)
        {
            metaExistente = new MetaMensual
            {
                Anio = dto.Anio,
                Mes = dto.Mes,
                Meta = decimal.Round(dto.Meta, 2, MidpointRounding.AwayFromZero)
            };

            _context.MetasMensuales.Add(metaExistente);
        }
        else
        {
            metaExistente.Meta = decimal.Round(dto.Meta, 2, MidpointRounding.AwayFromZero);
        }

        await _context.SaveChangesAsync();
        return Ok(metaExistente);
    }

    [HttpGet("{anio:int}/{mes:int}")]
    public async Task<IActionResult> Get(int anio, int mes)
    {
        var meta = await _context.MetasMensuales
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Anio == anio && m.Mes == mes);

        if (meta == null)
        {
            return NotFound();
        }

        return Ok(meta);
    }
}