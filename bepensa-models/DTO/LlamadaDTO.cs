namespace bepensa_models.DTO;

public class LlamadaDTO
{
    public int Id { get; set; }

    public int? IdPadre { get; set; }

    public string? Tema { get; set; }

    public int? IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Comentario { get; set; } = null!;

    public int IdTipoLlamada { get; set; }

    public string TipoLlamada { get; set; } = null!;

    public int IdSubcategoriaLlamada { get; set; }

    public string SubcategoriaLlamada { get; set; } = null!;

    public string CategoriaLlamada { get; set; } = null!;

    public int IdEstatusLlamada { get; set; }

    public string EstatusLlamada { get; set; } = null!;

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public int? IdOperadorMod { get; set; }

    public DateTime? FechaMod { get; set; }

    public UsuarioDTO? Usuario { get; set; }

    public List<LlamadaDTO>? Llamadas { get; set; }

    public OperadorDTO OperadorReg { get; set; } = null!;

    public OperadorDTO? OperadorMod { get; set; }
}
