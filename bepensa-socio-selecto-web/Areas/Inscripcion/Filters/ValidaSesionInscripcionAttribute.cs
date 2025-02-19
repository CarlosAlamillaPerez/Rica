using bepensa_socio_selecto_biz.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace bepensa_socio_selecto_web.Areas.Inscripcion.Filters
{
    public class ValidaSesionInscripcionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sesion = context.HttpContext.RequestServices.GetService<IAccessSession>();

            var pInscripcion = sesion?.CredencialesInscripcion?.Inscripcion;

            if (pInscripcion == null)
            {
                // Redirigir al Login si no está inicializado
                context.Result = new RedirectToActionResult("Login", "Inscripciones", new { area = "Autenticacion" });
            }

            base.OnActionExecuting(context);
        }
    }
}
