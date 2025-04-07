using AutoMapper;
using bepensa_biz.Extensions;
using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.EntityFrameworkCore;

namespace bepensa_biz.Proxies
{
    public class ObjetivosProxy : ProxyBase, IObjetivo
    {
        private readonly IMapper mapper;

        public ObjetivosProxy(BepensaContext context, IMapper mapper)
        {
            DBContext = context;
            this.mapper = mapper;
        }

        public Respuesta<MetaMensualDTO> ConsultarMetaMensual(UsuarioPeriodoRequest pUsuario)
        {
            Respuesta<MetaMensualDTO> resultado = new();

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

                var valiadPeriodo = DBContext.Periodos.Any(x => x.Id == pUsuario.IdPeriodo);

                if (!valiadPeriodo)
                {
                    resultado.Codigo = (int)CodigoDeError.PeriodoInvalido;
                    resultado.Mensaje = CodigoDeError.PeriodoInvalido.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var usuario = DBContext.Usuarios
                                .Include(x => x.MetasMensuales.Where(x => x.IdPeriodo == pUsuario.IdPeriodo))
                                .FirstOrDefault(x => x.Id == pUsuario.IdUsuario);

                if (usuario == null)
                {
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (usuario.MetasMensuales.Count == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = mapper.Map<MetaMensualDTO>(usuario.MetasMensuales.First());
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public Respuesta<MetaMensualDTO> ConsultarMetaMensual(RequestByIdUsuario pUsuario)
        {
            Respuesta<MetaMensualDTO> resultado = new();

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

                var fechaActual = DateTime.Now;

                var usuario = DBContext.Usuarios
                                .Include(x => x.MetasMensuales
                                    .Where(x => x.IdPeriodoNavigation.Fecha.Year == fechaActual.Year
                                            && x.IdPeriodoNavigation.Fecha.Month == fechaActual.Month))
                                .FirstOrDefault(x => x.Id == pUsuario.IdUsuario);

                if (usuario == null)
                {
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (usuario.MetasMensuales.Count == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = mapper.Map<MetaMensualDTO>(usuario.MetasMensuales.First());
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public Respuesta<List<PortafolioPrioritarioDTO>> ConsultarPortafolioPrioritario(UsuarioPeriodoRequest pUsuario)
        {
            Respuesta<List<PortafolioPrioritarioDTO>> resultado = new();

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

                var valiadPeriodo = DBContext.Periodos.Any(x => x.Id == pUsuario.IdPeriodo);

                if (!valiadPeriodo)
                {
                    resultado.Codigo = (int)CodigoDeError.PeriodoInvalido;
                    resultado.Mensaje = CodigoDeError.PeriodoInvalido.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var usuario = DBContext.Usuarios
                                .FirstOrDefault(x => x.Id == pUsuario.IdUsuario);

                if (usuario == null)
                {
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var portafolio = (from sca in DBContext.SubconceptosDeAcumulacions
                                 join sa in DBContext.SegmentosAcumulacions
                                    on sca.Id equals sa.IdSubcptoAcumulacon
                                 join emp in DBContext.Empaques
                                     on sa.Id equals emp.IdSegAcumulacion
                                 join demp in DBContext.CumplimientosPortafolios
                                     on emp.Id equals demp.IdEmpaque
                                 where demp.IdUsuario == pUsuario.IdUsuario
                                     && emp.IdPeriodo == pUsuario.IdPeriodo
                                    group new { sca, emp, demp} by new
                                    {
                                        sca.Id,
                                        sca.IdConceptoDeAcumulacion,
                                        sca.Nombre
                                    } into g
                                select new PortafolioPrioritarioDTO
                                {
                                    Id = g.Key.Id,
                                    IdConceptoDeAcumulacion = g.Key.IdConceptoDeAcumulacion,
                                    Nombre = g.Key.Nombre,
                                    CumplimientoPortafolio = g.SelectMany(x => x.emp.CumplimientosPortafolios).Select(cump => new CumplimientoPortafolioDTO
                                    {
                                        IdEmpaque = cump.IdEmpaque,
                                        Nombre = cump.IdEmpaqueNavigation.Nombre,
                                        Cumple = cump.Cumple
                                    }).Distinct().ToList()
                                }).ToList();

                if (portafolio == null)
                {
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = portafolio;

                resultado.Data.ForEach(i =>
                {
                    i.Porcentaje = (int)(i.CumplimientoPortafolio.Where(x => x.Cumple == true).Count() * 100 / i.CumplimientoPortafolio.Count);
                });
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public Respuesta<List<PortafolioPrioritarioDTO>> ConsultarPortafolioPrioritario(RequestByIdUsuario pUsuario)
        {
            Respuesta<List<PortafolioPrioritarioDTO>> resultado = new();

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

                var usuario = DBContext.Usuarios.Any(x => x.Id == pUsuario.IdUsuario);

                if (!usuario)
                {
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var fechaActual = DateTime.Now;

                var portafolio = (from sca in DBContext.SubconceptosDeAcumulacions
                                  join sa in DBContext.SegmentosAcumulacions
                                     on sca.Id equals sa.IdSubcptoAcumulacon
                                  join emp in DBContext.Empaques
                                      on sa.Id equals emp.IdSegAcumulacion
                                  join demp in DBContext.CumplimientosPortafolios
                                      on emp.Id equals demp.IdEmpaque
                                  where demp.IdUsuario == pUsuario.IdUsuario
                                      && emp.IdPeriodoNavigation.Fecha.Year == fechaActual.Year
                                      && emp.IdPeriodoNavigation.Fecha.Month == fechaActual.Month
                                  group new { sca, emp, demp } by new
                                  {
                                      sca.Id,
                                      sca.IdConceptoDeAcumulacion,
                                      sca.Nombre,
                                      sca.FondoColor,
                                      sca.LetraColor
                                  } into g
                                  select new PortafolioPrioritarioDTO
                                  {
                                      Id = g.Key.Id,
                                      IdConceptoDeAcumulacion = g.Key.IdConceptoDeAcumulacion,
                                      Nombre = g.Key.Nombre,
                                      CumplimientoPortafolio = g.SelectMany(x => x.emp.CumplimientosPortafolios).Select(cump => new CumplimientoPortafolioDTO
                                      {
                                          IdEmpaque = cump.IdEmpaque,
                                          Nombre = cump.IdEmpaqueNavigation.Nombre,
                                          Cumple = cump.Cumple
                                      }).Distinct().ToList(),
                                      FondoColor = g.Key.FondoColor,
                                      LetraColor = g.Key.LetraColor
                                  }).ToList();

                if (portafolio == null)
                {
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = portafolio;

                resultado.Data.ForEach(i =>
                {
                    i.Porcentaje = (int)(i.CumplimientoPortafolio.Where(x => x.Cumple == true).Count() * 100 / i.CumplimientoPortafolio.Count);
                });
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
