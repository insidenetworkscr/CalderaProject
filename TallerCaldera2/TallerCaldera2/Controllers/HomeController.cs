using System;
using System.Linq;
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
                // === TARJETAS ===
                AlertasActivas = _context.Alerts
                    .Count(a => !a.IsShown),

                TotalVehiculos = _context.Vehicles.Count(),

                MantenimientosMes = _context.Maintenances
                    .Count(m => m.Date >= inicioMes && m.Date < finMes),

                CitasPendientes = _context.Appointments
                    .Count(c => c.Status == AppointmentStatus.Pendiente)
            };

    
            var mantenimientosPorMes = _context.Maintenances
                .Where(m => m.Date >= hoy.AddMonths(-5))
                .GroupBy(m => new { m.Date.Year, m.Date.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g => new
                {
                    Mes = $"{g.Key.Month}/{g.Key.Year}",
                    Total = g.Count()
                })
                .ToList();

            model.MesesMantenimientos = mantenimientosPorMes
                .Select(x => x.Mes)
                .ToList();

            model.CantidadMantenimientos = mantenimientosPorMes
                .Select(x => x.Total)
                .ToList();

            var citasPorEstado = _context.Appointments
                .GroupBy(c => c.Status)
                .Select(g => new
                {
                    Estado = g.Key.ToString(),
                    Total = g.Count()
                })
                .ToList();

            model.EstadosCitas = citasPorEstado
                .Select(x => x.Estado)
                .ToList();

            model.CantidadCitas = citasPorEstado
                .Select(x => x.Total)
                .ToList();

      
            var vehiculosPorMarca = _context.Vehicles
                .GroupBy(v => v.Brand)
                .Select(g => new
                {
                    Marca = g.Key,
                    Total = g.Count()
                })
                .ToList();

            model.MarcasVehiculos = vehiculosPorMarca
                .Select(x => x.Marca)
                .ToList();

            model.CantidadVehiculos = vehiculosPorMarca
                .Select(x => x.Total)
                .ToList();

            return View(model);
        }
    }
}
