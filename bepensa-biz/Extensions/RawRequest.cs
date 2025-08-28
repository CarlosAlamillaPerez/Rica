using Microsoft.AspNetCore.Http;

namespace Business.BepensaWhatsapp.Extensiones
{
    public class RawRequest
    {
        private readonly RequestDelegate _next;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public RawRequest(RequestDelegate next)
        {
            _next = next;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();
            await _next(httpContext);
        }
    }
}
