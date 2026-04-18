namespace DashboardVentas.API.DTOs
{
    public class DetalleDiarioDto
    {
        public int Dia { get; set; }
        public DateTime Fecha { get; set; }
        public decimal VentaDia { get; set; }
        public decimal VentaAcumulada { get; set; }
        public decimal MetaAcumulada { get; set; }
        public decimal PorcentajeDia { get; set; }
        public decimal DiferenciaDia { get; set; }
    }
}
