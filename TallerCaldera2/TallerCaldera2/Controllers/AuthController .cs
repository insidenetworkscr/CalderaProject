using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TallerCaldera2.Ayudas;
using TallerCaldera2.Models;

public class AuthController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // LOGIN
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        string hash = EncriptarContra.Hash(password);

        var user = _context.Users
            .FirstOrDefault(u => u.Email == email && u.PasswordHash == hash);

        if (user == null)
        {
            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }

        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("UserName", user.FullName);

        return RedirectToAction("Index", "Home");
    }

    // REGISTER
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(User user, string registerCode)
    {
        string secretCode = _config["RegisterCode"];

        if (registerCode != secretCode)
        {
            ViewBag.Error = "Código de registro incorrecto";
            return View();
        }

        user.PasswordHash = EncriptarContra.Hash(user.PasswordHash);

        _context.Users.Add(user);
        _context.SaveChanges();

        return RedirectToAction("Login");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
