using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_models.ApiResponse;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.EntityFrameworkCore;

namespace bepensa_biz.Proxies
{
    public class OperacionesProxy : ProxyBase, IOperacion
    {
        private readonly Serilog.ILogger _logger;
        private readonly IApi _api;

        public OperacionesProxy(BepensaContext context, Serilog.ILogger logger, IApi api)
        {
            DBContext = context;
            _logger = logger;
            _api = api;
        }

        public async Task<Respuesta<Empty>> ActualizarEstatusRedenciones()
        {
            Respuesta<Empty> resultado = new()
            {
                IdTransaccion = Guid.NewGuid(),
            };

            try
            {
                var folios = await DBContext.Redenciones
                    .Where(x => x.FolioRms != null
                        && x.IdEstatusRedencion != (int)TipoEstatusRedencion.Entregado)
                    .Select(x => x.FolioRms)
                    .ToListAsync();

                var auth = await _api.Autenticacion();

                if (auth != null && auth.Data != null && auth.Data.Token != null)
                {
                    foreach (var folio in folios)
                    {
                        var consultarFolio = await _api.ConsultaFolio(new RequestEstatusOrden
                        {
                            Folio = folio,
                        }, auth.Data.Token);
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "ActualizarEstatusRedenciones() => IdTransaccion::{usuario}", resultado.IdTransaccion);
            }

            return resultado;
        }
    }
}
