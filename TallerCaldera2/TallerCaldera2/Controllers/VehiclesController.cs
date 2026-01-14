using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TallerCaldera2.Models;

namespace TallerCaldera2.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehiclesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var vehicles = await _context.Vehicles
                .OrderByDescending(v => v.CreatedDate)
                .ToListAsync();

            return View(vehicles);
        }

        // GET: Vehicles/Details/ABC123
        public async Task<IActionResult> Details(string plate)
        {
            if (plate == null)
                return NotFound();

            var vehicle = await _context.Vehicles
                .Include(v => v.Maintenances)
                    .ThenInclude(m => m.Photos)
                .Include(v => v.Maintenances)
                    .ThenInclude(m => m.SketchMarks)
                .FirstOrDefaultAsync(v => v.Plate == plate);

            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            // 🔎 VALIDAR PLACA REPETIDA
            bool plateExists = await _context.Vehicles
                .AnyAsync(v => v.Plate == vehicle.Plate);

            if (plateExists)
            {
                ModelState.AddModelError("Plate", "La placa ya está registrada.");
            }

            if (!ModelState.IsValid)
            {
                return View(vehicle);
            }

            vehicle.CreatedDate = DateTime.UtcNow;
            _context.Add(vehicle);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Vehicles/Edit/ABC123
        public async Task<IActionResult> Edit(string plate)
        {
            if (plate == null)
                return NotFound();

            var vehicle = await _context.Vehicles.FindAsync(plate);

            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }

        // POST: Vehicles/Edit/ABC123
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string plate, Vehicle vehicle)
        {
            if (plate != vehicle.Plate)
                return NotFound();

            // 🔎 VALIDAR PLACA REPETIDA (por seguridad)
            bool plateExists = await _context.Vehicles
                .AnyAsync(v => v.Plate == vehicle.Plate && v.Plate != plate);

            if (plateExists)
            {
                ModelState.AddModelError("Plate", "La placa ya está registrada.");
            }

            if (!ModelState.IsValid)
            {
                return View(vehicle);
            }

            try
            {
                _context.Update(vehicle);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Vehicles.Any(v => v.Plate == vehicle.Plate))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Vehicles/Delete/ABC123
        public async Task<IActionResult> Delete(string plate)
        {
            if (plate == null)
                return NotFound();

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.Plate == plate);

            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }

        // POST: Vehicles/Delete/ABC123
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string plate)
        {
            var vehicle = await _context.Vehicles.FindAsync(plate);

            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
