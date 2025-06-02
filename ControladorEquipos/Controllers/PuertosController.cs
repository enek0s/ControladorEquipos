using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControladorEquipos.Models;
using ControladorEquipos.Data;

namespace ControladorEquipos.Controllers
{
    public class PuertosController : Controller
    {
        private readonly AppDbContext _context;

        public PuertosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Puertos
        public async Task<IActionResult> Index()
        {
            var puertos = await _context.Puertos
                .Include(p => p.NivelRiesgo)
                .OrderBy(p => p.Puerto)
                .ToListAsync();

            return View(puertos);
        }

        // GET: Puertos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var puerto = await _context.Puertos
                .Include(p => p.NivelRiesgo)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (puerto == null) return NotFound();

            return View(puerto);
        }

        // GET: Puertos/Create
        public IActionResult Create()
        {
            ViewData["NivelRiesgoId"] = new SelectList(_context.NivelRiesgo, "Id", "Descripcion");
            return View();
        }

        // POST: Puertos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Puerto,Descripcion,NivelRiesgoId")] Puertos puertos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(puertos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Si hay error, vuelve a enviar la lista para que el dropdown funcione
            ViewData["NivelRiesgoId"] = new SelectList(_context.NivelRiesgo, "Id", "Descripcion", puertos.NivelRiesgoId);
            return View(puertos);
        }


        // GET: Puertos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var puertos = await _context.Puertos.FindAsync(id);
            if (puertos == null) return NotFound();

            ViewData["NivelRiesgoId"] = new SelectList(_context.NivelRiesgo, "Id", "Descripcion", puertos.NivelRiesgoId);
            return View(puertos);
        }

        // POST: Puertos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Puerto,Descripcion,NivelRiesgoId")] Puertos puertos)
        {
            if (id != puertos.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(puertos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PuertosExists(puertos.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["NivelRiesgoId"] = new SelectList(_context.NivelRiesgo, "Id", "Descripcion", puertos.NivelRiesgoId);
            return View(puertos);
        }

        // GET: Puertos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var puertos = await _context.Puertos
                .Include(p => p.NivelRiesgo)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (puertos == null) return NotFound();

            return View(puertos);
        }

        // POST: Puertos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var puertos = await _context.Puertos.FindAsync(id);
            if (puertos != null)
            {
                _context.Puertos.Remove(puertos);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PuertosExists(int id)
        {
            return _context.Puertos.Any(e => e.Id == id);
        }
    }
}
