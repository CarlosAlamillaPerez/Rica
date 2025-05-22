using AutoMapper;
using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_models.DTO;
using bepensa_models.DataModels;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.EntityFrameworkCore;

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

        public Respuesta<Empty> ResponderEncuesta(RepuestaEncuestaRequest pEncuesta)
        {
            Respuesta<Empty> resultado = new();

            try
            {

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
