using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TallerCaldera.Models;
using TallerCaldera2.Models;

namespace TallerCaldera2.Controllers
{
    public class MaintenancesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MaintenancesController> _logger;

        public MaintenancesController(ApplicationDbContext context, ILogger<MaintenancesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Maintenances
        public async Task<IActionResult> Index()
        {
            var query = _context.Maintenances
                .Include(m => m.Vehicle)
                .OrderByDescending(m => m.Date);

            var list = await query.ToListAsync();
            return View(list);
        }

        // GET: Maintenances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var maintenance = await _context.Maintenances
                .Include(m => m.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (maintenance == null)
                return NotFound();

            return View(maintenance);
        }

        // GET: Maintenances/Create
        // opcionalmente recibe plate desde Detalles del vehículo
        public IActionResult Create(string? vehiclePlate)
        {
            ViewData["VehiclePlate"] = new SelectList(_context.Vehicles, "Plate", "Plate", vehiclePlate);
            return View();
        }

        // POST: Maintenances/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Type,Observations,Cost,Mileage,VehiclePlate")] Maintenance maintenance)
        {
            _logger.LogInformation("=== POST /Maintenances/Create ===");
            _logger.LogInformation("VehiclePlate: {Plate}, Type: {Type}, Cost: {Cost}, Date: {Date}",
                maintenance.VehiclePlate, maintenance.Type, maintenance.Cost, maintenance.Date);

            // Evitar que la navegación 'Vehicle' rompa la validación
            ModelState.Remove(nameof(Maintenance.Vehicle));

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido en Create Maintenance:");
                foreach (var kvp in ModelState)
                {
                    foreach (var error in kvp.Value.Errors)
                    {
                        _logger.LogWarning(" - {Key}: {Error}", kvp.Key, error.ErrorMessage);
                    }
                }

                ViewData["VehiclePlate"] = new SelectList(_context.Vehicles, "Plate", "Plate", maintenance.VehiclePlate);
                return View(maintenance);
            }

            _context.Add(maintenance);

            // Actualizar fecha del último mantenimiento del vehículo
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Plate == maintenance.VehiclePlate);
            if (vehicle != null)
            {
                vehicle.LastMaintenanceDate = maintenance.Date;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Vehicles", new { plate = maintenance.VehiclePlate });
        }

        // GET: Maintenances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var maintenance = await _context.Maintenances.FindAsync(id);
            if (maintenance == null)
                return NotFound();

            ViewData["VehiclePlate"] = new SelectList(_context.Vehicles, "Plate", "Plate", maintenance.VehiclePlate);

            return View(maintenance);
        }

        // POST: Maintenances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Type,Observations,Cost,Mileage,VehiclePlate")] Maintenance maintenance)
        {
            if (id != maintenance.Id)
                return NotFound();

            ModelState.Remove(nameof(Maintenance.Vehicle));

            if (!ModelState.IsValid)
            {
                ViewData["VehiclePlate"] = new SelectList(_context.Vehicles, "Plate", "Plate", maintenance.VehiclePlate);
                return View(maintenance);
            }

            try
            {
                _context.Update(maintenance);

                var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Plate == maintenance.VehiclePlate);
                if (vehicle != null)
                {
                    // si quieres, solo actualizar si es la fecha más reciente
                    if (!vehicle.LastMaintenanceDate.HasValue || maintenance.Date > vehicle.LastMaintenanceDate.Value)
                    {
                        vehicle.LastMaintenanceDate = maintenance.Date;
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaintenanceExists(maintenance.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction("Details", "Vehicles", new { plate = maintenance.VehiclePlate });
        }

        // GET: Maintenances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var maintenance = await _context.Maintenances
                .Include(m => m.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (maintenance == null)
                return NotFound();

            return View(maintenance);
        }

        // POST: Maintenances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var maintenance = await _context.Maintenances.FindAsync(id);
            if (maintenance != null)
            {
                var plate = maintenance.VehiclePlate;

                _context.Maintenances.Remove(maintenance);
                await _context.SaveChangesAsync();

                // Recalcular última fecha de mantenimiento
                var vehicle = await _context.Vehicles
                    .Include(v => v.Maintenances)
                    .FirstOrDefaultAsync(v => v.Plate == plate);

                if (vehicle != null)
                {
                    vehicle.LastMaintenanceDate = vehicle.Maintenances.Any()
                        ? vehicle.Maintenances.Max(m => m.Date)
                        : null;

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Details", "Vehicles", new { plate });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MaintenanceExists(int id)
        {
            return _context.Maintenances.Any(e => e.Id == id);
        }
    }
}
