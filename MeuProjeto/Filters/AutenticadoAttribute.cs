using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MeuProjeto.Filters
{
    public class AutenticadoAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(
            ActionExecutingContext context)
        {
            var usuarioId = context.HttpContext
                .Session
                .GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                context.Result = new RedirectToActionResult(
                    "Login",
                    "Conta",
                    null);
            }

            base.OnActionExecuting(context);
        }
    }
}