using System.Collections.Generic;

namespace API.Configurations
{
    public class WheelItem
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Rate { get; set; }
        public int ActionId { get; set; }
    }
    public class WheelSetting
    {
        public List<WheelItem> Items { get; set; }
    }
}
