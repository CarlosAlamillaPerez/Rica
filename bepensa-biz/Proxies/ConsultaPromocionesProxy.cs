using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.ApiWa;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bepensa_biz.Proxies
{
    public class ConsultaPromocionesProxy : ProxyBase, IConsultasPromocionesProxy
    {
        private readonly IConfiguration _configuration;
        public ConsultaPromocionesProxy(BepensaContext Context, IConfiguration Configuracion)
        {
            DBContext = Context;
            _configuration = Configuracion;
        }

        public Respuesta<List<PromocionesDTOWa>> ConsultaPromociones(RequestCliente data)
        {
            Respuesta<List<PromocionesDTOWa>> resultado = new();
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


                if (!DBContext.Usuarios.Any(x => x.Cuc == data.Cliente))
                {
                    resultado.Codigo = (int)CodigoDeError.NonExistentRecord;
                    resultado.Mensaje = CodigoDeError.NonExistentRecord.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                Usuario usuario = DBContext.Usuarios.FirstOrDefault(x => x.Cuc == data.Cliente);//variable se guarda lo de usuario                
                int canal = DBContext.Programas.Where(x => x.Id == usuario.IdPrograma).Select(y => y.IdCanal).First();
                               
                List<PromocionesDTOWa> promociones = DBContext.ImagenesPromociones.Where(x =>x.IdCanal==canal && x.Tipo=="prm" &&x.FechaReg.Year==DateTime.Now.Year && x.FechaReg.Month==DateTime.Now.Month).Select(x => (PromocionesDTOWa)x).ToList();
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
