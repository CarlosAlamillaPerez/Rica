using bepensa_biz.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace bepensa_ss_web.Filters
{
    public class ValidaSesionUsuarioAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var areaActual = context.RouteData.Values["area"]?.ToString()?.ToLower();

            if (areaActual == "FuerzaVenta")
            {
                base.OnActionExecuting(context);
                return;
            }

            var sesion = context.HttpContext.RequestServices.GetService<IAccessSession>();
            var sesionActiva = sesion?.UsuarioActual;

            if (sesionActiva == null)
            {
                if (sesion?.FuerzaVenta != null)
                {
                    context.Result = new RedirectToActionResult("Index", "Home", new { area = "FuerzaVenta" });
                }
                else
                {
                    context.Result = new RedirectToActionResult("Login", "Cuentas", new { area = "Autenticacion" });
                }
            }

            base.OnActionExecuting(context);
        }
    }

}
