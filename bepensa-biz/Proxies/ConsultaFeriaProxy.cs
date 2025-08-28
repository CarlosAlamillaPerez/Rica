using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.ApiWa;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bepensa_biz.Proxies
{
    public class ConsultaFeriaProxy : ProxyBase, IConsultasFeriaProxy
    {
        private readonly IConfiguration _configuration;
        public ConsultaFeriaProxy(BepensaContext Context, IConfiguration Configuracion)
        {
            DBContext = Context;
            _configuration = Configuracion;
        }

        public Respuesta<ResponseFerias> ConsultaCliente(RequestCliente data)
        {
            Respuesta<ResponseFerias> resultado = new();
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
                Periodo periodo = DBContext.Periodos.FirstOrDefault(x => x.Fecha.Month == DateTime.Now.Month && x.Fecha.Year == DateTime.Now.Year);
                Programa programa = DBContext.Programas.FirstOrDefault(x => x.Id == usuario.IdPrograma);
                ImagenesPromocione imgpromocion = DBContext.ImagenesPromociones.FirstOrDefault(x => x.IdCanal == programa.IdCanal && x.IdPeriodo==periodo.Id);

                resultado.Data.id = imgpromocion.Id;
                resultado.Data.codigo = imgpromocion.Tipo;
                resultado.Data.nombre = imgpromocion.Nombre;
                resultado.Data.descripcion = imgpromocion.Nombre;
                resultado.Data.urlImagen = imgpromocion.Url;

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
