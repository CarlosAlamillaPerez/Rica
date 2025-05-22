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
                var consultar = DBContext.BitacoraDeEncuesta
                    .Include(x => x.IdEncuestaNavigation)
                        .ThenInclude(x => x.BitacoraDeEncuesta)
                    .Include(x => x.IdEncuestaNavigation)
                        .ThenInclude(x => x.PreguntasEncuesta)
                            .ThenInclude(x => x.IdTipoPreguntaNavigation)
                    .Include(x => x.IdEncuestaNavigation)
                        .ThenInclude(x => x.PreguntasEncuesta)
                            .ThenInclude(x => x.OpcionesPreguntumIdPreguntaNavigations)
                            .ThenInclude(x => x.IdTipoControlNavigation)
                    .Where(x =>
                        x.IdEncuestaNavigation.BitacoraDeEncuesta.Any(y => y.Id == x.Id)
                        && x.IdUsuario == pIdUsuario
                        && x.IdEstatus == (int)TipoDeEstatus.Activo && !x.Contestada)
                    .Select(x => x)
                    .ToList();

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

                var bde = DBContext.BitacoraDeEncuesta
                    .Include(x => x.IdEncuestaNavigation)
                        .ThenInclude(x => x.BitacoraDeEncuesta)
                    .Include(x => x.IdEncuestaNavigation)
                        .ThenInclude(x => x.PreguntasEncuesta)
                            .ThenInclude(x => x.IdTipoPreguntaNavigation)
                    .Include(x => x.IdEncuestaNavigation)
                        .ThenInclude(x => x.PreguntasEncuesta)
                            .ThenInclude(x => x.OpcionesPreguntumIdPreguntaNavigations)
                            .ThenInclude(x => x.IdTipoControlNavigation)
                    .Where(x =>
                        x.IdEncuestaNavigation.BitacoraDeEncuesta.Any(y => y.Id == x.Id)
                        && x.IdUsuario == pEncuesta.IdUsuario
                        && x.IdEstatus == (int)TipoDeEstatus.Activo
                        && x.Id == pEncuesta.IdBitacoraEncuesta)
                    .Select(x => x)
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
                            Texto = opcion.Texto
                        });
                    }
                }

                bde.NoContestaciones++;

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
    }
}
