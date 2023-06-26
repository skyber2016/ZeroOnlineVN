using API.Configurations;
using API.Cores;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MiddlewareTCP.command;
using Newtonsoft.Json.Linq;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MiddlewareTCP.Entities
{
    public class GameService : IDisposable
    {
        public string SessionId { get; set; }
        private SimpleTcpClient MidClient { get; set; }
        private IUnitOfWork UnitOfWork { get; set; }
        private IDictionary<string, byte[]> Cache { get; set; }
        private string Username { get; set; } = string.Empty;
        private string IP { get; set; } = "NOIP";
        private IOptions<AppSettings> AppSettings
        {
            get
            {
                return this.UnitOfWork.AppSettings;
            }
        }
        private TcpClient Game { get; set; }
        public GameService(IUnitOfWork unitOfWork)
        {
            try
            {
                this.Cache = new Dictionary<string, byte[]>();
                this.SessionId = Guid.NewGuid().ToString();
                this.UnitOfWork = unitOfWork;
                this.MidClient = new SimpleTcpClient();
                this.MidClient.DataReceived += ServerToMid_Receiver;
            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [Contructor] [{ex.Message}]");
                WriteError(ex);
            }
            
        }

        public void GameToMid_Disconnected(object sender, TcpClient e)
        {
            this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [GameToMid_Disconnected] [Game disconnected]");
            this.Dispose();
        }

        private void WriteError(Exception ex)
        {
            if(ex.InnerException != null)
            {
                this.WriteError(ex.InnerException);
            }
            this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [WriteError] [{ex.Message}]");
            this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [WriteError] [{ex.StackTrace}]");
        }
        
        public void GameToMid_Data(object sender, Message e)
        {
            try
            {
                var data = e.Data.Split();
                
                if (e.Data.GetPacketType(2).vnEquals(PacketContants.ARM))
                {
                    this.UnitOfWork.Logger.Data($"[GameToMid_Data] [{this.Username}] [{data}]");
                    var length = BitConverter.ToInt16(e.Data.Take(2), 0);
                    this.UnitOfWork.Logger.Queries($"[{this.IP}] [{this.Username}] Detected change ARM request -> {string.Join(" ", e.Data)}");
                    if (e.Data.Length != length)
                    {
                        this.UnitOfWork.Logger.Queries($"[{this.IP}] [{this.Username}] Detected packet length invalid -> {string.Join(" ", e.Data)}");
                    }
                    if (e.Data.Length > 24)
                    {
                        this.UnitOfWork.Logger.Error($"[{this.IP}] [{this.Username}] Detected bug lag ARM -> {string.Join(" ", e.Data)}");
                        return;
                    }
                    var key = "ChangeARM" + this.SessionId;
                    object obj;
                    this.UnitOfWork.Cache.TryGetValue(key, out obj);
                    if (obj == null)
                    {
                        this.UnitOfWork.Cache.Set(key, true, TimeSpan.FromSeconds(2));
                    }
                    else
                    {
                        this.UnitOfWork.Logger.Error($"[{this.IP}] [{this.Username}] pendding change arm");
                        this.UnitOfWork.Logger.Data($"[{this.IP}] [{this.Username}] pendding change arm");
                        return;
                    }
                }
                if (this.MidClient.TcpClient.Connected)
                {
                    try
                    {
                        this.MidClient.Write(e.Data);
                    }
                    catch (Exception ex)
                    {
                        this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [GameToMid_Data] [Dispose] [Mid client throw abort connection]");
                        this.WriteError(ex);
                        this.Dispose();
                    }
                }
                else
                {
                    this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [GameToMid_Data] [Dispose] [Client disconnected]");
                    this.Dispose();
                    return;
                }
                if (e.Data.GetPacketType(2).vnEquals(PacketContants.JoinGameRequest))
                {
                    this.UnitOfWork.Logger.Info($"[{this.Username}] [GameService] [GameToMid_Data] [Begin join game request]");
                    var secretKey = e.Data.Skip(8).Take(4).ToArray().Split();
                    if (PacketContants.SecretKey.ContainsKey(secretKey))
                    {
                        var username = PacketContants.SecretKey[secretKey];
                        this.UnitOfWork.Logger.Info($"[{e.TcpClient.GetIP()}] [{username}] login success");
                        this.Username = username;
                        
                    }
                    this.UnitOfWork.Logger.Info($"[{this.Username}] [GameService] [GameToMid_Data] [End join game request]");
                }
            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [GameToMid_Data] [Throw Exception]");
                this.WriteError(ex);
            }
        }

        public void GameToMid_Connected(object sender, TcpClient e)
        {
            try
            {
                this.Game = e;
                this.IP = this.Game.GetIP();
                var task = Task.Run(() =>
                {
                    try
                    {
                        this.UnitOfWork.Logger.Info($"[{this.Username}] [GameService] [GameToMid_Connected] [Begin Connect to server]");
                        this.MidClient.Connect(this.AppSettings.Value.IpServer, this.AppSettings.Value.PortGameServer);
                        this.UnitOfWork.Logger.Info($"[{this.Username}] [GameService] [GameToMid_Connected] [End connect to server success]");
                    }
                    catch (Exception ex)
                    {
                        this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [GameToMid_Connected] [{ex.Message}]");
                        this.WriteError(ex);
                    }
                });
                if (!task.Wait(TimeSpan.FromSeconds(3)))
                {
                    this.UnitOfWork.Logger.Error("TIMEOUT CONNECT TO SERVER");
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [GameToMid_Connected] [{ex.Message}]");
                this.Game.Close();
                this.Game.Dispose();
                this.WriteError(ex);
            }
            
        }

        public void ServerToMid_Receiver(object sender, Message e)
        {
            try
            {
                if (!this.Game.Client.Connected)
                {
                    this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [ServerToMid_Receiver] [Game not connected]");
                    this.Dispose();
                    return;
                }
                try
                {
                    this.Game.Client.Send(e.Data);
                }
                catch (Exception ex)
                {
                    this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [ServerToMid_Receiver] [Game throw abort connection]");
                    this.WriteError(ex);
                    this.Dispose();
                }

            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [ServerToMid_Receiver] [Throw exception]");
                this.WriteError(ex);
            }
            
        }

        public void Dispose()
        {
            try
            {
                this.UnitOfWork.Logger.Info($"[{this.Username}] [GameService] [Dispose] [Begin Logout client]");
                this.Game.Close();
                this.Game.Dispose();
                this.MidClient.Disconnect();
                this.MidClient.Dispose();
                this.UnitOfWork.Logger.Info($"[{this.Username}] [GameService] [Dispose] [End Logout client]");
            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [Dispose] [{ex.Message}]");
                this.WriteError(ex);
            }
            
        }
    }
}
