namespace API.Configurations
{
    public class WheelItem
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Rate { get; set; }
    }
    public class WheelSetting
    {
        public WheelItem[] Items { get; set; }
    }
}
