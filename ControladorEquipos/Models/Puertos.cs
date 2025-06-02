using System.Collections.Generic;

namespace ControladorEquipos.Models
{
    public class Puertos
    {
        public int Id { get; set; }

        public int Puerto { get; set; }

        public string? Descripcion { get; set; }

        // Nueva FK
        public int? NivelRiesgoId { get; set; }

        // Nueva propiedad de navegación
        public virtual NivelRiesgo? NivelRiesgo { get; set; }

        public ICollection<Equipos_tienen_puertos>? EquiposTienenPuertos { get; set; }
    }

}
