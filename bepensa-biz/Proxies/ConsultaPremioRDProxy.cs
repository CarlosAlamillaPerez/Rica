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
    public class ConsultaPremioRDProxy : ProxyBase, IConsultasPremiosRDProxy

    {
        private readonly IConfiguration _configuration;
        public ConsultaPremioRDProxy(BepensaRD_Context Context, IConfiguration Configuracion)
        {
            DBContextRD = Context;
            _configuration = Configuracion;
        }


        public Respuesta<List<PremioRDDTOWa>> PremiosSugeridosRD(RequestCliente data)
        {
            Respuesta<List<PremioRDDTOWa>> resultado = new();
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
                if (usuario.Saldos.Count() == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.SaldoInsuficiente;
                    resultado.Mensaje = CodigoDeError.SaldoInsuficiente.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                int disponible = usuario.Saldos == null ? 0 :usuario.Saldos.Where(x => x.IdUsuario==usuario.Id).OrderByDescending(x => x.Id).Select(x => x.Saldo1).First();

                int ptsPremio = DBContextRD.Premios.Where(x=>x.IdEstatus==(int)TipoDeEstatus.Activo).Select(x => x.Puntos).Min();

                if (disponible == 0 | disponible < ptsPremio)
                {
                    resultado.Codigo = (int)CodigoDeError.SaldoInsuficiente;
                    resultado.Mensaje = CodigoDeError.SaldoInsuficiente.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }
                List<PremioRDDTOWa> _premioSugerido = DBContextRD.Premios.Where(x => x.Puntos <= disponible && x.IdEstatus == (int)TipoDeEstatus.Activo).OrderByDescending(x => x.Puntos).Take(3).Select(x => (PremioRDDTOWa)x).ToList();
                resultado.Data = _premioSugerido;


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

        public Respuesta<List<PremioRDDTOWa>> PremiosCanjeadosRD(RequestClienteCanje data)
        {
            Respuesta<List<PremioRDDTOWa>> resultado = new();
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

                if (usuario.Redenciones.Count() == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.SinResultados;
                    resultado.Mensaje = CodigoDeError.SinResultados.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                List<int> premios = usuario.Redenciones.Where(x => x.FechaReg.Year == data.Anio && x.FechaReg.Month == data.Mes).Select(y => y.IdPremio).ToList();

                List<PremioRDDTOWa> premioscanjeados = DBContextRD.Premios.Where(x => premios.Contains(x.Id)).Select(x => (PremioRDDTOWa)x).ToList();

                resultado.Data = premioscanjeados;

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

        public Respuesta<List<PremioRDDTOWa>> PremioCanjeadoDetalleRD(RequestClienteCanjeDetalle data)
        {
            Respuesta<List<PremioRDDTOWa>> resultado = new();
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
                if (usuario.Redenciones.Count() == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.SinResultados;
                    resultado.Mensaje = CodigoDeError.SinResultados.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                Redencione redenciones = DBContextRD.Redenciones.FirstOrDefault(x => x.FolioRms == data.folio && x.IdUsuario == usuario.Id);

                if (!DBContextRD.Redenciones.Any(x => x.FolioRms == data.folio))
                {
                    resultado.Codigo = (int)CodigoDeError.SinResultados;
                    resultado.Mensaje = CodigoDeError.SinResultados.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                List<PremioRDDTOWa> premios = DBContextRD.Premios.Where(x => x.Id == redenciones.IdPremio).Select(x => (PremioRDDTOWa)x).ToList();

                resultado.Data = premios;

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