using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.ApiWa;
using bepensa_models.DTO;
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
    public class ConsultaProgramaProxy:ProxyBase,IConsultasProgramasProxy
    {
        private readonly IConfiguration _configuration;
        public ConsultaProgramaProxy(BepensaContext Context, IConfiguration Configuracion)
        {
            DBContext = Context;
            _configuration = Configuracion;
        }

        public Respuesta<List<SubConceptosDeAcumulacionDTO>> ConsultaBonos(RequestCliente data)
        {
            Respuesta<List<SubConceptosDeAcumulacionDTO>> resultado = new();
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
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                Usuario usuario = DBContext.Usuarios.FirstOrDefault(x => x.Cuc == data.Cliente);//variable se guarda lo de usuario                
                int canal = DBContext.Programas.Where(x => x.Id == usuario.IdPrograma).Select(y => y.IdCanal).First();

                var array = new[] { "Bonos" };
                List<int> listbonos = DBContext.ConceptosDeAcumulacions.Where(x => array.Any(y => x.Nombre == y) && x.IdTipoDeMovimiento == 1 && x.IdCanal == canal).Select(y => y.Id).ToList();

                List<SubConceptosDeAcumulacionDTO> bonos = DBContext.SubconceptosDeAcumulacions.Where(x => listbonos.Contains(x.IdConceptoDeAcumulacion)).Select(x => (SubConceptosDeAcumulacionDTO)x).ToList();
                resultado.Data = bonos;

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

        public Respuesta<List<OrigeneDTOWA>> ConsultaCanalesComunicacion(RequestCliente data)
        {
            Respuesta<List<OrigeneDTOWA>> resultado = new();
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
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                Usuario usuario = DBContext.Usuarios.FirstOrDefault(x => x.Cuc == data.Cliente);//variable se guarda lo de usuario
                List<OrigeneDTOWA> canales = DBContext.Origenes.Select(x => (OrigeneDTOWA)x).ToList();
                resultado.Data = canales;

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

        public Respuesta<List<SubConceptosDeAcumulacionDTO>> ConsultaMecanicas(RequestCliente data)
        {
            Respuesta<List<SubConceptosDeAcumulacionDTO>> resultado = new();
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
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                Usuario usuario = DBContext.Usuarios.FirstOrDefault(x => x.Cuc == data.Cliente);//variable se guarda lo de usuario                
                int canal = DBContext.Programas.Where(x => x.Id == usuario.IdPrograma).Select(y=>y.IdCanal).First();

                var array = new[] { "Bonos"};
                List<int> listacumulacion = DBContext.ConceptosDeAcumulacions.Where(x=>!array.Any(y=>x.Nombre==y) && x.IdTipoDeMovimiento == 1 && x.IdCanal==canal).Select(y => y.Id).ToList();

                List<SubConceptosDeAcumulacionDTO> mecanicas = DBContext.SubconceptosDeAcumulacions.Where(x=>listacumulacion.Contains(x.IdConceptoDeAcumulacion)).Select(x => (SubConceptosDeAcumulacionDTO)x).ToList();
                resultado.Data = mecanicas;

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

        public Respuesta<List<SubConceptosDeAcumulacionDTO>> ConsultaVigencia(RequestCliente data)
        {
            Respuesta<List<SubConceptosDeAcumulacionDTO>> resultado = new();
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
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                Usuario usuario = DBContext.Usuarios.FirstOrDefault(x => x.Cuc == data.Cliente);//variable se guarda lo de usuario                
                int canal = DBContext.Programas.Where(x => x.Id == usuario.IdPrograma).Select(y => y.IdCanal).First();

                //var array = new[] { "Bonos" };
                List<int> listcanjenegativo = DBContext.ConceptosDeAcumulacions.Where(x => x.IdTipoDeMovimiento == 2 && x.IdCanal == canal).Select(y => y.Id).ToList();

                List<SubConceptosDeAcumulacionDTO> canjes = DBContext.SubconceptosDeAcumulacions.Where(x => listcanjenegativo.Contains(x.IdConceptoDeAcumulacion)).Select(x => (SubConceptosDeAcumulacionDTO)x).ToList();
                resultado.Data = canjes;

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
