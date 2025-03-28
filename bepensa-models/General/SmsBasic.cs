namespace bepensa_models.General;

public class SmsBasic
{
    public string Text { get; set; } = null!;

    public string Campaign_name { get; set; } = null!;

    public List<TelefonoSms> Recipients { get; set; } = null!;

    public int Route { get; set; }

    public int Country { get; set; }

    public bool Encode { get; set; }

    public bool Long_message { get; set; }
}
