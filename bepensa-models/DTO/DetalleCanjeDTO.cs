namespace bepensa_models.DTO;

public class DetalleCanjeDTO
{
    public int Id { get; set; }

    public string folio { get; set; } = null!;

    public int puntos { get; set; }

    public string nombre { get; set; } = null!;

    public int tipoCanjeId { get; set; }

    public string tipoCanje { get; set; } = null!;

    public string estatus { get; set; } = null!;

    public DateTime fechaCanje { get; set; }

    public int cantidad { get; set; }

    public string enviadoCedi { get; set; } = null!;

    public string observaciones { get; set; } = null!;

    public string referencias { get; set; } = null!;

    public string disponibleFeria { get; set; } = null!;

    public string tipoEnvioCodigo { get; set; } = null!;

    public string origenCanje { get; set; } = null!;

}
