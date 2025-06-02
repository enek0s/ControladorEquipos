namespace ControladorEquipos.Models
{
    public class Equipos_tienen_puertos
    {
        public int Id { get; set; }

        // Claves foráneas
        public int IdEquipo { get; set; }
        public int IdPuerto { get; set; }

        // Propiedades de navegación
        public Equipos? Equipo { get; set; }
        public Puertos? Puerto { get; set; }
    }
}
