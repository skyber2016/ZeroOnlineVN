using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DetectDupeItem.Services
{
    internal static class WinService
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static Uri BaseAddress { get; set; }
        public static async Task BlockIP(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return;
            try
            {
                if (ip == "103.188.166.96") return;
                using (var http = new HttpClient())
                {
                    http.BaseAddress = BaseAddress;
                    var content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        ip
                    }), Encoding.UTF8, "application/json");
                    var response = await http.PostAsync("/block", content);
                    Console.WriteLine($"Block IP {ip} response {response.StatusCode}");
                    _logger.Info($"Block IP {ip} response {response.StatusCode}");
                    var respContent = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(respContent))
                    {
                        Console.WriteLine($"Block IP {ip} response {respContent}");
                        _logger.Info($"Block IP {ip} response {respContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException().Message);
                _logger.Error(ex.GetBaseException().Message);
            }


        }

        public static async Task UnblockIP(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return;
            if (ip == "103.188.166.96") return;
            try
            {
                using (var http = new HttpClient())
                {
                    http.BaseAddress = BaseAddress;
                    var content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        ip
                    }), Encoding.UTF8, "application/json");
                    var response = await http.PostAsync("/unblock", content);
                    Console.WriteLine($"UnBlock IP {ip} response {response.StatusCode}");
                    _logger.Info($"UnBlock IP {ip} response {response.StatusCode}");
                    var respContent = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(respContent))
                    {
                        Console.WriteLine($"UnBlock IP {ip} response {respContent}");
                        _logger.Info($"UnBlock IP {ip} response {respContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException().Message);
                _logger.Error(ex.GetBaseException().Message);
            }
        }
    }
}