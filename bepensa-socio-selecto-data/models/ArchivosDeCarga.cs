using System;
using System.Collections.Generic;

namespace bepensa_socio_selecto_data.models;

public partial class ArchivosDeCarga
{
    public int Id { get; set; }

    public int IdTipoDeArchivoDeCarga { get; set; }

    public string NombreDelArchivo { get; set; } = null!;

    public int TotalDeRegistros { get; set; }

    public int RegistrosCargados { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual TiposDeArchivoDeCarga IdTipoDeArchivoDeCargaNavigation { get; set; } = null!;
}
