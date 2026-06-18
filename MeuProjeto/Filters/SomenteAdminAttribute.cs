using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MeuProjeto.Filters
{
    public class SomenteAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(
            ActionExecutingContext context)
        {
            var perfilId = context.HttpContext
                .Session
                .GetInt32("PerfilId");

            if (perfilId != 1)
            {
                context.Result = new RedirectToActionResult(
                    "Index",
                    "Home",
                    null);
            }

            base.OnActionExecuting(context);
        }
    }
}