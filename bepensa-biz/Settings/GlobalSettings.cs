namespace bepensa_biz.Settings;

public class GlobalSettings
{
    public bool Produccion { get; set; }

    public string AppName { get; set; } = null!;

    public string ClientName { get; set; } = null!;

    public string Url { get; set; } = null!;

    public string UrlTradicional { get; set; } = null!;

    public string UrlOnPrimes { get; set; } = null!;

    public SesionSettings Sesion { get; set; } = null!;

    public AutenticacionSettings Autenticacion { get; set; } = null!;

    public RecuperacionPasswordSettings RecuperacionPassword { get; set; } = null!;

    public DateOnly PeriodoInicial { get; set; }

    public int Paginate { get; set; } = 10;

    public string NoWhatsApp { get; set; } = null!;

    public bool GAnalytics { get; set; }

    public string RutaLocalImg { get; set; } = null!;
}
