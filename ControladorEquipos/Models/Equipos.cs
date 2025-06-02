using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControladorEquipos.Models
{
    public class Equipos
    {
        public int Id { get; set; }
        public string? Hostname { get; set; }
        public string IP { get; set; }
        public string? MAC { get; set; }
        public string? OS { get; set; }

        // NUEVA propiedad de navegación a la tabla intermedia
        public ICollection<Equipos_tienen_puertos>? EquiposTienenPuertos { get; set; }
    }
}
