using Microsoft.EntityFrameworkCore;

namespace bepensa_data.StoredProcedures.Models;

[Keyless]
public class ConceptosEdoCtaCTE
{
    public int SaldoAnterior { get; set; }

    public int AcumuladoActual { get; set; }

    public int PuntosDisponibles { get; set; }

    public int PuntosCanjeados { get; set; }

    public int CanjesRealizados { get; set; }
}
