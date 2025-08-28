using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class DetallesDeEstadosDeCuentum
{
    public long Id { get; set; }

    public long IdEstadoDeCuenta { get; set; }

    public int IdSubconceptoDeAcumulacion { get; set; }

    public decimal Cantidad { get; set; }

    public int Puntos { get; set; }

    public string? Comentarios { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public virtual EstadosDeCuentum IdEstadoDeCuentaNavigation { get; set; } = null!;

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual SubconceptosDeAcumulacion IdSubconceptoDeAcumulacionNavigation { get; set; } = null!;

    public virtual ICollection<Redencione> Redenciones { get; set; } = new List<Redencione>();

    public virtual ICollection<Saldo> Saldos { get; set; } = new List<Saldo>();
}
