using bepensa_biz.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace bepensa_ss_op_web.Areas.FuerzaVenta.Filters
{
    public class ValidaSesionFDVAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sesion = context.HttpContext.RequestServices.GetService<IAccessSession>();

            var sesionActiva = sesion?.FuerzaVenta;

            if (sesionActiva == null)
            {

                context.Result = new RedirectToActionResult("Index", "Home", new { area = "Socio" });
            }

            base.OnActionExecuting(context);
        }
    }
}
