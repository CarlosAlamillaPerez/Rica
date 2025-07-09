using AutoMapper;
using bepensa_biz.Extensions;
using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_data.StoredProcedures.Models;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace bepensa_biz.Proxies
{
    public class ObjetivosProxy : ProxyBase, IObjetivo
    {
        private readonly Serilog.ILogger _logger;

        private readonly IMapper mapper;

        private readonly GlobalSettings _config;

        public ObjetivosProxy(BepensaContext context, Serilog.ILogger logger, IMapper mapper, IOptionsSnapshot<GlobalSettings> config)
        {
            DBContext = context;
            _logger = logger;
            this.mapper = mapper;
            _config = config.Value;
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
                                    .ThenInclude(x => x.IdPeriodoNavigation)
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
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "ConsultarMetaMensual(UsuarioPeriodoRequest) => IdUsuario::{usuario}", pUsuario.IdUsuario);
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
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "ConsultarMetaMensual(RequestByIdUsuario) => IdUsuario::{usuario}", pUsuario.IdUsuario);
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
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "ConsultarPortafolioPrioritario(UsuarioPeriodoRequest) => IdUsuario::{usuario}", pUsuario.IdUsuario);
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

                int idPeriodo = DBContext.Periodos
                    .First(x => x.Fecha.Year == fechaActual.Year && x.Fecha.Month == fechaActual.Month).Id;

                var consultar = ConsultarPortafolioPrioritario(pUsuario.IdUsuario, idPeriodo);

                List<PortafolioPrioritarioDTO>? portafolio = consultar
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

                if (portafolio == null || portafolio.Count == 0)
                {
                    int idCanal = DBContext.Usuarios.Where(x => x.Id == pUsuario.IdUsuario).Select(x => x.IdProgramaNavigation.IdCanal).First();

                    var pp = DBContext.SubconceptosDeAcumulacions
                        .Where(sda =>
                            sda.IdConceptoDeAcumulacionNavigation.Codigo.Equals("P3") && sda.IdConceptoDeAcumulacionNavigation.IdCanal == idCanal
                            && sda.SegmentosAcumulacions.Any(seg => sda.Id == seg.IdSda))
                        .ToList();

                    portafolio = pp
                        .Select(p => new PortafolioPrioritarioDTO
                        {
                            Id = p.Id,
                            Nombre = p.Nombre,
                            FondoColor = p.FondoColor,
                            LetraColor = p.LetraColor,
                            CumplimientoPortafolio = []
                        }).ToList();
                }

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
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "ConsultarPortafolioPrioritario(RequestByIdUsuario) => IdUsuario::{usuario}", pUsuario.IdUsuario);
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

                int idPeriodo = DBContext.Periodos
                    .First(x => x.Fecha.Year == fechaActual.Year && x.Fecha.Month == fechaActual.Month).Id;

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

                if (!portafolio.Any(x => x.IdPeriodo == idPeriodo))
                {
                    int idCanal = DBContext.Usuarios.Where(x => x.Id == pUsuario.IdUsuario).Select(x => x.IdProgramaNavigation.IdCanal).First();

                    var pp = DBContext.SubconceptosDeAcumulacions
                        .Where(sda =>
                            sda.IdConceptoDeAcumulacionNavigation.Codigo.Equals("P3") && sda.IdConceptoDeAcumulacionNavigation.IdCanal == idCanal
                            && sda.SegmentosAcumulacions.Any(seg => sda.Id == seg.IdSda))
                        .ToList();

                    var portafolioDefault = new DetallePortafolioPrioritarioDTO
                    {
                        IdPeriodo = idPeriodo,
                        Fecha = DateOnly.FromDateTime(fechaActual),
                        PortafolioPrioritario = pp.Select(sda => new PortafolioPrioritarioDTO
                        {
                            Id = sda.Id,
                            Nombre = sda.Nombre,
                            FondoColor = sda.FondoColor,
                            LetraColor = sda.LetraColor,
                            CumplimientoPortafolio = []
                        }).ToList()
                    };

                    portafolio.Add(portafolioDefault);
                }
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "ConsultarPortafoliosPrioritarios(RequestByIdUsuario) => IdUsuario::{usuario}", pUsuario.IdUsuario);
            }

            return resultado;
        }

        public Respuesta<List<PeriodosEmpaquesDTO>> ConsultarCumplimientoFotoExito(UsuarioByEmptyPeriodoRequest pUsuario)
        {
            Respuesta<List<PeriodosEmpaquesDTO>> resultado = new();

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

                var consultar = ConsultarFotoExito(pUsuario.IdUsuario, pUsuario.IdPeriodo);

                var fotoExito = consultar
                                .GroupBy(x => new
                                {
                                    x.IdPeriodo,
                                    x.Fecha
                                }).Select(detalle => new PeriodosEmpaquesDTO
                                {
                                    IdPeriodo = detalle.Key.IdPeriodo,
                                    Fecha = detalle.Key.Fecha,
                                    Categoria = detalle.GroupBy(y => new
                                    {
                                        y.IdSda,
                                        y.SubconceptoAcumulacion,
                                        y.SegAcumulacion,
                                        y.FondoColor,
                                        y.LetraColor
                                    }).Select(pp => new CategoriaEmpaqueDTO
                                    {
                                        Id = pp.Key.IdSda,
                                        Nombre = pp.Key.SegAcumulacion,
                                        //FondoColor = pp.Key.FondoColor,
                                        //LetraColor = pp.Key.LetraColor,
                                        Cumplimiento = pp.Select(cump => new CumplimientoEmpaqueDTO
                                        {
                                            Imagen = cump.Imagen,
                                            Nombre = cump.Empaques,
                                            Cumple = cump.Cumple
                                        }).ToList()
                                    }).ToList()
                                }).ToList();

                if (fotoExito == null || fotoExito.Count == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = fotoExito;

                resultado.Data.ForEach(i =>
                {
                    i.Categoria.ForEach(j =>
                    {
                        j.Porcentaje = (int)(j.Cumplimiento.Where(x => x.Cumple == true).Count() * 100 / j.Cumplimiento.Count);

                        j.Cumplimiento.ForEach(k =>
                        {
                            if (k.Imagen != null)
                                k.Imagen = $"{_config.UrlTradicional}images/foto-de-exito/{i.IdPeriodo}/{k.Imagen}";
                        });
                    });
                });

                resultado.Data.ForEach(i =>
                {
                    i.Porcentaje = (int)(i.Categoria.Sum(x => x.Porcentaje) / i.Categoria.Count);
                });
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "ConsultarCumplimientoFotoExito(UsuarioByEmptyPeriodoRequest) => IdUsuario::{usuario}", pUsuario.IdUsuario);
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

                var consultar = ConsultarMetasMensuales(pUsuario.IdUsuario);

                if (consultar == null || consultar.Count == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = consultar.OrderByDescending(x => x.IdPeriodo).Take(6).OrderBy(x => x.IdPeriodo).ToList();
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "ConsultarMetasMensuales(RequestByIdUsuario) => IdUsuario::{usuario}", pUsuario.IdUsuario);
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
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "ConsultarEjecucionTradicional(RequestByIdUsuario) => IdUsuario::{usuario}", pUsuario.IdUsuario);
            }

            return resultado;
        }

        public Respuesta<List<CumplimientoDeEnfriadorDTO>> ConsultarCumplimientosDeEnfriador(RequestByIdUsuario pUsuario)
        {
            Respuesta<List<CumplimientoDeEnfriadorDTO>> resultado = new();

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

                var consultar = DBContext.CumplimientosDeEnfriador.
                    FromSqlRaw("EXEC CumplimientosDeEnfriador @IdUsuario", idUsuario).ToList();

                var Periodos = consultar.GroupBy(g => new { g.IdPeriodo, g.NombrePeriodo }).Select(x => x.First()).ToList();

                List<CumplimientoDeEnfriadorDTO> CumplimientosDeEnfriador = new List<CumplimientoDeEnfriadorDTO>();

                foreach (var item in Periodos)
                {
                    CumplimientoDeEnfriadorDTO cumplimientoDeEnfriador = new CumplimientoDeEnfriadorDTO();
                    cumplimientoDeEnfriador.IdPeriodo = item.IdPeriodo;
                    cumplimientoDeEnfriador.NombrePeriodo = item.NombrePeriodo;
                    cumplimientoDeEnfriador.Fecha = item.Fecha;
                    cumplimientoDeEnfriador.Cumplimientos = consultar.Where(x => x.IdPeriodo == item.IdPeriodo).ToList();
                    CumplimientosDeEnfriador.Add(cumplimientoDeEnfriador);
                }

                if (consultar.Count == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = CumplimientosDeEnfriador;
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "ConsultarCumplimientosDeEnfriador(RequestByIdUsuario) => IdUsuario::{usuario}", pUsuario.IdUsuario);
            }

            return resultado;
        }


        public Respuesta<ResumenSocioSelectoDTO> ResumenSocioSelecto(LandingFDVRequest pLanding)
        {
            Respuesta<ResumenSocioSelectoDTO> resultado = new()
            {
                Data = new ResumenSocioSelectoDTO()
            };

            try
            {
                var valida = Extensiones.ValidateRequest(pLanding);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = false;

                    return resultado;
                }

                var fechaActual = DateTime.Now;

                int idPeriodo = DBContext.Periodos
                    .Where(x => x.Fecha.Year == fechaActual.Year && x.Fecha.Month == fechaActual.Month)
                    .Select(x => x.Id)
                    .First();

                var usuario = DBContext.Usuarios
                    .Include(x => x.MetasMensuales.Where(x => x.IdPeriodo == idPeriodo))
                        .ThenInclude(x => x.IdPeriodoNavigation)
                    .FirstOrDefault(x => x.Cuc.Equals(pLanding.Cuc) && x.IdEstatus == (int)TipoEstatus.Activo);

                if (usuario == null)
                {
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data.MetaMensual = mapper.Map<MetaMensualDTO>(usuario.MetasMensuales.FirstOrDefault());

                resultado.Data.PortafolioPrioritario = GetPortafolioPrioritario(usuario.Id, idPeriodo);

                resultado.Data.EstadoCuenta = GetEdoCtaEncabezados(usuario.Id, idPeriodo);
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "ResumenSocioSelecto(LandingFDVRequest) => Cuc::{usuario}", pLanding.Cuc);
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

        private List<FotoExitoCTE> ConsultarFotoExito(int pIdUsuario, int? pIdPeriodo = null)
        {
            var parametros = Extensiones.CrearSqlParametrosDelModelo(new
            {
                IdUsuario = pIdUsuario,
                IdPeriodo = pIdPeriodo
            });

            var consultar = DBContext.FotoExito
                .FromSqlRaw("EXEC ConceptosAcumulacion_ConsultarFotoExito @IdUsuario,  @IdPeriodo", parametros)
                .ToList();

            return consultar;
        }
        private List<MetaCompraDTO>? ConsultarMetasMensuales(int pIdUsuario, int? pIdPeriodo = null)
        {
            var parametros = Extensiones.CrearSqlParametrosDelModelo(new
            {
                IdUsuario = pIdUsuario,
                IdPeriodo = pIdPeriodo
            });

            var consultar = DBContext.MetaMensual
                .FromSqlRaw("EXEC ConceptosAcumulacion_ConsultarMetasMensuales @IdUsuario,  @IdPeriodo", parametros)
                .ToList();

            var result = consultar
                    .GroupBy(g => new
                    {
                        g.Id,
                        g.IdPeriodo,
                        g.Fecha,
                        g.Meta,
                        g.ImporteComprado,
                        g.CompraPreventa,
                        g.CompraDigital,
                        g.Porcentaje
                    }).Select(x => new MetaCompraDTO
                    {
                        Id = x.Key.Id,
                        IdPeriodo = x.Key.IdPeriodo,
                        Fecha = x.Key.Fecha,
                        Meta = x.Key.Meta,
                        ImporteComprado = x.Key.ImporteComprado,
                        CompraDigital = x.Key.CompraDigital,
                        Porcentaje = x.Key.Porcentaje,
                        Ventas = x.Where(x => x.VentasFechaVenta != null).Select(y => new ResumenVentaDTO
                        {
                            FechaVenta = y.VentasFechaVenta.Value.ToDateTime(TimeOnly.MinValue),
                            ImporteComprado = y.VentasImporteComprado.Value
                        }).OrderBy(x => x.FechaVenta).ToList(),
                    }).ToList();

            return result;
        }

        private ConceptosEdoCtaDTO GetEdoCtaEncabezados(int pIdUsuario, int? pIdPeriodo)
        {
            var parametros = Extensiones.CrearSqlParametrosDelModelo(new
            {
                IdUsuario = pIdUsuario,
                IdPeriodo = pIdPeriodo
            });

            var consultar = DBContext.EstadoCuentaGeneral
                .FromSqlRaw("EXEC Movimientos_ConsultarEncabezados @IdUsuario,  @IdPeriodo", parametros)
                .ToList();

            return mapper.Map<ConceptosEdoCtaDTO>(consultar.First());
        }


        private List<PortafolioPrioritarioDTO> GetPortafolioPrioritario(int pIdUsuario, int pIdPeriodo)
        {
            var consultar = ConsultarPortafolioPrioritario(pIdUsuario, pIdPeriodo);

            var portafolio = consultar
                    .GroupBy(y => new
                    {
                        y.IdSda,
                        y.SubconceptoAcumulacion,
                        y.FondoColor,
                        y.LetraColor,
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
                        }).ToList(),
                        Porcentaje = pp.Where(x => x.Cumple == true).Count() * 100 / pp.Count()
                    }).ToList();

            return portafolio;
        }
        #endregion
    }
}
