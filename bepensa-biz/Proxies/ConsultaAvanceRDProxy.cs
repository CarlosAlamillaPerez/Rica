using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_models.ApiWa;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.Extensions.Configuration;
using bepensa_data.modelsRD;
using bepensa_biz.Extensions;

namespace bepensa_biz.Proxies
{
    public class ConsultaAvanceRDProxy : ProxyBase, IConsultasAvanceRDProxy
    {
        private readonly IConfiguration _configuration;
        public ConsultaAvanceRDProxy(BepensaRD_Context Context, IConfiguration Configuracion)
        {
            DBContextRD = Context;
            _configuration = Configuracion;
        }

        public Respuesta<List<CuotaDeCompraRDDTOWa>> ConsultaAvance(RequestCliente data)
        {
            Respuesta<List<CuotaDeCompraRDDTOWa>> resultado = new();
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
                long idnegocio = usuario.IdNegocio;
                int idperiodo= DBContextRD.Periodos.Where(x=>x.Fecha.Month==DateTime.Now.Month && x.Fecha.Year == DateTime.Now.Year).Select(x=>x.Id).First();
                //int idperiodo = 14;

                if (DBContextRD.DetalleDeMetaDeCompras.Where(x=>x.IdNegocio==idnegocio && x.IdPeriodo==idperiodo).Count() == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.NonExistentRecord;
                    resultado.Mensaje = CodigoDeError.NonExistentRecord.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }
                                
                
                List<CuotaDeCompraRDDTOWa> avance = DBContextRD.DetalleDeMetaDeCompras.Where(x => x.IdPeriodo == idperiodo && x.IdNegocio == idnegocio).Select(x => (CuotaDeCompraRDDTOWa)x).ToList();
                resultado.Data = avance;

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
