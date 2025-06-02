namespace ControladorEquipos.Models
{
    public class DashboardViewModel
    {
        public int TotalEquipos { get; set; }
        public int TotalPuertosEscaneados { get; set; }

        public List<PuertoContado> PuertosMasComunes { get; set; } = new();
        public List<PuertoContado> PuertosRiesgoAlto { get; set; } = new();  // Nueva lista para riesgo alto
    }

    public class PuertoContado
    {
        public int NumeroPuerto { get; set; }
        public int Cantidad { get; set; }
    }


}
