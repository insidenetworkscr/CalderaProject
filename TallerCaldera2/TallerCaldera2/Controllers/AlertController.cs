using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TallerCaldera2.Models;



namespace TallerCaldera2.Controllers
{
    public class AlertController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AlertController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTADO DE ALERTAS
        public IActionResult Index()
        {
            var alerts = _context.Alerts
                .Include(a => a.Vehicle)
                .Where(a => !a.IsShown)
                .OrderBy(a => a.DueDate)
                .ToList();

            return View(alerts);
        }

        // MARCAR COMO ATENDIDA
        [HttpPost]
        public IActionResult MarkAsShown(int id)
        {
            var alert = _context.Alerts.FirstOrDefault(a => a.Id == id);

            if (alert == null)
                return NotFound();

            alert.IsShown = true;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}