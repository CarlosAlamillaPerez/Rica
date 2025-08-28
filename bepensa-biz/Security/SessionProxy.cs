using bepensa_biz.Interfaces;
using bepensa_data.models;
using bepensa_models.App;
using bepensa_models.CRM;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace bepensa_biz.Security
{
    public class SessionProxy : IAccessSession
    {
        private IHttpContextAccessor _contextAccessor { get; set; }

        public SessionProxy(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        #region Inscripcion
        public LoginInscripcionRequest CredencialesInscripcion
        {
            get
            {
                return Get<LoginInscripcionRequest>("credenciales_inscripcion");
            }
            set
            {
                Set("credenciales_inscripcion", value);
            }
        }
        #endregion

        #region Web
        public LoginApp Credenciales
        {
            get
            {
                return Get<LoginApp>("Credenciales");
            }
            set
            {
                Set("Credenciales", value);
            }
        }

        public UsuarioDTO UsuarioActual
        {
            get
            {
                return Get<UsuarioDTO>("usuario_actual");
            }
            set
            {
                Set("usuario_actual", value);
            }
        }

        public bool ForzarCambio
        {
            get { return Get<bool>(nameof(ForzarCambio)); }

            set
            {
                Set(nameof(ForzarCambio), value);
            }
        }
        #endregion

        #region CRM
        public LoginCRMRequest CredencialesCRM
        {
            get
            {
                return Get<LoginCRMRequest>("credencialesCRM");
            }
            set
            {
                Set("credencialesCRM", value);
            }
        }

        public OperadorDTO OperadorActual
        {
            get
            {
                return Get<OperadorDTO>("operador_actual");
            }
            set
            {
                Set("operador_actual", value);
            }
        }

        public FuerzaVentaDTO FuerzaVenta
        {
            get
            {
                return Get<FuerzaVentaDTO>("fdv_actual");
            }
            set
            {
                Set("fdv_actual", value);
            }
        }

        public List<SeccionDTO> CrmMenuOperador
        {
            get
            {
                return Get<List<SeccionDTO>>("menu_user");
            }
            set
            {
                Set("menu_user", value);
            }
        }
        #endregion

        public void Logout()
        {
            _contextAccessor.HttpContext.Session.Clear();
        }

        #region Cookies
        public void SetCookie(string key, string value, TimeSpan expiration)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.Add(expiration),
                HttpOnly = true,
                IsEssential = true,
                Path = "/",
                SameSite = SameSiteMode.Lax
            };

            if (_contextAccessor.HttpContext.Request.IsHttps)
            {
                cookieOptions.Secure = true; // Solo se enviará por HTTPS
            }

            //Console.WriteLine($"Estableciendo cookie: {key} = {value}");

            _contextAccessor.HttpContext.Response.Cookies.Append(key, value, cookieOptions);
        }
        #endregion

        #region Metodos Privados
        private TType Get<TType>(string key)
        {
            var data = Get(key);
            if (data == null)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<TType>(data);
        }

        private string Get(string key) => _contextAccessor.HttpContext.Session.GetString(key);

        private void Set<TType>(string key, TType value)
        {
            Set(key, JsonConvert.SerializeObject(value));
        }

        private void Set(string key, string value) => _contextAccessor.HttpContext.Session.SetString(key, value);
        #endregion
    }
}
