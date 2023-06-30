using GameServer.Core;
using GameServer.Core.Utils;
using SimpleTCP;
using System.Net.Sockets;
using System.Runtime.Caching;

namespace GameServer.Core
{
    public class Client : IDisposable
    {
        private readonly Logging Logging = Program.Logging;
        private TcpClient Game;
        private SimpleTcpClient _client { get; set; }
        public string Username { get; set; }
        public string SecretKey { get; set; }
        public Client(TcpClient tcpClient)
        {
            Game = tcpClient;
            _client = new SimpleTcpClient();
            _client.DataReceived += ServerSendDataToMid;
            Logging.Write()(GetMessage($"Logout at {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}"));
        }
        public Client(TcpClient tcpClient, string username) : this(tcpClient)
        {
            this.Username = username;
            Logging.Write()(GetMessage($"Logout at {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}"));
        }

        public void Listen()
        {
            var settings = Settings.GetSettings();
            var task = Task.Run(() =>
            {
                _client.Connect(settings.IpServer, settings.PortGameServer);
            });
            if (!Task.WaitAll(new Task[] { task }, TimeSpan.FromSeconds(3)))
            {
                Logging.Write()("TIMEOUT CONNECT TO SERVER");
                this.Dispose();
            }
            else
            {
                Logging.Write()(GetMessage($"Connect successfully {settings.IpServer}:{settings.PortGameServer}"));
            }
        }

        private string GetMessage(string message)
        {
            return $"[{Game?.Client?.RemoteEndPoint}] [{(Username ?? string.Empty).PadLeft(16)}] -> {message}";
        }

        public void GameSendDataToMid(object sender, Message e)
        {
            try
            {
                var data = GameServer.Core.ByteExtensions.vnJoin(e.Data);
                var length = BitConverter.ToInt16(e.Data.Take(2).ToArray(), 0);
                var tempData = e.Data;
                var datas = new List<byte[]>();
                while (tempData.Any()) 
                {
                    var l = BitConverter.ToInt16(tempData.Take(2).ToArray(), 0);
                    var addData = tempData.Skip(0).Take(l).ToArray();
                    datas.Add(addData);
                    tempData = tempData.Skip(l).ToArray();
                }
                var dataARM = datas.Where(x => x.GetPacketType(2).vnEquals(PacketContants.ARM)).ToList();
                foreach (var dataChecking in dataARM)
                {
                    Logging.WriteData()(GetMessage(data));
                    if (dataChecking.Length > 24)
                    {
                        Logging.Write()($"Detected bug lag ARM -> {string.Join(" ", e.Data)}");
                        return;
                    }
                    var key = "ChangeARM";
                    object obj = MemoryCache.Default.Get(key);
                    if (obj == null)
                    {
                        MemoryCache.Default.Add(key, e.Data, DateTime.Now.AddSeconds(2));
                        Logging.Write()($"Add to cache {e.Data.vnJoin()}");
                    }
                    else
                    {
                        Logging.Write()(GetMessage("Pendding change arm"));
                        return;
                    }
                }
                
                if (this._client.TcpClient.Connected)
                {
                    try
                    {
                        this._client.Write(e.Data);
                    }
                    catch (Exception ex)
                    {
                        this.Logging.Write()(ex.GetBaseException().Message);
                        this.Dispose();
                    }
                }
                else
                {
                    Logging.Write()(GetMessage("Client disconnected"));
                    this.Dispose();
                    return;
                }
                
            }
            catch (Exception ex)
            {
                Logging.Write()(GetMessage(ex.GetBaseException().Message));
            }
        }

        private void ServerSendDataToMid(object sender, Message e)
        {
            byte[] dataSend = e.Data;

            try
            {
                if (!this.Game.Client.Connected)
                {
                    Logging.Write()(GetMessage("Game is not connected"));
                    this.Dispose();
                    return;
                }
                Game.Client.Send(dataSend);
            }
            catch (Exception ex)
            {

                Logging.Write()(GetMessage(ex.GetBaseException().Message));
                this.Dispose();
            }
        }

        public void Dispose()
        {
            try
            {
                Logging.Write()(GetMessage($"Logout at {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}"));
                this._client.Disconnect();
                this._client.Dispose();
                Game.Close();
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                Logging.Write()(GetMessage(ex.GetBaseException().Message));
            }
        }
    }
}
