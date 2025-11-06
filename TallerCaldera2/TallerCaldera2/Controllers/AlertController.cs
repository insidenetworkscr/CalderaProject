using Microsoft.AspNetCore.Mvc;

namespace TallerCaldera2.Controllers
{
    public class AlertController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
