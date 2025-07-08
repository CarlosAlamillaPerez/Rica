using bepensa_biz.Settings;
using bepensa_models.ApiResponse;
using bepensa_models.General;
using Newtonsoft.Json;

namespace bepensa_biz.Interfaces
{
    public interface IApi
    {
        #region RMS
        Task<Respuesta<RastreoRMS>> Autenticacion();

        Task<Respuesta<ResponseRastreoGuia>> ConsultaFolio(RequestEstatusOrden data, string? token);
        #endregion

        #region Api Canje de Premios Digitales
        Task<Respuesta<DisponibilidadMKT>> Disponibilidad(List<string> data);

        Task<Respuesta<DisponibilidadMKT>> Disponibilidad(string data);

        Task<Respuesta<List<ResponseApiCPD>>> RedimePremiosDigitales(RequestApiCPD data);
        #endregion
    }
}
