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
using Microsoft.Extensions.Options;

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
                resultado.Data = GetEncuestas(pIdUsuario);

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

                var bde = DBContext.BitacoraDeEncuesta
                    .Include(x => x.IdUsuarioNavigation)
                    .Include(x => x.IdEncuestaNavigation)
                        .ThenInclude(x => x.PreguntasEncuesta)
                            .ThenInclude(x => x.RespuestaEsperada)
                    .Where(x => x.IdUsuario == pEncuesta.IdUsuario && x.Id == pEncuesta.IdBitacoraEncuesta)
                    .FirstOrDefault();

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

                Encuesta encuesta = bde.IdEncuestaNavigation;

                ICollection<PreguntasEncuestum> preguntas = encuesta.PreguntasEncuesta;

                var preOblitorias = encuesta.PreguntasEncuesta.Where(x => x.IdEstatus == (int)TipoEstatus.Activo && x.Obligatoria).ToList();

                if (pEncuesta.Preguntas.Select(x => preOblitorias.Select(x => x.Id).Contains(x.IdPregunta)).Count() != preOblitorias.Count)
                {
                    resultado.Codigo = (int)CodigoDeError.PreguntaFaltante;
                    resultado.Mensaje = CodigoDeError.PreguntaFaltante.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                bool cerrarEncuesta = true;

                foreach (var pregunta in pEncuesta.Preguntas)
                {
                    var result = ResponderPregunta(encuesta.Persistente, preguntas.First(x => x.Id == pregunta.IdPregunta), pregunta);

                    ICollection<RespuestasEncuestum> respuestas = result.Respuesta;
                    
                    if (!result.CerrarEncuesta)
                    {
                        cerrarEncuesta = false;
                    }

                    foreach (var respuesta in respuestas)
                    {
                        respuesta.IdBitacoraEncuesta = pEncuesta.IdBitacoraEncuesta;
                        respuesta.IdOrigen = idOrigen;
                        respuesta.IdPregunta = pregunta.IdPregunta;
                        respuesta.FechaReg = fechaActual;

                        bde.RespuestasEncuesta.Add(respuesta);
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

                if (cerrarEncuesta)
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

        private List<BitacoraEncuestaDTO> GetEncuestas(int pIdUsuario, long? pIdBDE = null)
        {
            return DBContext.BitacoraDeEncuesta
                .Where(x =>
                    x.IdUsuario == pIdUsuario
                    && x.IdEstatus == (int)TipoDeEstatus.Activo
                    && !x.Contestada
                    && (pIdBDE == null || x.Id == pIdBDE))
                .Select(x => new BitacoraEncuestaDTO
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
                    Encuesta = new EncuestaDTO
                    {
                        Id = x.IdEncuestaNavigation.Id,
                        Codigo = x.IdEncuestaNavigation.Codigo,
                        Nombre = x.IdEncuestaNavigation.Nombre,
                        Url = x.IdEncuestaNavigation.Url,
                        Preguntas = x.IdEncuestaNavigation.PreguntasEncuesta.Select(p => new PreguntaEncuestaDTO
                        {
                            Id = p.Id,
                            IdTipoPregunta = p.IdTipoPregunta,
                            TipoPregunta = p.IdTipoPreguntaNavigation.Nombre,
                            NumeroPregunta = p.NumeroPregunta,
                            Texto = p.Texto,
                            Obligatoria = p.Obligatoria,
                            MensajeObligatoria = p.MensajeObligatoria,
                            LimiteRespuestas = p.LimiteRespuestas,
                            MensajeLimite = p.MensajeLimite,
                            RespuestasRequeridas = p.RespuestasRequeridas,
                            MsjRspRequeridas = p.MsjRspRequeridas,
                            Codigo = p.Codigo,
                            Opciones = p.OpcionesPreguntumIdPreguntaNavigations.Select(o => new OpcionPreguntaDTO
                            {
                                Id = o.Id,
                                IdTipoControl = o.IdTipoControl,
                                TipoControl = o.IdTipoControlNavigation.Nombre,
                                Texto = o.Texto,
                                Valor = o.Valor,
                                IdSkipPreguntaEncuesta = o.IdSkipPreguntaEncuesta
                            }).ToList()
                        }).ToList()
                    }
                }).ToList();
        }

        private (bool CerrarEncuesta, ICollection<RespuestasEncuestum> Respuesta) ResponderPregunta(bool pEncPersistente, PreguntasEncuestum pPregunta, PreguntaRequest pPreguntaSelect)
        {
            switch (pPregunta.IdTipoPregunta)
            {
                case (int)TipoPregunta.Cerrada:
                    return PreguntaCerrada(pEncPersistente, pPregunta, pPreguntaSelect.Opciones.First());
                default:
                    throw new Exception(TipoExcepcion.TipoPreguntaNoIdentificado.GetDescription());
            }
        }

        private (bool CerrarEncuesta, ICollection<RespuestasEncuestum> Respuesta) PreguntaCerrada(bool pEncPersistente, PreguntasEncuestum pPregunta, RespuestaRequest pOpcionesSelect)
        {
            if (pEncPersistente)
            {
                if (pPregunta.RespuestaEsperada.Count > 0 && !pPregunta.RespuestaEsperada.Any(x => x.IdOpcionPregunta != null && x.IdOpcionPregunta == pOpcionesSelect.IdOpcion))
                {
                    return (false, [new RespuestasEncuestum
                    {
                        IdOpcionPregunta = pOpcionesSelect.IdOpcion
                    }]);
                }
            }

            return (true, [new RespuestasEncuestum
            {
                IdOpcionPregunta = pOpcionesSelect.IdOpcion
            }]);
        }
    }
}
