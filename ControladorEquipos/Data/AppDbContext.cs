using ControladorEquipos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace ControladorEquipos.Data

{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Puertos> Puertos { get; set; }
        public DbSet<Equipos> Equipos { get; set; }
        public DbSet<HistorialEscaneos> HistorialEscaneos { get; set; }
        public DbSet<Equipos_tienen_puertos> Equipos_tienen_puertos { get; set; } = default!;

        public DbSet<NivelRiesgo> NivelRiesgo { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Puertos>(tabla =>
            {
                tabla.HasKey(p => p.Id);
                tabla.Property(p => p.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();


                tabla.Property(p => p.Puerto);

                tabla.Property(p => p.Descripcion);
            });


            modelBuilder.Entity<Equipos>(tabla =>
            {
                tabla.HasKey(p => p.Id);

                tabla.Property(p => p.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tabla.Property(p => p.Hostname).HasMaxLength(50);
                tabla.Property(p => p.IP).HasMaxLength(50);
                tabla.Property(p => p.MAC).HasMaxLength(50);
                tabla.Property(p => p.OS).HasMaxLength(50);

            });


            modelBuilder.Entity<Equipos_tienen_puertos>(tabla =>
            {
                tabla.HasKey(e => e.Id);

                tabla.Property(e => e.Id)
                    .UseIdentityColumn()
                    .ValueGeneratedOnAdd();

                tabla.HasOne(e => e.Equipo)
                    .WithMany(equipo => equipo.EquiposTienenPuertos)
                    .HasForeignKey(e => e.IdEquipo)
                    .OnDelete(DeleteBehavior.Cascade);

                tabla.HasOne(e => e.Puerto)
                    .WithMany(puerto => puerto.EquiposTienenPuertos)
                    .HasForeignKey(e => e.IdPuerto)
                    .OnDelete(DeleteBehavior.Cascade);
            });


        }
    }
}
