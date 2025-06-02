using ControladorEquipos.Data;
using ControladorEquipos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

namespace ControladorEquipos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var totalEquipos = _context.Equipos.Count();
            var totalPuertosEscaneados = _context.Equipos_tienen_puertos.Count();

            // Puertos más comunes (igual que antes)
            var puertosMasComunes = _context.Equipos_tienen_puertos
                .Include(e => e.Puerto)
                .GroupBy(e => e.IdPuerto)
                .Select(g => new
                {
                    Puerto = g.First().Puerto.Puerto,
                    Cantidad = g.Count()
                })
                .OrderByDescending(g => g.Cantidad)
                .Take(10)
                .ToList();

            // Puertos con riesgo alto
            var puertosRiesgoAlto = _context.Equipos_tienen_puertos
                .Include(e => e.Puerto)
                    .ThenInclude(p => p.NivelRiesgo)
                .Where(e => e.Puerto.NivelRiesgo.Descripcion == "Alto") // Ajusta la condición según tu modelo
                .GroupBy(e => e.IdPuerto)
                .Select(g => new
                {
                    Puerto = g.First().Puerto.Puerto,
                    Cantidad = g.Count()
                })
                .OrderByDescending(g => g.Cantidad)
                .Take(10)
                .ToList();

            var viewModel = new DashboardViewModel
            {
                TotalEquipos = totalEquipos,
                TotalPuertosEscaneados = totalPuertosEscaneados,
                PuertosMasComunes = puertosMasComunes
                    .Select(p => new PuertoContado
                    {
                        NumeroPuerto = p.Puerto,
                        Cantidad = p.Cantidad
                    }).ToList(),
                PuertosRiesgoAlto = puertosRiesgoAlto
                    .Select(p => new PuertoContado
                    {
                        NumeroPuerto = p.Puerto,
                        Cantidad = p.Cantidad
                    }).ToList()
            };

            return View(viewModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
