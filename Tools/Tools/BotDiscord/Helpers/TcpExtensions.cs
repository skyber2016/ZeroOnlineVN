using API.Helpers;
using System.Net.Sockets;

namespace MiddlewareTCP
{
    public static class TcpExtensions
    {
        public static string GetSessionId(this TcpClient client)
        {
            return HashHelper.Base64Encode(client.Client.RemoteEndPoint.ToString());
        }
        public static string GetIP(this TcpClient client)
        {
            return client.Client.RemoteEndPoint.ToString();
        }
    }
}
