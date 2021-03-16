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
    public class LoginService
    {
        public string SessionId { get; set; }
        private SimpleTcpClient MidClient { get; set; }
        private IUnitOfWork UnitOfWork { get; set; }
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
        public string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat(" {0:x2}", b);
            return hex.ToString();
        }
        public void GameToMid_DataReceiver(object sender, Message e)
        {
            Console.WriteLine("G2M:" + ByteArrayToString(e.Data));
            this.MidClient.Write(e.Data);
        }

        public void GameToMid_Connected(object sender, TcpClient e)
        {
            this.Game = e;
            this.MidClient.Connect(this.AppSettings.Value.IpServer, this.AppSettings.Value.PortLoginServer);
        }

        public void ServerToMid_Receiver(object sender, Message e)
        {
            var data = string.Join(" ", e.Data);
            byte[] dataSend = e.Data;
            Console.WriteLine("S2M:" + ByteArrayToString(e.Data));
            var portLoginServer = string.Join(" ", Encoding.UTF8.GetBytes(this.UnitOfWork.AppSettings.Value.PortLoginServer.ToString()));
            var portLoginMid = string.Join(" ", Encoding.UTF8.GetBytes(this.UnitOfWork.AppSettings.Value.PortLoginMid.ToString()));
            if(data.Contains(portLoginServer))
            {
                dataSend = data.Replace(portLoginServer, portLoginMid).Split(' ').Select(x => Convert.ToByte(x)).ToArray();
            }
            if (!this.Game.Connected)
            {
                return;
            }
            this.Game.Client.Send(dataSend);
        }

        public void Dispose()
        {
            this.Game.Close();
            this.MidClient.Disconnect();
        }
    }
}
