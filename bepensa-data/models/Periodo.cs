using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Periodo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    public virtual ICollection<BitacoraDeWhatsApp> BitacoraDeWhatsApps { get; set; } = new List<BitacoraDeWhatsApp>();

    public virtual ICollection<BitacoraPushNotificacione> BitacoraPushNotificaciones { get; set; } = new List<BitacoraPushNotificacione>();

    public virtual ICollection<Empaque> Empaques { get; set; } = new List<Empaque>();

    public virtual ICollection<EvaluacionesAcumulacion> EvaluacionesAcumulacions { get; set; } = new List<EvaluacionesAcumulacion>();

    public virtual ICollection<HistoricoDeCortesCuentum> HistoricoDeCortesCuenta { get; set; } = new List<HistoricoDeCortesCuentum>();

    public virtual ICollection<HistoricoVenta> HistoricoVenta { get; set; } = new List<HistoricoVenta>();

    public virtual ICollection<ImagenesPromocione> ImagenesPromociones { get; set; } = new List<ImagenesPromocione>();

    public virtual ICollection<MetasMensuale> MetasMensuales { get; set; } = new List<MetasMensuale>();

    public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();

    public virtual ICollection<PorcentajesIncrementoVentum> PorcentajesIncrementoVenta { get; set; } = new List<PorcentajesIncrementoVentum>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
