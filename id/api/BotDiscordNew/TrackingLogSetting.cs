namespace BotDiscordNew
{
    public class TrackingLogSetting
    {
        public TrackingItem[] Items { get; set; }
    }

    public class TrackingItem
    {
        public string ItemId { get; set; }
        public string Name { get; set; }
    }
}
