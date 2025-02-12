namespace bepensa_socio_selecto_models.General;

public class AutenticacionRespuesta
{
    public string Token { get; set; } = null!;

    public DateTime Expira { get; set; }
}
