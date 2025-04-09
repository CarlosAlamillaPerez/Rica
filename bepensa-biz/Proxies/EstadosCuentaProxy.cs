using bepensa_biz.Extensions;
using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_models.DataModels;
using bepensa_models.Enums;
using bepensa_models.General;

namespace bepensa_biz.Proxies
{
    public class EstadosCuentaProxy : ProxyBase, IEstadoCuenta
    {
        public EstadosCuentaProxy(BepensaContext context)
        {
            DBContext = context;
        }

        public Respuesta<object> ConsultarTotales(UsuarioPeriodoRequest pUsuario)
        {
            Respuesta<object> resultado = new();

            try
            {
                var valida = Extensiones.ValidateRequest(pUsuario);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = false;

                    return resultado;
                }


            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }
    }
}
