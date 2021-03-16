using API.Configurations;
using API.Cores;
using Microsoft.Extensions.Options;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace MiddlewareTCP.Entities
{
    public class GameService : IDisposable
    {
        public string SessionId { get; set; }
        private SimpleTcpClient MidClient { get; set; }
        private SimpleTcpServer MidServer { get; set; }
        public bool IsOk
        {
            get
            {
                return this.MidServer.IsStarted;
            }
        }
        private IUnitOfWork UnitOfWork { get; set; }
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
                this.SessionId = Guid.NewGuid().ToString().Split('-').LastOrDefault();
                this.UnitOfWork = unitOfWork;
                this.MidServer = new SimpleTcpServer();
                this.MidServer.ClientConnected += GameToMid_Connected;
                this.MidServer.ClientDisconnected += GameToMid_Disconnected;
                this.MidServer.DataReceived += GameToMid_Data;
                this.MidServer.Start(AppSettings.Value.PortGameMid);

                this.MidClient = new SimpleTcpClient();
                this.MidClient.DataReceived += ServerToMid_Receiver;
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
            
        }

        private void GameToMid_Disconnected(object sender, TcpClient e)
        {
            this.Dispose();
        }

        private void WriteError(Exception ex)
        {
            if(ex.InnerException != null)
            {
                this.WriteError(ex.InnerException);
            }
            this.UnitOfWork.Logger.Error("GAME:" + ex.Message);
        }

        private void GameToMid_Data(object sender, Message e)
        {
            if (this.MidClient.TcpClient.Connected)
            {
                this.MidClient.Write(e.Data);
            }
            else
            {
                this.Game.Close();
                this.Game.Dispose();
                this.UnitOfWork.Logger.Error("MidClient is disconnected");
            }
        }

        private void GameToMid_Connected(object sender, TcpClient e)
        {
            this.Game = e;
            this.MidClient.Connect(this.AppSettings.Value.IpServer, this.AppSettings.Value.PortGameServer);
        }

        private void ServerToMid_Receiver(object sender, Message e)
        {
            if (!this.Game.Connected)
            {
                return;
            }
            this.Game.Client.Send(e.Data);
        }

        public void Dispose()
        {
            this.Game.Close();
            this.Game.Dispose();
            this.MidClient.Disconnect();
            this.MidClient.Dispose();
            this.MidServer.Stop();
        }
    }
}
