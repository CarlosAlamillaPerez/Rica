using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_models.ApiWa;
using bepensa_data.modelsRD;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.Extensions.Configuration;

namespace bepensa_biz.Proxies
{
    public class ConsultaPromocionesRDProxy : ProxyBase, IConsultasPromocionesRDProxy
    {
        private readonly IConfiguration _configuration;
        public ConsultaPromocionesRDProxy(BepensaRD_Context Context, IConfiguration Configuracion)
        {
            DBContextRD = Context;
            _configuration = Configuracion;
        }

        public Respuesta<List<PromocionesRDDTOWa>> ConsultaPromociones(RequestCliente data)
        {
            Respuesta<List<PromocionesRDDTOWa>> resultado = new();
            resultado.Data = new();

            try
            {
                var valida = Extensions.Extensiones.ValidateRequest(data);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;
                    goto final;
                }


                if (!DBContextRD.Usuarios.Any(x => x.Cuc == data.Cliente))
                {
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                Usuario usuario = DBContextRD.Usuarios.FirstOrDefault(x => x.Cuc == data.Cliente);//variable se guarda lo de usuario
                int idprograma = DBContextRD.Programas.Where(x => x.Id == usuario.IdPrograma).Select(y => y.Id).First();

                if (DBContextRD.ImagenesPromociones.Where(x=>x.IdPrograma==idprograma).Count() == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.NonExistentRecord;
                    resultado.Mensaje = CodigoDeError.NonExistentRecord.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                List<PromocionesRDDTOWa> promociones = DBContextRD.ImagenesPromociones.Where(x =>x.Tipo == "prm" && x.FechaReg.Year == DateTime.Now.Year && x.FechaReg.Month == DateTime.Now.Month).Select(x => (PromocionesRDDTOWa)x).ToList();
                resultado.Data = promociones;

            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
                //
            }
        final:
            return resultado;
        }
    }
}
