using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeuProjeto.Data;
using MeuProjeto.Filters;
using MeuProjeto.ViewModels;

namespace MeuProjeto.Controllers
{
    [Autenticado]
    [SomenteAdmin]
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var usuarios = _context.Usuarios
                .Include(u => u.Perfil)
                .Select(u => new UsuarioViewModel
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    Perfil = u.Perfil != null
                        ? u.Perfil.Nome
                        : "",
                    Ativo = u.Ativo
                })
                .ToList();

            return View(usuarios);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UsuarioCreateViewModel model)
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

            string hash =
                BCrypt.Net.BCrypt.HashPassword(
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

            return RedirectToAction(nameof(Index));
        }
    }
}