using bepensa_data.models;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IBitacoraEnvioCorreo
    {
        Respuesta<BitacoraEnvioCorreoDTO> ConsultaByToken(Guid? token);
        Respuesta<BitacoraEnvioCorreoDTO> ActualizaEstatus(long idBitacoraEnvioCorreo, TipoDeEstatus estatus);

        Respuesta<PlantillaCorreoDTO> ConsultarPlantilla(string pCodigo, int idUsuario, int idPeriodo);
    }
}
