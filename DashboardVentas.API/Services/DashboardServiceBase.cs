using DashboardVentas.API.Data;
using DashboardVentas.API.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DashboardVentas.API.Services;

public class DashboardService
{
    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardMensualDto> ObtenerDashboardAsync(int anio, int mes)
    {
        var metaRegistro = await _context.MetasMensuales
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Anio == anio && m.Mes == mes);

        if (metaRegistro == null)
        {
            throw new InvalidOperationException("No existe meta configurada para este mes.");
        }

        int diasDelMes = DateTime.DaysInMonth(anio, mes);
        DateTime hoy = DateTime.Today;

        int diaCorte;

        if (anio == hoy.Year && mes == hoy.Month)
        {
            diaCorte = hoy.Day;
        }
        else if (new DateTime(anio, mes, diasDelMes) < hoy)
        {
            diaCorte = diasDelMes;
        }
        else
        {
            diaCorte = 0;
        }

        decimal metaMensual = metaRegistro.Meta;
        decimal metaDiaria = decimal.Round(metaMensual / diasDelMes, 2, MidpointRounding.AwayFromZero);

        var ventas = await _context.Ventas
            .AsNoTracking()
            .Where(v => v.Fecha.Year == anio && v.Fecha.Month == mes)
            .ToListAsync();

        var ventasPorDia = ventas
            .GroupBy(v => v.Fecha.Day)
            .ToDictionary(g => g.Key, g => g.Sum(x => x.Monto));

        decimal acumulado = 0;
        var detalle = new List<DetalleDiarioDto>();

        for (int dia = 1; dia <= diasDelMes; dia++)
        {
            decimal ventaDia = ventasPorDia.TryGetValue(dia, out var montoDia) ? montoDia : 0;
            acumulado += ventaDia;

            // 🔥 CORREGIDO (antes estaba con dia - 1)
            decimal metaAcumulada = decimal.Round(metaDiaria * dia, 2, MidpointRounding.AwayFromZero);

            decimal porcentajeDia = metaAcumulada == 0
                ? 0
                : decimal.Round((acumulado / metaAcumulada) * 100, 2, MidpointRounding.AwayFromZero);

            decimal diferenciaDia = decimal.Round(acumulado - metaAcumulada, 2, MidpointRounding.AwayFromZero);

            var ventaActual = ventas.FirstOrDefault(v => v.Fecha.Day == dia);

            detalle.Add(new DetalleDiarioDto
            {
                Id = ventaActual?.Id ?? 0,
                Dia = dia,
                Fecha = new DateTime(anio, mes, dia),
                VentaDia = decimal.Round(ventaDia, 2, MidpointRounding.AwayFromZero),
                VentaAcumulada = decimal.Round(acumulado, 2, MidpointRounding.AwayFromZero),
                MetaAcumulada = metaAcumulada,
                PorcentajeDia = porcentajeDia,
                DiferenciaDia = diferenciaDia
            });
        }

        decimal ventaAcumulada = detalle
            .Where(d => d.Dia <= Math.Max(diaCorte, 0))
            .LastOrDefault()?.VentaAcumulada ?? 0;

        // 🔥 CORREGIDO (antes estaba con diaCorte - 1)
        decimal alDia = decimal.Round(metaDiaria * diaCorte-2, 2, MidpointRounding.AwayFromZero);

        decimal porcentajeAlDia = alDia == 0
            ? 0
            : decimal.Round((ventaAcumulada / alDia) * 100, 2, MidpointRounding.AwayFromZero);

        decimal diferencia = decimal.Round(ventaAcumulada - alDia, 2, MidpointRounding.AwayFromZero);
        decimal falta = decimal.Round(metaMensual - ventaAcumulada, 2, MidpointRounding.AwayFromZero);

        decimal porcentajeTotal = metaMensual == 0
            ? 0
            : decimal.Round((ventaAcumulada / metaMensual) * 100, 2, MidpointRounding.AwayFromZero);

        // 🔥 Esto ya está correcto
        int diasRestantes = Math.Max(diasDelMes - diaCorte+1, 0);

