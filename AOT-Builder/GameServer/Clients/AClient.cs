using System.IO;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    public abstract class AClient : IClient
    {
        protected readonly Guid _id;
        protected readonly NetworkStream _clientNetwork;
        protected NetworkStream _targetNetwork;
        protected readonly TcpClient _tcpClient;
        protected readonly TcpClient _targetServer;
        private readonly string _targetIp;
        private readonly int _targetPort;
        public AClient(TcpClient tcpClient, string targetIp, int targetPort)
        {
            _id = Guid.NewGuid();
            _tcpClient = tcpClient;
            _clientNetwork = tcpClient.GetStream();
            _targetServer = new TcpClient();
            
            _targetIp = targetIp;
            _targetPort = targetPort;

        }
        public abstract Task HandleMessage(byte[] buffer, int len, Channel channel);
        public string GetClientId() => _id.ToString();
        public IPEndPoint GetClientIP() => _tcpClient.Client.RemoteEndPoint as IPEndPoint;
        public async Task ConnectAsync()
        {
            await _targetServer.ConnectAsync(_targetIp, _targetPort);
            _targetNetwork = _targetServer.GetStream();
        }
        public async Task ReceivePacket(NetworkStream network, Channel channel)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;

            try
            {
                while ((bytesRead = await network.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await HandleMessage(buffer, bytesRead, channel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Transfer error: {ex.Message}");
            }
        }
        public async Task BeginTransferAsync()
        {
            await this.ConnectAsync();
            var serverToClient = this.ReceivePacket(_targetNetwork, Channel.S2C);
            var clientToServer = this.ReceivePacket(_clientNetwork, Channel.C2S);
            // Chờ cho đến khi một trong hai bên kết thúc
            await Task.WhenAny(clientToServer, serverToClient);
            this.Dispose();
        }

        public void Dispose()
        {
            _tcpClient.Dispose();
            _targetServer.Dispose();
        }

    }
}
