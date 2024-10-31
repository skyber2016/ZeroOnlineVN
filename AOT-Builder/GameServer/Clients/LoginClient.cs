using GameServer.Packets;
using System;
using System.Net.Sockets;

namespace GameServer
{
    public class LoginClient : AClient
    {
        public LoginClient(TcpClient tcpClient, string targetIp, int targetPort) : base(tcpClient, targetIp, targetPort)
        {
        }

        public override async Task HandleMessage(byte[] buffers, int len, Channel channel)
        {
            if (channel == Channel.C2S)
            {
                await HandleClient(buffers, len);
            }
            else
            {
                await HandleServer(buffers, len);
            }
        }

        private async Task HandleClient(byte[] buffers, int len)
        {
            var msg = new NetworkMessage();
            msg.Deserialize(buffers, len);

            if (msg.Type == PacketType._MSG_LOGIN)
            {
                var loginPacket = new LoginPacket();
                loginPacket.Deserialize(buffers, len);
                await _targetNetwork.WriteAsync(buffers, 0, len);
            }
            else
            {
                await _targetNetwork.WriteAsync(buffers, 0, len);
            }
            await _targetNetwork.FlushAsync();

        }

        private async Task HandleServer(byte[] buffers, int len)
        {
            await _clientNetwork.WriteAsync(buffers, 0, len);
            await _clientNetwork.FlushAsync();
        }
    }
}
