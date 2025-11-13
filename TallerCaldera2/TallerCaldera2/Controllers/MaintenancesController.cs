using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TallerCaldera.Models;
using TallerCaldera2.Models;

namespace TallerCaldera2.Controllers
{
    public class MaintenancesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly string uploadPath;

        public MaintenancesController(ApplicationDbContext context)
        {
            _context = context;
            uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/mantenimientos");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);
        }

        // INDEX
        public async Task<IActionResult> Index()
        {
            var data = _context.Maintenances
                .Include(m => m.Vehicle)
                .Include(m => m.Photos);

            return View(await data.ToListAsync());
        }

        // DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var maintenance = await _context.Maintenances
                .Include(m => m.Vehicle)
                .Include(m => m.Photos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (maintenance == null)
                return NotFound();

            return View(maintenance);
        }

        // GET: Create
        public IActionResult Create(string? vehiclePlate)
        {
            ViewData["VehiclePlate"] =
                new SelectList(_context.Vehicles, "Plate", "Plate", vehiclePlate);

            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Maintenance maintenance, List<IFormFile> photos)
        {
            ModelState.Remove(nameof(Maintenance.Vehicle));

            if (!ModelState.IsValid)
            {
                ViewData["VehiclePlate"] =
                    new SelectList(_context.Vehicles, "Plate", "Plate", maintenance.VehiclePlate);
                return View(maintenance);
            }

            _context.Add(maintenance);
            await _context.SaveChangesAsync();

            // GUARDAR FOTOS
            foreach (var file in photos)
            {
                if (file.Length > 0)
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    string filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await file.CopyToAsync(stream);

                    _context.MaintenancePhotos.Add(new MaintenancePhoto
                    {
                        MaintenanceId = maintenance.Id,
                        ImageUrl = $"/uploads/mantenimientos/{fileName}"
                    });
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Vehicles", new { plate = maintenance.VehiclePlate });
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var maintenance = await _context.Maintenances
                .Include(m => m.Photos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (maintenance == null) return NotFound();

            ViewData["VehiclePlate"] = new SelectList(_context.Vehicles, "Plate", "Plate");

            return View(maintenance);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Maintenance maintenance, List<IFormFile> newPhotos, List<int> deletePhotos)
        {
            if (id != maintenance.Id)
                return NotFound();

            ModelState.Remove(nameof(Maintenance.Vehicle));

            if (!ModelState.IsValid)
            {
                ViewData["VehiclePlate"] = new SelectList(_context.Vehicles, "Plate", "Plate");
                return View(maintenance);
            }

            _context.Update(maintenance);
            await _context.SaveChangesAsync();

            // ELIMINAR FOTOS SELECCIONADAS
            if (deletePhotos != null)
            {
                foreach (var photoId in deletePhotos)
                {
                    var photo = await _context.MaintenancePhotos.FindAsync(photoId);

                    if (photo != null)
                    {
                        string physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", photo.ImageUrl.TrimStart('/'));

                        if (System.IO.File.Exists(physicalPath))
                            System.IO.File.Delete(physicalPath);

                        _context.MaintenancePhotos.Remove(photo);
                    }
                }
            }

            // AGREGAR NUEVAS FOTOS
            foreach (var file in newPhotos)
            {
                if (file.Length > 0)
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    string filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await file.CopyToAsync(stream);

                    _context.MaintenancePhotos.Add(new MaintenancePhoto
                    {
                        MaintenanceId = maintenance.Id,
                        ImageUrl = $"/uploads/mantenimientos/{fileName}"
                    });
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Maintenances", new { id = maintenance.Id });
        }

        // DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var maintenance = await _context.Maintenances
                .Include(m => m.Photos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (maintenance != null)
            {
                foreach (var photo in maintenance.Photos)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", photo.ImageUrl.TrimStart('/'));

                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }

                _context.Maintenances.Remove(maintenance);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
