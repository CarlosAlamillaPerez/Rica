using bepensa_biz.Settings;
using bepensa_models.ApiResponse;
using bepensa_models.General;
using Newtonsoft.Json;

namespace bepensa_biz.Interfaces
{
    public interface IApi
    {
        #region RMS
        Respuesta<RastreoRMS> Autenticacion();

        Respuesta<ResponseRastreoGuia> ConsultaFolio(RequestEstatusOrden data, string? token);
        #endregion

        #region Api Canje de Premios Digitales
        Respuesta<DisponibilidadMKT> Disponibilidad(List<string> data);

        Respuesta<DisponibilidadMKT> Disponibilidad(string data);

        Respuesta<List<ResponseApiCPD>> RedimePremiosDigitales(RequestApiCPD data);
        #endregion
    }
}
