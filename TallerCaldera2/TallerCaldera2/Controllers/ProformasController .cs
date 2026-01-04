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

        public ProformasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.Proformas
                .Include(p => p.Items)
                .ToList();

            return View(data);
        }

        public IActionResult Create()
        {
            var model = new Proforma();
            model.Items.Add(new ProformaItem()); // mínimo 1 item
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Proforma proforma)
        {
            Console.WriteLine($"➡ Entró al POST | Items: {proforma.Items.Count}");

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    foreach (var e in error.Value.Errors)
                    {
                        Console.WriteLine($"❌ {error.Key}: {e.ErrorMessage}");
                    }
                }
                return View(proforma);
            }

            proforma.Codigo = $"PRO-{DateTime.Now.Year}-{_context.Proformas.Count() + 1:0000}";
            proforma.FechaEmision = DateTime.Now;
            proforma.FechaValidez = DateTime.Now.AddDays(15);

            // 🔥 ASIGNAR RELACIÓN
            foreach (var item in proforma.Items)
            {
                item.Proforma = proforma;
            }

            _context.Proformas.Add(proforma);
            _context.SaveChanges();

            Console.WriteLine("✅ Proforma guardada correctamente");

            return RedirectToAction(nameof(Index));
        }

        public IActionResult DescargarPdf(int id)
        {
            var proforma = _context.Proformas
                .Include(p => p.Items)
                .FirstOrDefault(p => p.Id == id);

            if (proforma == null)
                return NotFound();

            var pdf = new ProformaPdf(proforma);
            return File(pdf.GeneratePdf(), "application/pdf", $"{proforma.Codigo}.pdf");
        }
    }
}
