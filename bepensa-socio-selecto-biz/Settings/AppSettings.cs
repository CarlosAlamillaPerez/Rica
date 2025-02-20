namespace bepensa_biz.Settings;
public class AppSettings
{
    public int IdPrograma { get; set; }

    public string TokeyKey { get; set; } = null!;

    public int TokeyExpiration { get; set; }

    public string ApiKey { get; set; } = null!;

    public string ApiProvider { get; set; } = null!;
}
