using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace API.Helpers
{
    public static class HttpHelper
    {
        public static string GetIP(IHttpContextAccessor Accessor)
        {
            var key = "X-Forwarded-For";
            var hasIP = Accessor.HttpContext?.Request.Headers.ContainsKey(key) ?? false;
            if (!hasIP)
            {
                var host = Dns.GetHostEntry(Dns.GetHostName()).AddressList.LastOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                if (host != null)
                {
                    return host.ToString();
                }
                return "127.0.0.1";
            }
            return Accessor.HttpContext.Request.Headers[key].ToString();
        }
    }
}
