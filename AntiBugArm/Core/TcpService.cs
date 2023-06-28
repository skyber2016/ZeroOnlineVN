using SimpleTCP;
using System.Net.Sockets;
using System.Net;

namespace Core
{
    public static class TcpService
    {
        public static SimpleTcpServer GetServer() => new SimpleTcpServer();
        public static SimpleTcpClient GetClient() => new SimpleTcpClient();

        public static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Any, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}
