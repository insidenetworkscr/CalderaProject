using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TallerCaldera.Models;
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

        public async Task<IActionResult> Index()
        {
            var vehicles = await _context.Vehicles
                .OrderByDescending(v => v.CreatedDate)
                .ToListAsync();
            return View(vehicles);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle == null) return NotFound();
            return View(vehicle);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            if (!ModelState.IsValid) return View(vehicle);
            vehicle.CreatedDate = DateTime.UtcNow;
            _context.Add(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound();
            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehicle vehicle)
        {
            if (id != vehicle.Id) return NotFound();
            if (!ModelState.IsValid) return View(vehicle);

            _context.Update(vehicle);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle == null) return NotFound();
            return View(vehicle);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
