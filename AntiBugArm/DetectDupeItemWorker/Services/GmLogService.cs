using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DetectDupeItemCore.Services
{
    internal static class GmLogService
    {
        public static Uri BaseAddress { get; set; }
        public static async Task<string[]> GetLines(string fileName, Encoding encoding)
        {
            using var http = new HttpClient();
            http.BaseAddress = BaseAddress;
            var res = await http.GetAsync(fileName, Worker.ApplicationCancellationToken);
            if (res.IsSuccessStatusCode)
            {
                var byteArray = await res.Content.ReadAsByteArrayAsync();
                var text = encoding.GetString(byteArray);
                return text.Split('\n').Where(x => !string.IsNullOrEmpty(x) && x != "\r").ToArray();
            }
            else
            {
                Console.WriteLine($"GET request {fileName} response {res.StatusCode}");
            }
            return Array.Empty<string>();
        }
    }
}
