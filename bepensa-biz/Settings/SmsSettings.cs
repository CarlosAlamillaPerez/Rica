namespace bepensa_biz.Settings
{
    public class SmsSettings
    {
        public string Url { get; set; } = null!;

        public string CampaignName { get; set; } = null!;

        public int Route { get; set; }

        public int Country { get; set; }
    }
}
