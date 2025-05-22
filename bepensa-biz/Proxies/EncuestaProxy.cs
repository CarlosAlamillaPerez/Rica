using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.EntityFrameworkCore;

namespace bepensa_biz.Proxies
{
    public class EncuestaProxy : ProxyBase, IEncuesta
    {
        public EncuestaProxy(BepensaContext context)
        {
            DBContext = context;
        }

        public Respuesta<List<Encuesta>> ConsultarEncuestas(int pIdUsuario)
        {
            Respuesta<List<Encuesta>> resultado = new();

            try
            {
                var consultar = DBContext.BitacoraDeEncuesta
                    .Include(x => x.IdEncuestaNavigation)
                        .ThenInclude(x => x.PreguntasEncuesta)
                            .ThenInclude(x => x.IdTipoPreguntaNavigation)
                    .Include(x => x.IdEncuestaNavigation)
                        .ThenInclude(x => x.PreguntasEncuesta)
                            .ThenInclude(x => x.OpcionesPreguntumIdPreguntaNavigations)
                            .ThenInclude(x => x.IdTipoControlNavigation)
                    .Where(x => x.IdUsuario == pIdUsuario && x.IdEstatus == (int)TipoDeEstatus.Activo && !x.Contestada)
                    .Select(x => x.IdEncuestaNavigation)
                    .ToList();

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
    }
}
