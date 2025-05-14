using bepensa_biz.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace bepensa_ss_crm.Filters
{
    public class ValidaSesionUsuarioAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sesion = context.HttpContext.RequestServices.GetService<IAccessSession>();

            var sesionActiva = sesion?.UsuarioActual;

            if (sesionActiva == null)
            {

                context.Result = new RedirectToActionResult("Index", "Socios", new { area = "Usuario" });
            }

            base.OnActionExecuting(context);
        }
    }
}
