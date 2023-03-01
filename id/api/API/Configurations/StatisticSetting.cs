using System.Collections.Generic;

namespace API.Configurations
{
    public class StatisticSetting
    {
        public string Name { get; set; }
        public int EventType { get; set; }
        public long Data { get; set; }
        public string DataFormat => this.Data.ToString("#,##0");
    }
}
