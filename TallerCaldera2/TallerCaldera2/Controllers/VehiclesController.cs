using System.Linq;
using System.Threading.Tasks;
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
            if (plate == null) return NotFound();

            var vehicle = await _context.Vehicles
                .Include(v => v.Maintenances)
                .FirstOrDefaultAsync(v => v.Plate == plate);

            if (vehicle == null) return NotFound();

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create() => View();

        // POST: Vehicles/Create
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

        // GET: Vehicles/Edit/ABC123
        public async Task<IActionResult> Edit(string plate)
        {
            if (plate == null) return NotFound();

            var vehicle = await _context.Vehicles.FindAsync(plate);
            if (vehicle == null) return NotFound();

            return View(vehicle);
        }

        // POST: Vehicles/Edit/ABC123
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string plate, Vehicle vehicle)
        {
            if (plate != vehicle.Plate) return NotFound();
            if (!ModelState.IsValid) return View(vehicle);

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
            if (plate == null) return NotFound();

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.Plate == plate);

            if (vehicle == null) return NotFound();

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
