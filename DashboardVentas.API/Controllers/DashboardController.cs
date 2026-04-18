using DashboardVentas.API.DTOs;
using DashboardVentas.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DashboardVentas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly DashboardService _dashboardService;

    public DashboardController(DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("{anio:int}/{mes:int}")]
    public async Task<ActionResult<DashboardMensualDto>> Get(int anio, int mes)
    {
        if (mes < 1 || mes > 12)
        {
            return BadRequest("El mes debe estar entre 1 y 12.");
        }

        try
        {
            var dashboard = await _dashboardService.ObtenerDashboardAsync(anio, mes);
            return Ok(dashboard);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }
}