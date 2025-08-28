using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_models.ApiWa;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.Extensions.Configuration;
using bepensa_data.modelsRD;

namespace bepensa_biz.Proxies
{
    public class ConsultaProgramaRDProxy : ProxyBase, IConsultasProgramasRDProxy
    {
        private readonly IConfiguration _configuration;
        public ConsultaProgramaRDProxy(BepensaRD_Context Context, IConfiguration Configuracion)
        {
            DBContextRD = Context;
            _configuration = Configuracion;
        }

        public Respuesta<List<ConceptosDeAcumulacionRDDTOWa>> ConsultaBonos(RequestCliente data)
        {
            Respuesta<List<ConceptosDeAcumulacionRDDTOWa>> resultado = new();
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

                var array = new[] { "Bonos" };
                List<int> listbonos = DBContextRD.ConceptosDeAcumulacions.Where(x => array.Any(y => x.Nombre == y)).Select(y => y.Id).ToList();


                List<ConceptosDeAcumulacionRDDTOWa> bonos = DBContextRD.SubconceptosDeAcumulacions.Where(x => listbonos.Contains(x.IdConceptoDeAcumulacion)).Select(x => (ConceptosDeAcumulacionRDDTOWa)x).ToList();
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

        public Respuesta<List<OrigeneRDDTOWa>> ConsultaCanalesComunicacion(RequestCliente data)
        {
            Respuesta<List<OrigeneRDDTOWa>> resultado = new();
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
                List<OrigeneRDDTOWa> canales = DBContextRD.Origenes.Select(x => (OrigeneRDDTOWa)x).ToList();
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

        public Respuesta<List<ConceptosDeAcumulacionRDDTOWa>> ConsultaMecanicas(RequestCliente data)
        {
            Respuesta<List<ConceptosDeAcumulacionRDDTOWa>> resultado = new();
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

                var array = new[] { "Bonos" };
                List<int> positivos = DBContextRD.ConceptosDeAcumulacions.Where(x => !array.Any(y => x.Nombre == y) && x.IdTipoDeMovimiento==1).Select(y => y.Id).ToList();

                List<ConceptosDeAcumulacionRDDTOWa> mecanicas = DBContextRD.SubconceptosDeAcumulacions.Where(x => positivos.Contains(x.IdConceptoDeAcumulacion)).Select(x => (ConceptosDeAcumulacionRDDTOWa)x).ToList();                
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

        public Respuesta<List<ConceptosDeAcumulacionRDDTOWa>> ConsultaVigencia(RequestCliente data)
        {
            Respuesta<List<ConceptosDeAcumulacionRDDTOWa>> resultado = new();
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

                //var array = new[] { "Bonos" };
                List<int> negativos = DBContextRD.ConceptosDeAcumulacions.Where(x => x.IdTipoDeMovimiento == 2).Select(y => y.Id).ToList();

                List<ConceptosDeAcumulacionRDDTOWa> canjes = DBContextRD.SubconceptosDeAcumulacions.Where(x => negativos.Contains(x.IdConceptoDeAcumulacion)).Select(x => (ConceptosDeAcumulacionRDDTOWa)x).ToList();
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
