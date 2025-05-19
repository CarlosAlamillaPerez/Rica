namespace bepensa_biz.Settings;

public class GlobalSettings
{
    public bool Produccion { get; set; }

    public string Url { get; set; } = null!;

    public SesionSettings Sesion { get; set; } = null!;

    public AutenticacionSettings Autenticacion { get; set; } = null!;

    public RecuperacionPasswordSettings RecuperacionPassword { get; set; } = null!;

    public DateOnly PeriodoInicial { get; set; }

    public string NoWhatsApp { get; set; } = null!;
}
