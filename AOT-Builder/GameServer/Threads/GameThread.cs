using System.Net.Sockets;
using System.Net;

namespace GameServer
{
    public class GameThread
    {
        private readonly string _targetIp;
        private readonly int _targetPort;
        private readonly int _proxyPort;
        private static GameThread _instance { get; set; }
        public static GameThread Instance
        {
            get
            {
                return _instance ??= new GameThread();
            }
        }

        public async Task ListenAsync(string targetIp, int targetPort)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, NetworkManager.ProxyGamePort);
            listener.Start();
            Console.WriteLine($"Proxy listening on port {NetworkManager.ProxyGamePort}");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                var gameClient = NetworkManager.Append(new GameClient(client, _targetIp, _proxyPort));
                _ = Task.Run(() => gameClient.BeginTransferAsync());
            }
        }
    }
}
