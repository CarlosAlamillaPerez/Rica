using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Origene
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<BitacoraPushNotificacione> BitacoraPushNotificaciones { get; set; } = new List<BitacoraPushNotificacione>();

    public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();

    public virtual ICollection<HistorialCompraPunto> HistorialCompraPuntos { get; set; } = new List<HistorialCompraPunto>();

    public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();

    public virtual ICollection<Redencione> Redenciones { get; set; } = new List<Redencione>();

    public virtual ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();

    public virtual ICollection<RespuestasEncuestum> RespuestasEncuesta { get; set; } = new List<RespuestasEncuestum>();

    public virtual ICollection<SeguimientoVista> SeguimientoVista { get; set; } = new List<SeguimientoVista>();
}
