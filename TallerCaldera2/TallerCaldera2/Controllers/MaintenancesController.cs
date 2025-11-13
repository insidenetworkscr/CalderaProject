using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        private readonly IWebHostEnvironment _env;

        public MaintenancesController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Maintenances
        public async Task<IActionResult> Index()
        {
            var maintenances = await _context.Maintenances
                .Include(m => m.Vehicle)
                .Include(m => m.Photos)
                .Include(m => m.SketchMarks)
                .OrderByDescending(m => m.Date)
                .ToListAsync();

            return View(maintenances);
        }

        // GET: Maintenances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var maintenance = await _context.Maintenances
                .Include(m => m.Vehicle)
                .Include(m => m.Photos)
                .Include(m => m.SketchMarks)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (maintenance == null)
                return NotFound();

            return View(maintenance);
        }

        // GET: Maintenances/Create
        public IActionResult Create(string vehiclePlate = null)
        {
            ViewData["VehiclePlate"] = new SelectList(_context.Vehicles, "Plate", "Plate", vehiclePlate);
            return View(new Maintenance
            {
                Date = DateTime.Now,
                VehiclePlate = vehiclePlate
            });
        }

        // POST: Maintenances/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Maintenance maintenance,
            List<IFormFile> photos,
            string SketchData)
        {
            if (!ModelState.IsValid)
            {
                ViewData["VehiclePlate"] = new SelectList(_context.Vehicles, "Plate", "Plate", maintenance.VehiclePlate);
                return View(maintenance);
            }

            // Guardar mantenimiento primero
            _context.Add(maintenance);
            await _context.SaveChangesAsync();

            // Actualizar última fecha de mantenimiento del vehículo
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Plate == maintenance.VehiclePlate);
            if (vehicle != null)
            {
                vehicle.LastMaintenanceDate = maintenance.Date;
            }

            // Guardar fotos
            await SavePhotosAsync(maintenance.Id, photos);

            // Guardar marcas del boceto
            SaveSketchMarks(maintenance.Id, SketchData);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Maintenances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var maintenance = await _context.Maintenances
                .Include(m => m.Photos)
                .Include(m => m.SketchMarks)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (maintenance == null)
                return NotFound();

            ViewData["VehiclePlate"] = new SelectList(_context.Vehicles, "Plate", "Plate", maintenance.VehiclePlate);

            return View(maintenance);
        }

        // POST: Maintenances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Maintenance maintenance,
            List<IFormFile> newPhotos,
            string SketchData,
            int[] deletePhotoIds)
        {
            if (id != maintenance.Id)
                return NotFound();

            var existing = await _context.Maintenances
                .Include(m => m.Photos)
                .Include(m => m.SketchMarks)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (existing == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["VehiclePlate"] = new SelectList(_context.Vehicles, "Plate", "Plate", maintenance.VehiclePlate);
                return View(existing);
            }

            // Actualizar campos básicos
            existing.Date = maintenance.Date;
            existing.Type = maintenance.Type;
            existing.Observations = maintenance.Observations;
            existing.Cost = maintenance.Cost;
            existing.Mileage = maintenance.Mileage;
            existing.VehiclePlate = maintenance.VehiclePlate;

            // Borrar fotos seleccionadas
            if (deletePhotoIds != null && deletePhotoIds.Length > 0)
            {
                var toDelete = existing.Photos
                    .Where(p => deletePhotoIds.Contains(p.Id))
                    .ToList();

                foreach (var photo in toDelete)
                {
                    // borrar archivo físico
                    if (!string.IsNullOrWhiteSpace(photo.ImageUrl))
                    {
                        var physical = Path.Combine(_env.WebRootPath, photo.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                        if (System.IO.File.Exists(physical))
                            System.IO.File.Delete(physical);
                    }

                    _context.MaintenancePhotos.Remove(photo);
                }
            }

            // Agregar fotos nuevas
            await SavePhotosAsync(existing.Id, newPhotos);

            // Reemplazar marcas del boceto
            var oldMarks = existing.SketchMarks.ToList();
            _context.SketchMarks.RemoveRange(oldMarks);
            SaveSketchMarks(existing.Id, SketchData);

            // Actualizar fecha de último mantenimiento
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Plate == existing.VehiclePlate);
            if (vehicle != null)
            {
                vehicle.LastMaintenanceDate = existing.Date;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
            var maintenance = await _context.Maintenances
                .Include(m => m.Photos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (maintenance != null)
            {
                // Borrar archivos físicos de fotos
                foreach (var photo in maintenance.Photos)
                {
                    if (!string.IsNullOrWhiteSpace(photo.ImageUrl))
                    {
                        var physical = Path.Combine(_env.WebRootPath, photo.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                        if (System.IO.File.Exists(physical))
                            System.IO.File.Delete(physical);
                    }
                }

                _context.Maintenances.Remove(maintenance);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task SavePhotosAsync(int maintenanceId, List<IFormFile> photos)
        {
            if (photos == null || photos.Count == 0)
                return;

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "maintenances", maintenanceId.ToString());
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            foreach (var file in photos)
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var relativePath = $"/uploads/maintenances/{maintenanceId}/{fileName}";

                    _context.MaintenancePhotos.Add(new MaintenancePhoto
                    {
                        MaintenanceId = maintenanceId,
                        ImageUrl = relativePath
                    });
                }
            }
        }

        private void SaveSketchMarks(int maintenanceId, string sketchData)
        {
            if (string.IsNullOrWhiteSpace(sketchData))
                return;

            var pairs = sketchData.Split('|', StringSplitOptions.RemoveEmptyEntries);
            foreach (var p in pairs)
            {
                var xy = p.Split(',', StringSplitOptions.RemoveEmptyEntries);
                if (xy.Length == 2 &&
                    int.TryParse(xy[0], out var x) &&
                    int.TryParse(xy[1], out var y))
                {
                    _context.SketchMarks.Add(new SketchMark
                    {
                        MaintenanceId = maintenanceId,
                        PosX = x,
                        PosY = y
                    });
                }
            }
        }
    }
}
