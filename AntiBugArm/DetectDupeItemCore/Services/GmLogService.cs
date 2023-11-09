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
            var byteArray = await http.GetByteArrayAsync(fileName);
            if (!byteArray.Any())
            {
                return Array.Empty<string>();
            }
            var text = encoding.GetString(byteArray);
            return text.Split(Environment.NewLine);
        }
    }
}
