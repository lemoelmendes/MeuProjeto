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
    }
}