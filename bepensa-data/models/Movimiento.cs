using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Movimiento
{
    public int Id { get; set; }

    public int IdPeriodo { get; set; }

    public int IdUsuario { get; set; }

    public int IdSubcptoAcumulacon { get; set; }

    public int Puntos { get; set; }

    public decimal? Cantidad { get; set; }

    public int Saldo { get; set; }

    public string? Comentario { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOrigen { get; set; }

    public int? IdOperadorReg { get; set; }

    public int? IdPrograma { get; set; }

    public Guid? IdTransaccionLog { get; set; }

    public virtual Operadore? IdOperadorRegNavigation { get; set; }

    public virtual Origene IdOrigenNavigation { get; set; } = null!;

    public virtual Periodo IdPeriodoNavigation { get; set; } = null!;

    public virtual Programa? IdProgramaNavigation { get; set; }

    public virtual SubconceptosDeAcumulacion IdSubcptoAcumulaconNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
