using bepensa_biz.Interfaces;
using bepensa_data.models;
using bepensa_models.CRM;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace bepensa_biz.Settings
{
    public class SessionProxy : IAccessSession
    {
        private IHttpContextAccessor ContextAccesor { get; set; }

        public SessionProxy(IHttpContextAccessor httpContextAccessor)
        {
            ContextAccesor = httpContextAccessor;
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
        public LoginRequest Credenciales
        {
            get
            {
                return Get<LoginRequest>("Credenciales");
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

        public SesionCRM SesionCRM { get; set; }
        #endregion

        public void Logout()
        {
            ContextAccesor.HttpContext.Session.Clear();
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

            if (ContextAccesor.HttpContext.Request.IsHttps)
            {
                cookieOptions.Secure = true; // Solo se enviará por HTTPS
            }

            //Console.WriteLine($"Estableciendo cookie: {key} = {value}");

            ContextAccesor.HttpContext.Response.Cookies.Append(key, value, cookieOptions);
        }

        public string GetCookie(string key)
        {
            return ContextAccesor.HttpContext.Request.Cookies[key];
        }

        public void DeleteCookie(string key)
        {
            ContextAccesor.HttpContext.Response.Cookies.Append(key,"", new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(-1),
                HttpOnly = true,
                IsEssential = true,
                Path = "/",
                SameSite = SameSiteMode.Lax,
                Secure = true
            });
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

        private string Get(string key) => ContextAccesor.HttpContext.Session.GetString(key);

        public void SetSesion(string key, string value)
        {
            Set(key, value);
        }

        public string GetSesion(string key)
        {
            return Get(key);
        }

        private void Set<TType>(string key, TType value)
        {
            Set(key, JsonConvert.SerializeObject(value));
        }

        private void Set(string key, string value) => ContextAccesor.HttpContext.Session.SetString(key, value);

        public void RemoveSesion(string key) => ContextAccesor.HttpContext.Session.Remove(key);
        #endregion
    }
}
