using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControladorEquipos.Models;
using ControladorEquipos.Data;

namespace ControladorEquipos.Controllers
{
    public class EquiposController : Controller
    {
        private readonly AppDbContext _context;

        public EquiposController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var equiposOrdenados = await _context.Equipos
                .OrderBy(e => e.IP)
                .ToListAsync();

            return View(equiposOrdenados);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var equipo = await _context.Equipos
                .Include(e => e.EquiposTienenPuertos!)
                    .ThenInclude(etp => etp.Puerto!)
                        .ThenInclude(p => p.NivelRiesgo)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (equipo == null) return NotFound();

            return View(equipo);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Hostname,IP,MAC,OS")] Equipos equipos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(equipos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(equipos);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var equipos = await _context.Equipos.FindAsync(id);
            if (equipos == null) return NotFound();

            return View(equipos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Hostname,IP,MAC,OS")] Equipos equipos)
        {
            if (id != equipos.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquiposExists(equipos.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(equipos);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var equipos = await _context.Equipos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipos == null) return NotFound();

            return View(equipos);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipos = await _context.Equipos.FindAsync(id);
            if (equipos != null) _context.Equipos.Remove(equipos);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquiposExists(int id)
        {
            return _context.Equipos.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return RedirectToAction(nameof(Index));

            bool esPuerto = int.TryParse(query, out int numeroPuerto);

            if (esPuerto)
            {
                var equiposConPuerto = await _context.Equipos_tienen_puertos
                    .Where(etp => etp.Puerto!.Puerto == numeroPuerto)
                    .Select(etp => etp.Equipo)
                    .Distinct()
                    .OrderBy(e => e.IP)
                    .ThenBy(e => e.Hostname)
                    .ToListAsync();

                if (!equiposConPuerto.Any())
                {
                    TempData["MensajeBusqueda"] = $"No se encontraron equipos con el puerto {numeroPuerto}.";
                    return RedirectToAction(nameof(Index));
                }

                return View("EquiposPorPuerto", equiposConPuerto);
            }
            else
            {
                var equipo = await _context.Equipos
                    .FirstOrDefaultAsync(e => e.Hostname!.ToLower() == query.ToLower() || e.IP.ToLower() == query.ToLower());

                if (equipo == null)
                {
                    TempData["MensajeBusqueda"] = $"No se encontró equipo con Hostname o IP '{query}'.";
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Details), new { id = equipo.Id });
            }
        }

        [HttpPost]
        public IActionResult VerificarYEjecutar()
        {
            string mensaje = "";
            bool todoCorrecto = true;

            // Verificar Python
            var pythonCheck = EjecutarComando("python", "--version");
            if (!pythonCheck.Contains("3.13.3"))
            {
                mensaje += "❌ Python 3.13.3 no está instalado o no es la versión correcta.<br/>";
                todoCorrecto = false;
            }
            else
            {
                mensaje += "✅ Python 3.13.3 detectado.<br/>";
            }

            // Verificar Nmap con ruta completa
            string rutaNmap = @"C:\nmap\nmap.exe";
            if (!System.IO.File.Exists(rutaNmap))
            {
                mensaje += "❌ Nmap no está instalado en la ruta esperada.<br/>";
                todoCorrecto = false;
            }
            else
            {
                var nmapCheck = EjecutarComando(rutaNmap, "--version");
                if (!nmapCheck.Contains("7.96"))
                {
                    mensaje += "❌ Nmap 7.96 no está instalado.<br/>";
                    todoCorrecto = false;
                }
                else
                {
                    mensaje += "✅ Nmap 7.96 detectado.<br/>";
                }
            }

            // Ejecutar script.py si todo correcto
            if (todoCorrecto)
            {
                string rutaScript = @"C:\nmap\script.py";
                if (!System.IO.File.Exists(rutaScript))
                {
                    mensaje += "❌ No se encontró script.py en la ruta esperada.<br/>";
                    todoCorrecto = false;
                }
                else
                {
                    var resultadoScript = EjecutarComando("python", $"\"{rutaScript}\"");
                    mensaje += "✅ Script ejecutado:<br/><pre>" + System.Net.WebUtility.HtmlEncode(resultadoScript) + "</pre>";
                }
            }

            TempData["MensajeBusqueda"] = mensaje;
            return RedirectToAction(nameof(Index));
        }


        private string EjecutarComando(string archivo, string argumentos)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = archivo,
                    Arguments = argumentos,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var proceso = Process.Start(psi))
                {
                    string salida = proceso.StandardOutput.ReadToEnd();
                    string error = proceso.StandardError.ReadToEnd();
                    proceso.WaitForExit();
                    return salida + error;
                }
            }
            catch (Exception ex)
            {
                return $"Error ejecutando comando: {ex.Message}";
            }
        }
    }
}
