using Microsoft.AspNetCore.Mvc;
using MeuProjeto.Filters;

namespace MeuProjeto.Controllers
{
    [Autenticado]
    [SomenteAdmin]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}