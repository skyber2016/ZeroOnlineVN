using Core;
using Core.Utils;
using SimpleTCP;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;

namespace LoginServer
{
    public class Client : IDisposable
    {
        private readonly Logging Logging = Program.Logging;
        private TcpClient Game;
        private SimpleTcpClient _client { get; set; }
        private string _username { get; set; }
        private Action OnDispose { get; set; }
        public Client(TcpClient tcpClient, Action onDispose)
        {
            OnDispose = onDispose;
            Game = tcpClient;
            _client = new SimpleTcpClient();
            _client.DataReceived += ServerSendDataToMid;
        }

        public void Listen()
        {
            var settings = Settings.GetSettings();
            Console.WriteLine($"Connect to {settings.PortLoginServer}");
            var task = Task.Run(() =>
            {
                _client.Connect(settings.IpServer, settings.PortLoginServer);
            });
            if (!Task.WaitAll(new Task[] { task }, TimeSpan.FromSeconds(3)))
            {
                Logging.Write()("TIMEOUT CONNECT TO SERVER");
                this.Dispose();
            }
        }

        private string GetMessage(string message)
        {
            return $"[{Game?.Client?.RemoteEndPoint}] [{_username?.PadLeft(16)}] -> {message}";
        }

        public void GameSendDataToMid(object sender, Message e)
        {

            if (e.Data.Length > 136)
            {
                Logging.Write()(GetMessage("Data wrong"));
                return;
            }
            var ip = e.TcpClient.GetIP();
            try
            {
                if (e.Data.GetPacketType().vnEquals(PacketContants.LoginTypeRequest))
                {
                    this._username = e.Data.Skip(8).Take(32).Where(x => x != 0x0).ToArray().ConvertToString();
                    Logging.Write()(GetMessage("Setted username"));
                }
                if (this._client.TcpClient.Connected)
                {
                    this._client.Write(e.Data);
                }
                else
                {
                    Logging.Write()(GetMessage("Server game is disconnected"));
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logging.Write()(ex.GetBaseException().ToString());
            }
        }

        private void ServerSendDataToMid(object sender, Message e)
        {
            byte[] dataSend = e.Data;

            try
            {
                if (e.Data.GetPacketType(2).vnEquals(PacketContants.LoginResponse))
                {
                    var setting = Settings.GetSettings();
                    var secretKey = e.Data.Skip(8).Take(4).ToArray();
                    var data = string.Join(" ", e.Data);
                    var portLoginServer = string.Join(" ", BitConverter.GetBytes(setting.PortGameServer));
                    var avaiablePort = TcpService.GetRandomUnusedPort();
                    //var avaiablePort = 62664;
                    var portLoginMid = string.Join(" ", BitConverter.GetBytes(avaiablePort));
                    var ipServer = setting.IpServer;
                    var ipMid = setting.IpMid;
                    if (data.Contains(portLoginServer))
                    {
                        dataSend = data.Replace(portLoginServer, portLoginMid).Split(' ').Select(x => Convert.ToByte(x)).ToArray();
                        dataSend = dataSend.Replace(ipServer.ToByte(), ipMid.ToByte());
                        Logging.Write()(GetMessage($"Change port {portLoginServer} to {portLoginMid}"));
                        Logging.Write()(GetMessage($"Change IP {ipServer} to {ipMid}"));
                        var pathToFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "GameServer.exe");
                        if (File.Exists(pathToFile))
                        {
                            var process = new Process();
                            process.StartInfo.FileName = pathToFile;
                            process.StartInfo.Arguments = $"{_username} {avaiablePort}";
                            process.StartInfo.CreateNoWindow = true;
                            process.StartInfo.RedirectStandardOutput = true;
                            process.StartInfo.RedirectStandardError = true;
                            process.StartInfo.UseShellExecute = false;
                            process.EnableRaisingEvents = true;
                            process.Start();
                            Logging.Write()(GetMessage($"Create server game with port {avaiablePort}"));
                        }
                        else
                        {
                            Logging.Write()($"Not found {pathToFile}");
                        }
                    }
                    
                }
                if (!this.Game.Client.Connected)
                {
                    Logging.Write()(GetMessage("Game is not connected"));
                    this.Dispose();
                    return;
                }
                Game.Client.Send(dataSend);
            }
            catch (Exception)
            {
                this.Dispose();
            }
        }

        public void Dispose()
        {
            try
            {
                Game.Close();
                this._client.Disconnect();
                this._client.Dispose();
                this.OnDispose();
            }
            catch (Exception ex)
            {
                Logging.Write()(ex.GetBaseException().Message);
            }
        }
    }
}
