using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TallerCaldera2.Models;
using TallerCaldera2.Models.ViewModels;

namespace TallerCaldera2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hoy = DateTime.Today;
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
            var finMes = inicioMes.AddMonths(1);

            var model = new DashboardViewModel
            {
                // ?? Alertas activas
                AlertasActivas = _context.Alerts
                    .Count(a => !a.IsShown),

                // ?? Total de vehículos
                TotalVehiculos = _context.Vehicles.Count(),

                // ?? Mantenimientos del mes actual
                MantenimientosMes = _context.Maintenances
                    .Count(m => m.Date >= inicioMes && m.Date < finMes),

                // ?? Citas pendientes
                CitasPendientes = _context.Appointments
                    .Count(c => c.Status == AppointmentStatus.Pendiente)
            };

            return View(model);
        }
    }
}