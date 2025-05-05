namespace bepensa_models.App;

public class ImagenesPromocionesDTO
{
    public int Id { get; set; }

    public int IdCanal { get; set; }

    public int IdPeriodo { get; set; }

    public string Nombre { get; set; } = null!;

    public string Url { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public int IdOperadorMod { get; set; }
}
