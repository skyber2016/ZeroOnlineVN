using System.Collections.Generic;

namespace API.Configurations
{
    public class AppSettings
    {
        public int EventType_Statistic { get; set; }
        public int VIPDefault { get; set; }
        public string BotToken { get; set; }
        public List<ulong> Channels { get; set; }
    }
}
