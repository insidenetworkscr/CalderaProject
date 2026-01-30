using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TallerCaldera2.Ayudas;
using TallerCaldera2.Models;
using System.Linq;

public class AuthController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // ===================== LOGIN =====================
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string fullName, string password)
    {
        string hash = EncriptarContra.Hash(password);

        var user = _context.Users
            .FirstOrDefault(u => u.FullName == fullName && u.PasswordHash == hash);

        if (user == null)
        {
            ViewBag.Error = "❌ Nombre o contraseña incorrectos";
            return View();
        }

        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("UserName", user.FullName);

        return RedirectToAction("Index", "Home");
    }

    // ===================== REGISTER =====================
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

        // 🔒 Validar correo repetido
        bool correoExiste = _context.Users.Any(u => u.Email == user.Email);
        if (correoExiste)
        {
            ViewBag.Error = "Ya existe un usuario con ese correo";
            return View();
        }

        // 🔒 Validar nombre repetido
        bool nombreExiste = _context.Users.Any(u => u.FullName == user.FullName);
        if (nombreExiste)
        {
            ViewBag.Error = "Ya existe un usuario con ese nombre";
            return View();
        }

        user.PasswordHash = EncriptarContra.Hash(user.PasswordHash);

        _context.Users.Add(user);
        _context.SaveChanges();

        TempData["Success"] = "Usuario registrado correctamente";
        return RedirectToAction("Login");
    }

    // ===================== LOGOUT =====================
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
