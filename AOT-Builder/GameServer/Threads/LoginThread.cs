using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    public class LoginThread
    {
        private static LoginThread _instance { get;set; }
        public static LoginThread Instance
        {
            get
            {
                return _instance ??= new LoginThread();
            }
        }

        public async Task ListenAsync(string targetIp, int targetPort)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, NetworkManager.ProxyLoginPort);
            listener.Start();
            Console.WriteLine($"Proxy listening on port {NetworkManager.ProxyLoginPort}");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                var loginClient = NetworkManager.Append(new LoginClient(client, targetIp, targetPort));
                _ = Task.Run(async () =>
                {
                    await loginClient.BeginTransferAsync();
                    NetworkManager.Remove(loginClient);
                } );
            }
        }

    }
}
