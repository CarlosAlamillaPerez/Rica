using bepensa_socio_selecto_biz.Interfaces;
using bepensa_socio_selecto_models.DataModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace bepensa_socio_selecto_biz.Settings
{
    public class SessionProxy : IAccessSession
    {
        private IHttpContextAccessor ContextAccesor { get; set; }

        public SessionProxy(IHttpContextAccessor httpContextAccessor)
        {
            ContextAccesor = httpContextAccessor;
        }

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

        public void Logout()
        {
            ContextAccesor.HttpContext.Session.Clear();
        }

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

        private void Set<TType>(string key, TType value)
        {
            Set(key, JsonConvert.SerializeObject(value));
        }

        private void Set(string key, string value) => ContextAccesor.HttpContext.Session.SetString(key, value);
        #endregion
    }
}
