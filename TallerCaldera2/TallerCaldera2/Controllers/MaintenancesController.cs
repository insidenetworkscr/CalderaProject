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

        // ================= INDEX =================
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

        // ================= DETAILS =================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var maintenance = await _context.Maintenances
                .Include(m => m.Vehicle)
                .Include(m => m.Photos)
                .Include(m => m.SketchMarks)
                .Include(m => m.Items)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (maintenance == null) return NotFound();

            return View(maintenance);
        }

        // ================= CREATE GET =================
        public IActionResult Create(string vehiclePlate = null)
        {
            ViewData["VehiclePlate"] = new SelectList(_context.Vehicles, "Plate", "Plate", vehiclePlate);

            return View(new Maintenance
            {
                Date = DateTime.Now,
                VehiclePlate = vehiclePlate,
                Items = new List<MaintenanceItem>()
            });
        }

        // ================= CREATE POST =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(524288000)]
        [RequestFormLimits(MultipartBodyLengthLimit = 524288000)]
        public async Task<IActionResult> Create(Maintenance maintenance, List<IFormFile> photos, string SketchData)
        {
            ModelState.Remove("Cost");

            maintenance.Items = maintenance.Items?
                .Where(i => !string.IsNullOrWhiteSpace(i.Servicio))
                .ToList() ?? new List<MaintenanceItem>();

            maintenance.Cost = maintenance.Items.Sum(i => i.Unidad * i.Precio);

            if (!ModelState.IsValid)
            {
                ViewData["VehiclePlate"] =
                    new SelectList(_context.Vehicles, "Plate", "Plate", maintenance.VehiclePlate);

                return View(maintenance);
            }

            _context.Add(maintenance);
            await _context.SaveChangesAsync();

            // Actualizar última fecha de mantenimiento del vehículo
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Plate == maintenance.VehiclePlate);
            if (vehicle != null)
            {
                vehicle.LastMaintenanceDate = maintenance.Date;
            }

            // CREAR ALERTA DE MANTENIMIENTO
            var dueDate = maintenance.Date.AddMonths(6);

            // Buscar alerta existente del vehículo
            var existingAlert = await _context.Alerts
                .FirstOrDefaultAsync(a => a.VehiclePlate == maintenance.VehiclePlate);

            if (existingAlert != null)
            {
                // Actualizar alerta existente con nueva fecha
                existingAlert.DueDate = dueDate;
                existingAlert.IsShown = false;
            }
            else
            {
                // Crear alerta nueva solo si no existe
                _context.Alerts.Add(new Alert
                {
                    VehiclePlate = maintenance.VehiclePlate,
                    DueDate = dueDate,
                    Message = "Mantenimiento próximo",
                    IsShown = false
                });
            }

            // Guardar fotos
            await SavePhotosAsync(maintenance.Id, photos);

            // Guardar marcas del boceto
            SaveSketchMarks(maintenance.Id, SketchData);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // ================= EDIT GET =================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var maintenance = await _context.Maintenances
                .Include(m => m.Photos)
                .Include(m => m.SketchMarks)
                .Include(m => m.Items)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (maintenance == null) return NotFound();

            ViewData["VehiclePlate"] = new SelectList(
                _context.Vehicles,
                "Plate",
                "Plate",
                maintenance.VehiclePlate);

            return View(maintenance);
        }

        // ================= EDIT POST =================
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
                .Include(m => m.Items)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (existing == null)
                return NotFound();

           
            ModelState.Remove("Cost");

            // ================= ITEMS =================
            _context.MaintenanceItems.RemoveRange(existing.Items);

            existing.Items = maintenance.Items?
                .Where(i => !string.IsNullOrWhiteSpace(i.Servicio))
                .Select(i => new MaintenanceItem
                {
                    Servicio = i.Servicio,
                    Unidad = i.Unidad,
                    Precio = i.Precio,
                    MaintenanceId = existing.Id
                })
                .ToList() ?? new List<MaintenanceItem>();

            // ✅ COSTO SIEMPRE CALCULADO EN SERVIDOR
            existing.Cost = existing.Items.Sum(i => i.Unidad * i.Precio);

            if (!ModelState.IsValid)
            {
                ViewData["VehiclePlate"] = new SelectList(
                    _context.Vehicles,
                    "Plate",
                    "Plate",
                    maintenance.VehiclePlate);

                return View(existing);
            }

            // ================= CAMPOS BÁSICOS =================
            existing.Date = maintenance.Date;
            existing.Type = maintenance.Type;
            existing.Provider = maintenance.Provider;
            existing.Observations = maintenance.Observations;
            existing.Mileage = maintenance.Mileage;
            existing.VehiclePlate = maintenance.VehiclePlate;
            existing.TrabajosPorRealizar = maintenance.TrabajosPorRealizar;
            existing.TrabajosRealizados = maintenance.TrabajosRealizados;
            existing.FormaPago = maintenance.FormaPago;
            existing.Combustible = maintenance.Combustible;

            // ❌ NO volver a tocar Cost aquí
            // existing.Cost = maintenance.Cost;  <-- ELIMINADO

            // ================= BORRAR FOTOS =================
            if (deletePhotoIds != null && deletePhotoIds.Length > 0)
            {
                var toDelete = existing.Photos
                    .Where(p => deletePhotoIds.Contains(p.Id))
                    .ToList();

                foreach (var photo in toDelete)
                {
                    if (!string.IsNullOrWhiteSpace(photo.ImageUrl))
                    {
                        var physical = Path.Combine(
                            _env.WebRootPath,
                            photo.ImageUrl.TrimStart('/')
                                .Replace('/', Path.DirectorySeparatorChar));

                        if (System.IO.File.Exists(physical))
                            System.IO.File.Delete(physical);
                    }

                    _context.MaintenancePhotos.Remove(photo);
                }
            }

            // ================= NUEVAS FOTOS =================
            await SavePhotosAsync(existing.Id, newPhotos);

            // ================= BOCETO =================
            _context.SketchMarks.RemoveRange(existing.SketchMarks);
            SaveSketchMarks(existing.Id, SketchData);

            // ================= ACTUALIZAR VEHÍCULO =================
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.Plate == existing.VehiclePlate);

            if (vehicle != null)
                vehicle.LastMaintenanceDate = existing.Date;

            // ================= ACTUALIZAR ALERTA =================
            var dueDate = existing.Date.AddMonths(6);

            var existingAlert = await _context.Alerts
                .FirstOrDefaultAsync(a => a.VehiclePlate == existing.VehiclePlate);

            if (existingAlert != null)
            {
                existingAlert.DueDate = dueDate;
                existingAlert.IsShown = false;
            }
            else
            {
                _context.Alerts.Add(new Alert
                {
                    VehiclePlate = existing.VehiclePlate,
                    DueDate = dueDate,
                    Message = "Mantenimiento próximo",
                    IsShown = false
                });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // ================= DELETE =================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var maintenance = await _context.Maintenances
                .Include(m => m.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (maintenance == null) return NotFound();

            return View(maintenance);
        }

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
                    if (!string.IsNullOrWhiteSpace(photo.ImageUrl))
                    {
                        var physical = Path.Combine(_env.WebRootPath,
                            photo.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                        if (System.IO.File.Exists(physical))
                            System.IO.File.Delete(physical);
                    }
                }

                _context.Maintenances.Remove(maintenance);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ================= HELPERS =================
        private async Task SavePhotosAsync(int maintenanceId, List<IFormFile> photos)
        {
            if (photos == null || photos.Count == 0) return;

            var folder = Path.Combine(_env.WebRootPath, "uploads", "maintenances", maintenanceId.ToString());

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            foreach (var file in photos)
            {
                if (file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
                    var path = Path.Combine(folder, fileName);

                    using var stream = new FileStream(path, FileMode.Create);
                    await file.CopyToAsync(stream);

                    _context.MaintenancePhotos.Add(new MaintenancePhoto
                    {
                        MaintenanceId = maintenanceId,
                        ImageUrl = $"/uploads/maintenances/{maintenanceId}/{fileName}"
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