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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Id == id);

            if (usuario == null)
                return NotFound();

            var model = new UsuarioEditViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Ativo = usuario.Ativo
            };

            return View(model);
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

        [HttpPost]
        public IActionResult Edit(UsuarioEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool emailExiste = _context.Usuarios
                .Any(u => u.Email == model.Email
                       && u.Id != model.Id);

            if (emailExiste)
            {
                ModelState.AddModelError(
                    "",
                    "Já existe um usuário com este e-mail.");

                return View(model);
            }

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Id == model.Id);

            if (usuario == null)
                return NotFound();

            usuario.Nome = model.Nome;
            usuario.Email = model.Email;

            //Refatorar essa validação para um UsuarioService, evitando duplicação entre Edit e Desativar.
            if (usuario.PerfilId == 1 && usuario.Ativo && !model.Ativo)
            {
                int adminsAtivos = _context.Usuarios
                    .Count(u =>
                        u.PerfilId == 1 &&
                        u.Ativo);

                if (adminsAtivos <= 1)
                {
                    ModelState.AddModelError(
                        "",
                        "Deve existir pelo menos um administrador ativo.");

                    return View(model);
                }
            }
            usuario.Ativo = model.Ativo;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Desativar(int id)
        {
            var usuario = _context.Usuarios
                .Include(u => u.Perfil)
                .FirstOrDefault(u => u.Id == id);

            if (usuario == null)
                return NotFound();

            //Refatorar essa validação para um UsuarioService, evitando duplicação entre Edit e Desativar.
            if (usuario.PerfilId == 1)
            {
                int adminsAtivos = _context.Usuarios
                    .Count(u =>
                        u.PerfilId == 1 &&
                        u.Ativo);

                if (adminsAtivos <= 1)
                {
                    TempData["Erro"] =
                        "Deve existir pelo menos um administrador ativo.";

                    return RedirectToAction(nameof(Index));
                }
            }

            usuario.Ativo = false;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Reativar(int id)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Id == id);

            if (usuario == null)
                return NotFound();

            usuario.Ativo = true;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}