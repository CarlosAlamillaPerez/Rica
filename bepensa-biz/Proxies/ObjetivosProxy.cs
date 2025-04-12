using AutoMapper;
using bepensa_biz.Extensions;
using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_data.StoredProcedures.Models;
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

                var consultar = ConsultarPortafolioPrioritario(pUsuario.IdUsuario, pUsuario.IdPeriodo);

                var portafolio = consultar
                    .GroupBy(y => new
                    {
                        y.IdSda,
                        y.SubconceptoAcumulacion,
                        y.FondoColor,
                        y.LetraColor
                    }).Select(pp => new PortafolioPrioritarioDTO
                    {
                        Id = pp.Key.IdSda,
                        Nombre = pp.Key.SubconceptoAcumulacion,
                        FondoColor = pp.Key.FondoColor,
                        LetraColor = pp.Key.LetraColor,
                        CumplimientoPortafolio = pp.Select(cump => new CumplimientoPortafolioDTO
                        {
                            Nombre = cump.Empaques,
                            Cumple = cump.Cumple
                        }).ToList()
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

                var idPeriodo = DBContext.Periodos
                    .FirstOrDefault(x => x.Fecha.Year == fechaActual.Year && x.Fecha.Month == fechaActual.Month)?.Id
                        ?? throw new Exception(TipoExcepcion.PeriodoNoIdentificado.GetDescription());

                var consultar = ConsultarPortafolioPrioritario(pUsuario.IdUsuario, idPeriodo);

                var portafolio = consultar
                    .GroupBy(y => new
                    {
                        y.IdSda,
                        y.SubconceptoAcumulacion,
                        y.FondoColor,
                        y.LetraColor
                    }).Select(pp => new PortafolioPrioritarioDTO
                    {
                        Id = pp.Key.IdSda,
                        Nombre = pp.Key.SubconceptoAcumulacion,
                        FondoColor = pp.Key.FondoColor,
                        LetraColor = pp.Key.LetraColor,
                        CumplimientoPortafolio = pp.Select(cump => new CumplimientoPortafolioDTO
                        {
                            Nombre = cump.Empaques,
                            Cumple = cump.Cumple
                        }).ToList()
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

                var consultar = ConsultarPortafolioPrioritario(pUsuario.IdUsuario);

                var portafolio = consultar
                                .GroupBy(x => new
                                {
                                    x.IdPeriodo,
                                    x.Fecha
                                }).Select(detalle => new DetallePortafolioPrioritarioDTO
                                {
                                    IdPeriodo = detalle.Key.IdPeriodo,
                                    Fecha = detalle.Key.Fecha,
                                    PortafolioPrioritario = detalle.GroupBy(y => new
                                    {
                                        y.IdSda,
                                        y.SubconceptoAcumulacion,
                                        y.FondoColor,
                                        y.LetraColor
                                    }).Select(pp => new PortafolioPrioritarioDTO
                                    {
                                        Id = pp.Key.IdSda,
                                        Nombre = pp.Key.SubconceptoAcumulacion,
                                        FondoColor = pp.Key.FondoColor,
                                        LetraColor = pp.Key.LetraColor,
                                        CumplimientoPortafolio = pp.Select(cump => new CumplimientoPortafolioDTO
                                        {
                                            Nombre = cump.Empaques,
                                            Cumple = cump.Cumple
                                        }).ToList()
                                    }).ToList()
                                }).ToList();

                if (portafolio == null || portafolio.Count == 0)
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


        #region Accesos para Stored Procedure
        private List<PortafolioPrioritarioCTE> ConsultarPortafolioPrioritario(int pIdUsuario, int? pIdPeriodo = null)
        {
            var parametros = Extensiones.CrearSqlParametrosDelModelo(new
            {
                IdUsuario = pIdUsuario,
                IdPeriodo = pIdPeriodo
            });

            var consultar = DBContext.PortafolioPrioritario
                .FromSqlRaw("EXEC ConceptosAcumulacion_ConsultarPortafolioPrioritario @IdUsuario,  @IdPeriodo", parametros)
                .ToList();

            return consultar;
        }
        #endregion
    }
}
