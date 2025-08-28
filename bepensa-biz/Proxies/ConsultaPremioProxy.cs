using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.ApiWa;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.Extensions.Configuration;
using System.IO.Pipelines;

namespace bepensa_biz.Proxies
{
    public class ConsultaPremioProxy : ProxyBase, IConsultasPremiosProxy

    {
        private readonly IConfiguration _configuration;
        public ConsultaPremioProxy(BepensaContext Context, IConfiguration Configuracion)
        {
            DBContext = Context;
            _configuration = Configuracion;
        }


        public Respuesta<ResponseConsultaPremiosSugeridos> PremiosSugeridos(RequestCliente data)
        {
            Respuesta<ResponseConsultaPremiosSugeridos> resultado = new();
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

                if (usuario.Movimientos.Count() == null)
                {
                    resultado.Codigo = (int)CodigoDeError.SaldoInsuficiente;
                    resultado.Mensaje = CodigoDeError.SaldoInsuficiente.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                var disponible = DBContext.Movimientos.Where(x => x.IdUsuario == usuario.Id).OrderByDescending(x => x.Id).Select(x => x.Saldo).First();

                var ptsPremio = DBContext.Premios.Where(x => x.IdEstatus == (int)TipoDeEstatus.Activo).Select(x => x.Puntos).Min();

                if (disponible == 0 | disponible < ptsPremio)
                {
                    resultado.Codigo = (int)CodigoDeError.SaldoInsuficiente;
                    resultado.Mensaje = CodigoDeError.SaldoInsuficiente.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }
                //List<PremioDTOWa> _premioSugerido = DBContext.Premios.Where(x => x.Puntos <= disponible && x.IdEstatus == (int)TipoDeEstatus.Activo).OrderByDescending(x => x.Puntos).Take(3).Select(x => (PremioDTOWa)x).ToList();

                //_premioSugerido.ForEach(x =>
                //{
                //    string imagencadena = bool.Parse(_configuration["Production"].ToString()) ? _configuration["SettingsRecursosMedia:PRODUrl"] : _configuration["SettingsRecursosMedia:QAUrl"];
                //    x.url= imagencadena + x.url;
                //});
                //resultado.Data

                //resultado.Data = _premioSugerido;

                List<PremioDTOWa> lista = DBContext.Premios.Select(x => (PremioDTOWa)x).ToList();

                lista.ForEach(x =>
                {
                    string imagencadena = bool.Parse(_configuration["Production"].ToString()) ? _configuration["SettingsRecursosMedia:PRODUrl"] : _configuration["SettingsRecursosMedia:QAUrl"];                    
                    x.urlImagen = imagencadena + x.urlImagen;
                    x.urlPaginaDetalle = "https://qa-web.socioselecto-bepensa.com";
                });

                resultado.Data.PremiosSugeridos = lista.Where(x => x.Puntos <= disponible && x.IdEstatus == (int)TipoDeEstatus.Activo).OrderByDescending(x => x.Puntos).Take(3).ToList();



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

        public Respuesta<List<PremioDTOWa>> PremiosCanjeados(RequestClienteCanje data)
        {
            Respuesta<List<PremioDTOWa>> resultado = new();
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
                
                if (usuario.Redenciones.Count() == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.SinResultados;
                    resultado.Mensaje = CodigoDeError.SinResultados.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                List<int> premios = usuario.Redenciones.Where(x=>x.FechaReg.Year==data.Anio && x.FechaReg.Month==data.Mes).Select(y => y.IdPremio).ToList();

                List<PremioDTOWa> premioscanjeados = DBContext.Premios.Where(x => premios.Contains(x.Id)).Select(x => (PremioDTOWa)x).ToList();

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

        public Respuesta<List<PremioDTOWa>> PremioCanjeadoDetalle(RequestClienteCanjeDetalle data)
        {
            Respuesta<List<PremioDTOWa>> resultado = new();
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
                
                if (usuario.Redenciones.Count() == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.SinResultados;
                    resultado.Mensaje = CodigoDeError.SinResultados.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }


                Redencione redenciones = DBContext.Redenciones.FirstOrDefault(x => x.FolioRms == data.folio && x.IdUsuario == usuario.Id);

                if (!DBContext.Redenciones.Any(x => x.FolioRms == data.folio))
                {
                    resultado.Codigo = (int)CodigoDeError.SinResultados;
                    resultado.Mensaje = CodigoDeError.SinResultados.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                List<PremioDTOWa> premios = DBContext.Premios.Where(x =>x.Id==redenciones.IdPremio).Select(x => (PremioDTOWa)x).ToList();

                resultado.Data=premios;

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