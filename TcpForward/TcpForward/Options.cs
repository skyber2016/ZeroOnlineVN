using Newtonsoft.Json;

namespace ServerForward
{
    public class Options
    {
        public int LocalPort { get; set; }
        public string RemoteHost { get; set; }
        public int RemotePort { get; set; }
        public int MaxConnect { get; set; }
        public string HostConnection { get; set; }

        private static Options _instance { get; set; }
        public static Options Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GetOptions();
                }
                return _instance;
            }
        }

        private static Options GetOptions()
        {
            var baseDir = Directory.GetCurrentDirectory();
            var pathToConfig = Path.Combine(baseDir, "appsettings.json");
            if (File.Exists(pathToConfig))
            {
                var jsonText = File.ReadAllText(pathToConfig);
                return JsonConvert.DeserializeObject<Options>(jsonText);
            }
            throw new ApplicationException($"File not found path {pathToConfig}");
        }
    }
}
