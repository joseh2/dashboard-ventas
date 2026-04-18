namespace DashboardVentas.API.DTOs
{
   
    public class DashboardMensualDto
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public string NombreMes { get; set; } = string.Empty;
        public int DiaCorte { get; set; }
        public int DiasDelMes { get; set; }

        public decimal MetaMensual { get; set; }
        public decimal MetaDiaria { get; set; }
        public decimal VentaAcumulada { get; set; }

        public decimal AlDia { get; set; }
        public decimal PorcentajeAlDia { get; set; }
        public decimal Diferencia { get; set; }

        public decimal Falta { get; set; }
        public decimal DiariaParaMeta { get; set; }
        public decimal PorcentajeTotal { get; set; }

        public ResumenComparativoDto MesPasado { get; set; } = new();
        public ResumenComparativoDto AnioPasado { get; set; } = new();

        public decimal ProyeccionTienda { get; set; }
        public decimal ProyeccionVendedor { get; set; }
        public decimal ProyeccionMayoreo { get; set; }

        public List<DetalleDiarioDto> Detalle { get; set; } = new();
    }
}
