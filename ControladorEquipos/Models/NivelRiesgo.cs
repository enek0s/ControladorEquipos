namespace ControladorEquipos.Models
{
    public class NivelRiesgo
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Puertos> Puertos { get; set; }  // Navegación inversa
    }

}
