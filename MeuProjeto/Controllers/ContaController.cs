using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeuProjeto.Data;
using MeuProjeto.ViewModels;

namespace MeuProjeto.Controllers
{
    public class ContaController : Controller
    {
        private readonly AppDbContext _context;

        public ContaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = _context.Usuarios
                .Include(u => u.Perfil)
                .FirstOrDefault(u => u.Email == model.Email);

            if (usuario == null)
            {
                ModelState.AddModelError("", "Usuário ou senha inválidos");
                return View(model);
            }

            bool senhaValida = BCrypt.Net.BCrypt.Verify(
                model.Senha,
                usuario.SenhaHash);

            if (!senhaValida)
            {
                ModelState.AddModelError("", "Usuário ou senha inválidos");
                return View(model);
            }

            if (!usuario.Ativo)
            {
                ModelState.AddModelError(
                    "",
                    "Usuário desativado.");

                return View(model);
            }

            HttpContext.Session.SetInt32(
                "UsuarioId",
                usuario.Id);

            HttpContext.Session.SetString(
                "Nome",
                usuario.Nome);

            HttpContext.Session.SetInt32(
                "PerfilId",
                usuario.PerfilId);

            HttpContext.Session.SetString(
                "PerfilNome",
                usuario.Perfil?.Nome ?? "");

            return RedirectToAction(
                "Index",
                "Home");
        }

        [HttpPost]
        public IActionResult Registrar(RegistroViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool emailExiste = _context.Usuarios
                .Any(u => u.Email == model.Email);

            if (emailExiste)
            {
                ModelState.AddModelError(
                    "",
                    "Já existe um usuário com este e-mail.");

                return View(model);
            }

            string hash = BCrypt.Net.BCrypt.HashPassword(
                model.Senha);

            var usuario = new Models.Usuario
            {
                Nome = model.Nome,
                Email = model.Email,
                SenhaHash = hash,
                PerfilId = 2,
                Ativo = true
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Login",
                "Conta");
        }
    }
}