using API.Configurations;
using API.Cores;
using Microsoft.Extensions.Options;
using AutoAnswer.command;
using SimpleTCP;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AutoAnswer.Entities
{
    public class LoginService : IDisposable
    {
        public string SessionId { get; set; }
        private SimpleTcpClient MidClient { get; set; }
        private IUnitOfWork UnitOfWork { get; set; }
        public string Username { get; set; }
        public byte[] secretKey { get; set; }
        private IOptions<AppSettings> AppSettings
        {
            get
            {
                return this.UnitOfWork.AppSettings;
            }
        }
        private TcpClient Game { get; set; }
        public LoginService(IUnitOfWork unitOfWork)
        {
            try
            {
                this.SessionId = Guid.NewGuid().ToString();
                this.UnitOfWork = unitOfWork;
                this.MidClient = new SimpleTcpClient();
                this.MidClient.DataReceived += ServerToMid_Receiver;
            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[{this.Username}] [LoginService] [Dispose] [Throw exception]");
                this.WriteError(ex);
            }
        }

        private void WriteError(Exception ex)
        {

            if (ex.InnerException != null)
            {
                this.WriteError(ex.InnerException);
            }
            this.UnitOfWork.Logger.Error($"[{this.Username}] [LoginService] [WriteError] [{ex.Message}]");
            this.UnitOfWork.Logger.Error($"[{this.Username}] [LoginService] [WriteError] [{ex.StackTrace}]");
        }
        public void GameToMid_DataReceiver(object sender, Message e)
        {
            var ip = e.TcpClient.GetIP();
            try
            {
                this.UnitOfWork.Logger.Info($"[{this.Username}] [LoginService] [GameToMid_DataReceiver] [Begin send data]");
                if (e.Data.GetPacketType().vnEquals(PacketContants.LoginTypeRequest))
                {
                    this.UnitOfWork.Logger.Info($"[{this.Username}] [LoginService] [GameToMid_DataReceiver] [Begin get username]");
                    this.Username = e.Data.Skip(8).Take(32).Where(x => x != 0x0).ToArray().ConvertToString();
                }
                if (this.MidClient.TcpClient.Connected)
                {
                    this.UnitOfWork.Logger.Info($"[{this.Username}] [LoginService] [GameToMid_DataReceiver] [Begin send data to mid client]");
                    this.MidClient.Write(e.Data);
                }
                else
                {
                    this.UnitOfWork.Logger.Error($"[{ip}] [{this.Username}] [LoginService] [GameToMid_DataReceiver] [Mid client is not connected]");
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[{ip}] [{this.Username}] [LoginService] [GameToMid_DataReceiver] [Throw exception]");
                this.WriteError(ex);
            }
        }

        public void GameToMid_Connected(object sender, TcpClient e)
        {
            var ip = e.GetIP();
            try
            {
                this.Game = e;
                try
                {
                    this.UnitOfWork.Logger.Info($"[{this.Username}] [LoginService] [GameToMid_Connected] [Begin connect to server]");
                    this.MidClient.Connect(this.AppSettings.Value.IpServer, this.AppSettings.Value.PortLoginServer);
                }
                catch (Exception ex)
                {
                    this.UnitOfWork.Logger.Error($"[{ip}] [{this.Username}] [LoginService] [GameToMid_Connected] [End connect to server timeout]");
                    this.WriteError(ex);
                }
            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[{ip}] [{this.Username}] [LoginService] [GameToMid_Connected] [Throw exception]");
                this.WriteError(ex);
                this.Game.Close();
                this.Game.Dispose();
            }
        }
        public void ServerToMid_Receiver(object sender, Message e)
        {
            try
            {
                var ip = e.TcpClient.GetIP();
                this.UnitOfWork.Logger.Info($"[{ip}] [{this.Username}] [LoginService] [ServerToMid_Receiver] [Begin send data to Game]");
                byte[] dataSend = e.Data;

                if (e.Data.GetPacketType(2).vnEquals(PacketContants.LoginResponse))
                {
                    this.secretKey = e.Data.Skip(8).Take(4).ToArray();
                    PacketContants.SecretKey[this.secretKey.Split()] = this.Username;
                    var data = string.Join(" ", e.Data);
                    var portLoginServer = string.Join(" ", BitConverter.GetBytes(this.UnitOfWork.AppSettings.Value.PortGameServer));
                    var portLoginMid = string.Join(" ", BitConverter.GetBytes(this.UnitOfWork.AppSettings.Value.PortGameMid));
                    var ipServer = this.UnitOfWork.AppSettings.Value.IpServer;
                    var ipMid = this.UnitOfWork.AppSettings.Value.IpMid;
                    if (data.Contains(portLoginServer))
                    {
                        dataSend = data.Replace(portLoginServer, portLoginMid).Split(' ').Select(x => Convert.ToByte(x)).ToArray();
                        dataSend = dataSend.Replace(ipServer.ToByte(), ipMid.ToByte());
                        this.UnitOfWork.Logger.Info($"[{ip}] [{this.Username}] [LoginService] [ServerToMid_Receiver] [Change port {portLoginServer} to {portLoginMid}]");
                        this.UnitOfWork.Logger.Info($"[{ip}] [{this.Username}] [LoginService] [ServerToMid_Receiver] [Change IP {ipServer} to {ipMid}]");

                    }
                }
                if (!this.Game.Client.Connected)
                {
                    this.UnitOfWork.Logger.Error($"[{ip}] [{this.Username}] [LoginService] [ServerToMid_Receiver] [Game is not connected]");
                    this.Dispose();
                    return;
                }
                this.Game.Client.Send(dataSend);
                this.UnitOfWork.Logger.Info($"[{this.Username}] [LoginService] [ServerToMid_Receiver] [End send data to Game success]");
            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[{this.Username}] [LoginService] [Dispose] [Throw exception]");

                this.WriteError(ex);
            }
        }

        public void Dispose()
        {
            try
            {
                this.UnitOfWork.Logger.Info($"[{this.Username}] [LoginService] [Dispose] [Begin dispose login service]");
                this.Game.Close();
                this.Game.Dispose();
                this.MidClient.Disconnect();
                this.MidClient.Dispose();
                this.UnitOfWork.Logger.Info($"[{this.Username}] [LoginService] [Dispose] [End dispose login service]");
            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[{this.Username}] [LoginService] [Dispose] [Throw exception]");
                this.WriteError(ex);
            }
            
        }
    }
}
