using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using TallerCaldera2.Models;
using TallerCaldera2.PdfDocuments;


namespace TallerCaldera2.Controllers
{
    public class ProformasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProformasController(
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // ===============================
        // LISTADO + BUSCADOR
        // ===============================
        public async Task<IActionResult> Index(string search)
        {
            var query = _context.Proformas
                .Include(p => p.Items)      // 🔥 CLAVE
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.ClienteNombre.Contains(search) ||
                    p.Codigo.Contains(search) ||
                    p.ClienteEmail.Contains(search)
                );
            }

            var proformas = await query
                .OrderByDescending(p => p.FechaEmision)
                .ToListAsync();

            ViewBag.Search = search;

            return View(proformas);
        }

        // ===============================
        // FORM CREAR
        // ===============================
        public IActionResult Create()
        {
            var model = new Proforma
            {
                FechaEmision = DateTime.Now,
                FechaValidez = DateTime.Now.AddDays(15)
            };

            model.Items.Add(new ProformaItem());
            return View(model);
        }

        // ===============================
        // GUARDAR
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Proforma proforma,
            List<IFormFile> Imagenes)
        {
            if (proforma.Items == null || !proforma.Items.Any())
            {
                ModelState.AddModelError("", "Debe agregar al menos un item");
            }

            if (!ModelState.IsValid)
            {
                return View(proforma);
            }

            // ===============================
            // SUBIR IMÁGENES
            // ===============================
            if (Imagenes != null && Imagenes.Any())
            {
                var uploadsFolder = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "proformas");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                foreach (var img in Imagenes)
                {
                    if (img.Length == 0) continue;

                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(img.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await img.CopyToAsync(stream);

                    proforma.Images.Add(new ProformaImage
                    {
                        ImageUrl = "/uploads/proformas/" + fileName
                    });
                }

                proforma.ImagenUrl = proforma.Images.First().ImageUrl;
            }

            // ===============================
            // CÓDIGO
            // ===============================
            var count = await _context.Proformas.CountAsync() + 1;
            proforma.Codigo = $"PRO-{DateTime.Now.Year}-{count:0000}";

            proforma.FechaEmision = DateTime.Now;
            proforma.FechaValidez = DateTime.Now.AddDays(15);

            foreach (var item in proforma.Items)
            {
                item.Proforma = proforma;
            }

            _context.Proformas.Add(proforma);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ===============================
        // PDF
        // ===============================
        public async Task<IActionResult> DescargarPdf(int id)
        {
            var proforma = await _context.Proformas
                .Include(p => p.Items)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proforma == null)
                return NotFound();

            var pdf = new ProformaPdf(proforma, _environment);

            return File(
                pdf.GeneratePdf(),
                "application/pdf",
                $"{proforma.Codigo}.pdf"
            );
        }

        // ===============================
        // ELIMINAR
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var proforma = await _context.Proformas
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proforma == null)
                return NotFound();

            _context.Proformas.Remove(proforma);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}