using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class Programa
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? PrefijoRms { get; set; }

    public int IdEstatus { get; set; }

    public DateTime? FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual ICollection<ImagenesPromocione> ImagenesPromociones { get; set; } = new List<ImagenesPromocione>();

    public virtual ICollection<PuntajesDeSubconceptosDeAcumulacion> PuntajesDeSubconceptosDeAcumulacions { get; set; } = new List<PuntajesDeSubconceptosDeAcumulacion>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
