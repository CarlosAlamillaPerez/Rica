namespace bepensa_socio_selecto_biz.Settings;

public class GlobalSettings
{
    public bool Produccion { get; set; }

    public string Url { get; set; } = null!;

    public SesionSettings Sesion { get; set; } = null!;

    public AutenticacionSettings Autenticacion { get; set; } = null!;

    public RecuperacionPasswordSettings RecuperacionPassword { get; set; } = null!;
}
