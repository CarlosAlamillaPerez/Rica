using AutoMapper;
using bepensa_biz.Extensions;
using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.Data.SqlClient;
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

                resultado.Data.Porcentaje = (int)(resultado.Data.ImporteComprado * 100 / resultado.Data.Meta);
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

                resultado.Data.Porcentaje = (int)(resultado.Data.ImporteComprado * 100 / resultado.Data.Meta);
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
                                     on sca.Id equals sa.IdSda
                                  join emp in DBContext.Empaques
                                      on sa.Id equals emp.IdSegAcumulacion
                                  join demp in DBContext.CumplimientosPortafolios
                                      on emp.Id equals demp.IdEmpaque
                                  where demp.IdUsuario == pUsuario.IdUsuario
                                      && emp.IdPeriodo == pUsuario.IdPeriodo
                                  orderby sca.Orden
                                  group new { sca, emp, demp } by new
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
                                     on sca.Id equals sa.IdSda
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
                                      CumplimientoPortafolio = g
                                          .Select(x => new CumplimientoPortafolioDTO
                                          {
                                              IdEmpaque = x.emp.Id,
                                              Nombre = x.emp.Nombre,
                                              Cumple = x.demp.Cumple,
                                          })
                                          .GroupBy(x => x.IdEmpaque)
                                          .Select(grp => grp.First())
                                          .ToList(),
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

        public Respuesta<List<DetallePortafolioPrioritarioDTO>> ConsultarPortafoliosPrioritarios(RequestByIdUsuario pUsuario)
        {
            Respuesta<List<DetallePortafolioPrioritarioDTO>> resultado = new();

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

                fechaActual = new DateTime(fechaActual.Year, fechaActual.Month, 1);

                var fechaInicio = fechaActual.AddMonths(-6);

                var portafolio = (from per in DBContext.Periodos
                                  join emp in DBContext.Empaques
                                     on per.Id equals emp.IdPeriodo
                                  join cump in DBContext.CumplimientosPortafolios
                                      on emp.Id equals cump.IdEmpaque
                                  where per.Fecha >= DateOnly.FromDateTime(fechaInicio)
                                     && per.Fecha <= DateOnly.FromDateTime(fechaActual)
                                     && cump.IdUsuario == pUsuario.IdUsuario
                                     && emp.IdSegAcumulacionNavigation.IdSdaNavigation.IdConceptoDeAcumulacionNavigation.Codigo.Equals(TipoConceptoAcumulacion.PortafolioPrioritario.GetDisplayName())
                                  group new { per, emp, cump } by new
                                  {
                                      per.Id,
                                      per.Fecha
                                  } into g
                                  orderby g.Key.Fecha
                                  select new DetallePortafolioPrioritarioDTO
                                  {
                                      IdPeriodo = g.Key.Id,
                                      Fecha = g.Key.Fecha,
                                      PortafolioPrioritario =
                                        g.GroupBy(pp => new
                                        {
                                            pp.emp.IdSegAcumulacionNavigation.IdSdaNavigation.IdConceptoDeAcumulacion,
                                            pp.emp.IdSegAcumulacionNavigation.IdSdaNavigation.Nombre,
                                            pp.emp.IdSegAcumulacionNavigation.IdSdaNavigation.FondoColor,
                                            pp.emp.IdSegAcumulacionNavigation.IdSdaNavigation.LetraColor,
                                            pp.emp.IdSegAcumulacionNavigation.IdSdaNavigation.Orden
                                        })
                                        .OrderBy(grupo => grupo.Key.Orden)
                                      .Select(grupo => new PortafolioPrioritarioDTO
                                      {
                                          Id = grupo.Key.IdConceptoDeAcumulacion,
                                          IdConceptoDeAcumulacion = grupo.Key.IdConceptoDeAcumulacion,
                                          Nombre = grupo.Key.Nombre,
                                          FondoColor = grupo.Key.FondoColor,
                                          LetraColor = grupo.Key.LetraColor,
                                          CumplimientoPortafolio = grupo
                                          .Select(x => new CumplimientoPortafolioDTO
                                          {
                                              IdEmpaque = x.emp.Id,
                                              Nombre = x.emp.Nombre,
                                              Cumple = x.cump.Cumple
                                          })
                                          .GroupBy(x => x.IdEmpaque)
                                          .Select(grp => grp.First())
                                          .ToList()
                                      }).ToList()
                                  }
                                 ).ToList();

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
                    i.PortafolioPrioritario.ForEach(j =>
                    {
                        j.Porcentaje = (int)(j.CumplimientoPortafolio.Where(x => x.Cumple == true).Count() * 100 / j.CumplimientoPortafolio.Count);
                    });
                });

                resultado.Data.ForEach(i =>
                {
                    i.Porcentaje = (int)(i.PortafolioPrioritario.Sum(x => x.Porcentaje) / i.PortafolioPrioritario.Count);
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

        public Respuesta<List<MetaCompraDTO>> ConsultarMetasMensuales(RequestByIdUsuario pUsuario)
        {
            Respuesta<List<MetaCompraDTO>> resultado = new();

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

                fechaActual = new DateTime(fechaActual.Year, fechaActual.Month, 1);

                var fechaInicio = fechaActual.AddMonths(-6);



                var consultar = (from mm in DBContext.MetasMensuales
                                 join ven in DBContext.Ventas
                                  on new { mm.IdUsuario, mm.IdPeriodo } equals new { ven.IdUsuario, ven.IdPeriodo } into ventasJoin
                                 from ven in ventasJoin.DefaultIfEmpty()
                                 join dven in DBContext.DetalleVentas
                                 on ven.Id equals dven.IdVenta into detalleVJoin
                                 from dven in detalleVJoin.DefaultIfEmpty()
                                 where mm.IdUsuario == pUsuario.IdUsuario
                                    && mm.IdPeriodoNavigation.Fecha >= DateOnly.FromDateTime(fechaInicio)
                                    && mm.IdPeriodoNavigation.Fecha <= DateOnly.FromDateTime(fechaActual)
                                 group new { mm, ven, dven } by new
                                 {
                                     mm.Id,
                                     mm.IdPeriodo,
                                     mm.IdPeriodoNavigation.Fecha,
                                     mm.Meta,
                                     mm.ImporteComprado,
                                     mm.CompraPreventa,
                                     mm.CompraDigital
                                 } into g
                                 select new MetaCompraDTO
                                 {
                                     Id = g.Key.Id,
                                     IdPeriodo = g.Key.IdPeriodo,
                                     Fecha = g.Key.Fecha,
                                     Meta = g.Key.Meta,
                                     ImporteComprado = g.Key.ImporteComprado,
                                     CompraPreventa = g.Key.CompraPreventa,
                                     CompraDigital = g.Key.CompraDigital,
                                     Porcentaje = (int)(g.Key.ImporteComprado * 100 / g.Key.Meta),
                                     Ventas = g.Any(x => x.ven != null) ? g.Where(v => v.ven != null)
                                     .GroupBy(v => new { v.ven.Id, v.ven.FechaVenta })
                                        .Select(v => new ResumenVentaDTO
                                        {
                                            Id = v.Key.Id,
                                            FechaVenta = v.Key.FechaVenta,
                                            ImporteComprado = v.Sum(x => x.dven != null ? x.dven.Importe : 0)
                                        }).ToList()
                                        : null
                                 }
                               ).ToList();

                if (consultar.Count == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = consultar;
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public Respuesta<List<EjecucionDTO>> ConsultarEjecucionTradicional(RequestByIdUsuario pUsuario)
        {
            Respuesta<List<EjecucionDTO>> resultado = new();

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
                var idUsuario = new SqlParameter("@IdUsuario", pUsuario.IdUsuario);

                var consultar = DBContext.EjecuionTradicional.FromSqlRaw("EXEC EvaluacionesAcumulacion_ConsultarEjecucionTR @IdUsuario", idUsuario).ToList();

                if (consultar == null || consultar.Count == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = mapper.Map<List<EjecucionDTO>>(consultar);
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
