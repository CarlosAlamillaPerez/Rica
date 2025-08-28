using bepensa_models.DataModels;

namespace bepensa_models.General;

public class ReporteGeneral<TData>
{
    public TData? Data { get; set; }

    public ReporteRequest? DatosReportes { get; set; }
}
