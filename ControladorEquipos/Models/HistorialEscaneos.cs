using System;

namespace ControladorEquipos.Models
{
    public class HistorialEscaneos
    {
        public int Id { get; set; }
        public string? Hostname { get; set; }
        public string IP { get; set; }
        public string? MAC { get; set; }
        public string? OS { get; set; }
        public string? PuertosAbiertos { get; set; }  // Ejemplo: "22,80,443"
        public DateTime FechaEscaneo { get; set; }
    }
}
