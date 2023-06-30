using Newtonsoft.Json;

namespace GameServer.Core
{
    public  class Settings
    {
        public string IpServer { get; set; }
        public string IpMid { get; set; }
        public int PortLoginServer { get; set; }
        public int PortGameServer { get; set; }
        public int PortLoginMid { get; set; }

        private static Settings _instance { get; set; }
        public static Settings GetSettings()
        {
            if(_instance == null )
            {
                var file = File.ReadAllText("appsettings.json");
                _instance =  JsonConvert.DeserializeObject<Settings>(file);
            }
            return _instance;
            
        }
    }
}
