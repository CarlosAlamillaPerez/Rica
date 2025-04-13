using AutoMapper;
using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.Extensions.Options;

namespace bepensa_biz.Proxies
{
    public class PeriodosProxy : ProxyBase, IPeriodo
    {
        private readonly GlobalSettings _ajustes;
        private readonly IMapper mapper;
        public PeriodosProxy(BepensaContext context, IMapper mapper, IOptionsSnapshot<GlobalSettings> ajustes)
        {
            DBContext = context;
            this.mapper = mapper;
            _ajustes = ajustes.Value;
        }

        public Respuesta<List<PeriodoDTO>> ConsultarPeriodos(int omitir)
        {
            Respuesta<List<PeriodoDTO>> resultado = new();
            try
            {
                var hoy = DateOnly.FromDateTime(DateTime.Now);

                var primerDiaDelMesActual = new DateOnly(hoy.Year, hoy.Month, 1);
                var primerDiaDelProximoMes = primerDiaDelMesActual.AddMonths(1);

                List<Periodo> periodos = DBContext.Periodos
                    .Where(p => p.Fecha >= _ajustes.PeriodoInicial && p.Fecha < primerDiaDelProximoMes)
                    .OrderByDescending(p => p.Fecha)
                    .Skip(omitir) // Omite una o mas fechas
                .ToList();

                if (periodos == null)
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                }

                resultado.Data = mapper.Map<List<PeriodoDTO>>(periodos);
            }
            catch (Exception)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
            }
        
            return resultado;
        }
    }
}
