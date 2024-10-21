using System.Net.Sockets;

namespace GameServer
{
    public class GameClient : AClient
    {
        public GameClient(TcpClient tcpClient, string targetIp, int targetPort) : base(tcpClient, targetIp, targetPort)
        {
        }

        public override async Task HandleMessage(byte[] buffer, int len, Channel channel)
        {
            if (channel == Channel.C2S)
            {
                await _targetNetwork.WriteAsync(buffer, 0, len);
                await _targetNetwork.FlushAsync();
            }
            else
            {
                await _clientNetwork.WriteAsync(buffer, 0, len);
                await _clientNetwork.FlushAsync();
            }
        }
    }
}
