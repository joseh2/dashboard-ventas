using DashboardVentas.API.Data;
using DashboardVentas.API.DTOs;
using DashboardVentas.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DashboardVentas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VentasController : ControllerBase
{
    private readonly AppDbContext _context;

    public VentasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CrearVentaDto dto)
    {
        if (dto.Monto < 0)
        {
            return BadRequest("El monto no puede ser negativo.");
        }

        var venta = new venta
        {
             Fecha = DateTime.SpecifyKind(dto.Fecha, DateTimeKind.Utc),
            Monto = decimal.Round(dto.Monto, 2, MidpointRounding.AwayFromZero)
        };

        _context.Ventas.Add(venta);
        await _context.SaveChangesAsync();

        return Ok(venta);
    }

    [HttpGet("{anio:int}/{mes:int}")]
    public async Task<IActionResult> Get(int anio, int mes)
    {
        var ventas = await _context.Ventas
            .AsNoTracking()
            .Where(v => v.Fecha.Year == anio && v.Fecha.Month == mes)
            .OrderBy(v => v.Fecha)
            .ToListAsync();

        return Ok(ventas);
    }
}