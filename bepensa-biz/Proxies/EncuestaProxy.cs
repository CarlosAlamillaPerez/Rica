using AutoMapper;
using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.DTO;
using bepensa_models.DataModels;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.EntityFrameworkCore;
using bepensa_biz.Extensions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Net.NetworkInformation;

namespace bepensa_biz.Proxies
{
    public class EncuestaProxy : ProxyBase, IEncuesta
    {
        private readonly IMapper mapper;
        public EncuestaProxy(BepensaContext context, IMapper mapper)
        {
            DBContext = context;
            this.mapper = mapper;
        }

        public Respuesta<List<BitacoraEncuestaDTO>> ConsultarEncuestas(int pIdUsuario)
        {
            Respuesta<List<BitacoraEncuestaDTO>> resultado = new();

            try
            {
                //var consultar = DBContext.BitacoraDeEncuesta
                //    .Where(x =>
                //        //x.IdEncuestaNavigation.BitacoraDeEncuesta.Any(y => y.Id == x.Id)
                //        x.IdUsuario == pIdUsuario
                //        && x.IdEstatus == (int)TipoDeEstatus.Activo && !x.Contestada)
                //    .Include(x => x.IdEncuestaNavigation)
                //        .ThenInclude(x => x.BitacoraDeEncuesta)
                //    .Include(x => x.IdEncuestaNavigation)
                //        .ThenInclude(x => x.PreguntasEncuesta)
                //            .ThenInclude(x => x.IdTipoPreguntaNavigation)
                //    .Include(x => x.IdEncuestaNavigation)
                //        .ThenInclude(x => x.PreguntasEncuesta)
                //            .ThenInclude(x => x.OpcionesPreguntumIdPreguntaNavigations)
                //            .ThenInclude(x => x.IdTipoControlNavigation)
                //    .Select(x => x)
                //    .ToList();

                var consultar = GetEncuestas(pIdUsuario);


                resultado.Data = mapper.Map<List<BitacoraEncuestaDTO>>(consultar);

            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public Respuesta<Empty> ResponderEncuesta(EncuestaRequest pEncuesta, int idOrigen)
        {
            Respuesta<Empty> resultado = new();

            try
            {
                var valida = Extensiones.ValidateRequest(pEncuesta);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;

                    return resultado;
                }

                var bde = DBContext.BitacoraDeEncuesta.Include(x => x.IdUsuarioNavigation).Include(x => x.IdEncuestaNavigation).Where(x => x.IdUsuario == pEncuesta.IdUsuario && x.Id == pEncuesta.IdBitacoraEncuesta).FirstOrDefault();

                if (bde == null)
                {
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (bde.Contestada)
                {
                    resultado.Codigo = (int)CodigoDeError.EncuestaRespondida;
                    resultado.Mensaje = CodigoDeError.EncuestaRespondida.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var fechaActual = DateTime.Now;

                var encuesta = bde.IdEncuestaNavigation;

                foreach (var respuesta in pEncuesta.Preguntas)
                {
                    foreach (var opcion in respuesta.Opciones)
                    {
                        bde.RespuestasEncuesta.Add(new RespuestasEncuestum
                        {
                            IdBitacoraEncuesta = pEncuesta.IdBitacoraEncuesta,
                            IdOrigen = idOrigen,
                            IdPregunta = respuesta.IdPregunta,
                            IdOpcionPregunta = opcion.IdOpcion,
                            Texto = opcion.Texto,
                            FechaReg = fechaActual,
                        });
                    }
                }

                bde.NoContestaciones++;

                bde.IdUsuarioNavigation?.BitacoraDeUsuarios.Add(new BitacoraDeUsuario
                {
                    IdTipoDeOperacion = (int)TipoOperacion.EncuestaCompletada,
                    FechaReg = fechaActual,
                    Notas = TipoOperacion.EncuestaCompletada.GetDescription(),
                    IdOrigen = idOrigen
                });

                if (bde.FechaInicioRespuesta == null)
                {
                    bde.FechaInicioRespuesta = fechaActual;
                }

                if (bde.FechaFinRespuesta == null)
                {
                    bde.FechaFinRespuesta = fechaActual;
                }

                if (pEncuesta.Preguntas.Any(x => x.Opciones.Any(y => y.IdOpcion == 1 || y.IdOpcion == 3)))
                {
                    bde.FechaRespuestaEsperada = fechaActual;
                    bde.Contestada = true;
                }

                Update(bde);

                resultado.Mensaje = encuesta.MensajeEnvio;
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        private BitacoraDeEncuestum Update(BitacoraDeEncuestum encuesta)
        {
            var strategy = DBContext.Database.CreateExecutionStrategy();

            strategy.Execute(() =>
            {
                var transaction = DBContext.Database.BeginTransaction();

                try
                {
                    DBContext.Update(encuesta);
                    DBContext.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }

            });

            return encuesta;
        }

        private List<BitacoraDeEncuestum> GetEncuestas(int pIdUsuario, long? pIdBDE = null)
        {
            return DBContext.BitacoraDeEncuesta
                .Where(x =>
                    x.IdUsuario == pIdUsuario
                    && x.IdEstatus == (int)TipoDeEstatus.Activo
                    && !x.Contestada
                    && (pIdBDE == null || x.Id == pIdBDE))
                .Select(x => new BitacoraDeEncuestum
                {
                    Id = x.Id,
                    Url = x.Url,
                    NoIngresos = x.NoIngresos,
                    FechaIngreso = x.FechaIngreso,
                    NoContestaciones = x.NoContestaciones,
                    FechaInicioRespuesta = x.FechaInicioRespuesta,
                    FechaFinRespuesta = x.FechaFinRespuesta,
                    FechaRespuestaEsperada = x.FechaRespuestaEsperada,
                    Contestada = x.Contestada,
                    IdUsuarioNavigation = x.IdUsuarioNavigation,
                    IdEncuestaNavigation = new Encuesta
                    {
                        Id = x.IdEncuestaNavigation.Id,
                        Codigo = x.IdEncuestaNavigation.Codigo,
                        Nombre = x.IdEncuestaNavigation.Nombre,
                        Url = x.IdEncuestaNavigation.Url,
                        PreguntasEncuesta = x.IdEncuestaNavigation.PreguntasEncuesta.Select(p => new PreguntasEncuestum
                        {
                            Id = p.Id,
                            IdTipoPregunta = p.IdTipoPregunta,
                            IdTipoPreguntaNavigation = p.IdTipoPreguntaNavigation,
                            NumeroPregunta = p.NumeroPregunta,
                            Texto = p.Texto,
                            Obligatoria = p.Obligatoria,
                            MensajeObligatoria = p.MensajeObligatoria,
                            LimiteRespuestas = p.LimiteRespuestas,
                            MensajeLimite = p.MensajeLimite,
                            RespuestasRequeridas = p.RespuestasRequeridas,
                            MsjRspRequeridas = p.MsjRspRequeridas,
                            Codigo = p.Codigo,
                            OpcionesPreguntumIdPreguntaNavigations = p.OpcionesPreguntumIdPreguntaNavigations.Select(o => new OpcionesPreguntum
                            {
                                Id = o.Id,
                                IdTipoControl = o.IdTipoControl,
                                IdTipoControlNavigation = o.IdTipoControlNavigation,
                                Texto = o.Texto,
                                Valor = o.Valor,
                                IdSkipPreguntaEncuesta = o.IdSkipPreguntaEncuesta
                            }).ToList()
                        }).ToList()
                    }
                }).ToList();
        }
    }
}
