using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControladorEquipos.Data;
using ControladorEquipos.Models;

namespace ControladorEquipos.Controllers
{
    public class HistorialEscaneosController : Controller
    {
        private readonly AppDbContext _context;

        public HistorialEscaneosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string filterBy, string query)
        {
            var escaneos = _context.HistorialEscaneos.AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                query = query.ToLower();

                switch (filterBy)
                {
                    case "ip":
                        escaneos = escaneos.Where(h => h.IP.ToLower().Contains(query));
                        break;
                    case "puerto":
                        escaneos = escaneos.Where(h => h.PuertosAbiertos.ToLower().Contains(query));
                        break;
                    default:
                        escaneos = escaneos.Where(h => h.Hostname.ToLower().Contains(query));
                        break;
                }
            }

            var lista = await escaneos.OrderBy(h => h.FechaEscaneo).ToListAsync();

            return View(lista);
        }
    }


}