        decimal diariaParaMeta = (diasRestantes > 0 && falta > 0)
            ? decimal.Round(falta / diasRestantes, 2, MidpointRounding.AwayFromZero)
            : 0;

        var mesPasado = await ObtenerComparativoMismoCorteAsync(anio, mes, diaCorte, -1);
        var anioPasado = await ObtenerComparativoAnualMismoCorteAsync(anio, mes, diaCorte);

        return new DashboardMensualDto
        {
            Anio = anio,
            Mes = mes,
            NombreMes = CultureInfo.GetCultureInfo("es-SV").DateTimeFormat.GetMonthName(mes),
            DiaCorte = diaCorte,
            DiasDelMes = diasDelMes,
            MetaMensual = decimal.Round(metaMensual, 2, MidpointRounding.AwayFromZero),
            MetaDiaria = metaDiaria,
            VentaAcumulada = ventaAcumulada,
            AlDia = alDia,
            PorcentajeAlDia = porcentajeAlDia,
            Diferencia = diferencia,
            Falta = falta,
            DiariaParaMeta = diariaParaMeta,
            PorcentajeTotal = porcentajeTotal,
            MesPasado = mesPasado,
            AnioPasado = anioPasado,
            ProyeccionTienda = decimal.Round(diariaParaMeta * 0.5m, 2, MidpointRounding.AwayFromZero),
            ProyeccionVendedor = decimal.Round(diariaParaMeta * 0.5m / 5, 2, MidpointRounding.AwayFromZero),
            ProyeccionMayoreo = decimal.Round(diariaParaMeta * 0.1m / 5, 2, MidpointRounding.AwayFromZero),
            Detalle = detalle
        };
    }

    private async Task<ResumenComparativoDto> ObtenerComparativoMismoCorteAsync(int anio, int mes, int diaCorte, int offsetMeses)
    {
        DateTime fechaBase = new DateTime(anio, mes, 1).AddMonths(offsetMeses);
        return await ObtenerComparativoAsync(fechaBase.Year, fechaBase.Month, diaCorte);
    }

    private async Task<ResumenComparativoDto> ObtenerComparativoAnualMismoCorteAsync(int anio, int mes, int diaCorte)
    {
        return await ObtenerComparativoAsync(anio - 1, mes, diaCorte);
    }

    private async Task<ResumenComparativoDto> ObtenerComparativoAsync(int anio, int mes, int diaCorte)
    {
        var metaRegistro = await _context.MetasMensuales
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Anio == anio && m.Mes == mes);

        if (metaRegistro == null)
        {
            return new ResumenComparativoDto();
        }

        int diasDelMes = DateTime.DaysInMonth(anio, mes);
        int diaRealCorte = Math.Min(Math.Max(diaCorte, 0), diasDelMes);

        decimal metaDiaria = decimal.Round(metaRegistro.Meta / diasDelMes, 2, MidpointRounding.AwayFromZero);
        decimal metaAlCorte = decimal.Round(metaDiaria * diaRealCorte, 2, MidpointRounding.AwayFromZero);

        decimal ventaAcumulada = await _context.Ventas
            .AsNoTracking()
            .Where(v => v.Fecha.Year == anio && v.Fecha.Month == mes && v.Fecha.Day <= diaRealCorte)
            .SumAsync(v => (decimal?)v.Monto) ?? 0;

        ventaAcumulada = decimal.Round(ventaAcumulada, 2, MidpointRounding.AwayFromZero);

        decimal porcentaje = metaAlCorte == 0
            ? 0
            : decimal.Round((ventaAcumulada / metaAlCorte) * 100, 2, MidpointRounding.AwayFromZero);

        decimal diferencia = decimal.Round(ventaAcumulada - metaAlCorte, 2, MidpointRounding.AwayFromZero);

        return new ResumenComparativoDto
        {
            VentaAcumulada = ventaAcumulada,
            Porcentaje = porcentaje,
            Diferencia = diferencia
        };
    }
}

