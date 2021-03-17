using API.Configurations;
using API.Cores;
using Microsoft.Extensions.Options;
using MiddlewareTCP.command;
using SimpleTCP;
using System;
using System.Linq;
using System.Net.Sockets;

namespace MiddlewareTCP.Entities
{
    public class LoginService
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
                this.SessionId = Guid.NewGuid().ToString().Split('-').LastOrDefault();
                this.UnitOfWork = unitOfWork;
                this.MidClient = new SimpleTcpClient();
                this.MidClient.DataReceived += ServerToMid_Receiver;
                this.UnitOfWork.Logger.Info("LOGIN IS STARTED");
            }
            catch (Exception ex)
            {
                this.WriteError(ex);
            }
        }

        private void WriteError(Exception ex)
        {
            if(ex.InnerException != null)
            {
                WriteError(ex.InnerException);
            }
            this.UnitOfWork.Logger.Error("LOGIN: " + ex.Message);
        }
        public void GameToMid_DataReceiver(object sender, Message e)
        {
            try
            {
                if (e.Data.GetPacketType().vnEquals(PacketContants.LoginTypeRequest))
                {
                    this.Username = e.Data.Skip(8).Take(32).Where(x => x != 0x0).ToArray().ConvertToString();
                }
                this.MidClient.Write(e.Data);
            }
            catch (Exception ex)
            {
                this.WriteError(ex);
            }
        }

        public void GameToMid_Connected(object sender, TcpClient e)
        {
            try
            {
                this.Game = e;
                this.MidClient.TimeOut = TimeSpan.FromSeconds(5);
                this.MidClient.Connect(this.AppSettings.Value.IpServer, this.AppSettings.Value.PortLoginServer);
            }
            catch (Exception ex)
            {
                this.WriteError(ex);
                this.Game.Close();
                this.Game.Dispose();
            }
        }
        public void ServerToMid_Receiver(object sender, Message e)
        {
            try
            {
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
                    }
                }
                if (!this.Game.Connected)
                {
                    return;
                }
                this.Game.Client.Send(dataSend);
            }
            catch (Exception ex)
            {
                this.WriteError(ex);
            }
        }

        public void Dispose()
        {
            try
            {
                this.Game.Close();
                this.MidClient.Disconnect();
            }
            catch (Exception ex)
            {
                this.WriteError(ex);
            }
            
        }
    }
}
