using System;
using System.Collections.Generic;

namespace bepensa_socio_selecto_data.models;

public partial class LayoutAltaInscripciónDomV1
{
    public string Cuc { get; set; } = null!;

    public string Embotelladora { get; set; } = null!;

    public string Cedi { get; set; } = null!;

    public string NombreDeJefeVenta { get; set; } = null!;

    public string NombreSupervisor { get; set; } = null!;

    public string RazonSocial { get; set; } = null!;

    public string? TelefonoFijo { get; set; }

    public string Provincia { get; set; } = null!;

    public string Municipio { get; set; } = null!;

    public string Calle { get; set; } = null!;

    public int? IdEmbotelladora { get; set; }

    public int? IdCedi { get; set; }

    public int? IdSupervisor { get; set; }
}
