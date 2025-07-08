using bepensa_models.App;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;

namespace bepensa_biz.Interfaces;

public interface IApp
{
    Task<Respuesta<string>> ConsultaParametro(int pParametro);

    Task<Respuesta<List<ImagenesPromocionesDTO>>> ConsultaImgPromociones(int IdCanal);

    Task<Respuesta<Empty>> SeguimientoVistas(SegVistaRequest data, int idOrigen = (int)TipoOrigen.App);
}
