using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class TiposDeArchivoDeCarga
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<ArchivosDeCarga> ArchivosDeCargas { get; set; } = new List<ArchivosDeCarga>();
}
