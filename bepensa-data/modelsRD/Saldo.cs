using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class Saldo
{
    public long Id { get; set; }

    public long IdUsuario { get; set; }

    public long IdDetalleDeEstadoDeCuenta { get; set; }

    public int IdTipoDeMovimiento { get; set; }

    public int Puntos { get; set; }

    public int Saldo1 { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public virtual DetallesDeEstadosDeCuentum IdDetalleDeEstadoDeCuentaNavigation { get; set; } = null!;

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual TiposDeMovimiento IdTipoDeMovimientoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
