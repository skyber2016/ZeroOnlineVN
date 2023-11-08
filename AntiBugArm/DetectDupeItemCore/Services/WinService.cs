using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DetectDupeItem.Services
{
    internal static class WinService
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void BlockIP(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return;
            if (ip == "103.188.166.96") return;
            var key = $"BLOCK IP ADDRESS - {ip}";
            Process compiler = new Process();
            compiler.StartInfo.FileName = "cmd";
            compiler.StartInfo.Arguments = $"/C netsh advfirewall firewall add rule name=\"{key}\" dir=in action=block remoteip={ip}";
            compiler.StartInfo.UseShellExecute = false;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.Start();

            compiler.WaitForExit();
            var output = compiler.StandardOutput.ReadToEnd();
            _logger.Info($"{key} : {output.Trim()}");

        }

        public static void UnblockIP(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return;
            if (ip == "103.188.166.96") return;
            var key = $"BLOCK IP ADDRESS - {ip}";
            //System.Diagnostics.Process.Start("cmd", $"/C netsh advfirewall firewall delete rule name=\"{key}\"");
            Process compiler = new Process();
            compiler.StartInfo.FileName = "cmd";
            compiler.StartInfo.Arguments = $"/C netsh advfirewall firewall delete rule name=\"{key}\"";
            compiler.StartInfo.UseShellExecute = false;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.Start();

            compiler.WaitForExit();
            var output = compiler.StandardOutput.ReadToEnd();
            _logger.Info($"RELEASE {key} : {output.Trim()}");
        }
    }
}

//using Newtonsoft.Json;
//using System;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;

//namespace DetectDupeItem.Services
//{
//    internal static class WinService
//    {
//        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
//        private static Uri BaseAddress => new Uri(Environment.GetCommandLineArgs()[2]);
//        public static async Task BlockIP(string ip)
//        {
//            if (string.IsNullOrEmpty(ip)) return;
//            if (ip == "103.188.166.96") return;
//            using (var http = new HttpClient())
//            {
//                http.BaseAddress = BaseAddress;
//                var content = new StringContent(JsonConvert.SerializeObject(new
//                {
//                    ip
//                }), Encoding.UTF8, "application/json");
//                var response = await http.PostAsync("/block", content);
//                _logger.Info($"Block IP response {response.StatusCode}");
//                var respContent = await response.Content.ReadAsStringAsync();
//                if (!string.IsNullOrEmpty(respContent))
//                {
//                    _logger.Info($"Block IP response {respContent}");
//                }
//            }

//        }

//        public static async Task UnblockIP(string ip)
//        {
//            if (string.IsNullOrEmpty(ip)) return;
//            if (ip == "103.188.166.96") return;
//            using (var http = new HttpClient())
//            {
//                http.BaseAddress = BaseAddress;
//                var content = new StringContent(JsonConvert.SerializeObject(new
//                {
//                    ip
//                }), Encoding.UTF8, "application/json");
//                var response = await http.PostAsync("/unblock", content);
//                _logger.Info($"UnBlock IP response {response.StatusCode}");
//                var respContent = await response.Content.ReadAsStringAsync();
//                if (!string.IsNullOrEmpty(respContent))
//                {
//                    _logger.Info($"UnBlock IP response {respContent}");
//                }
//            }
//        }
//    }
//}