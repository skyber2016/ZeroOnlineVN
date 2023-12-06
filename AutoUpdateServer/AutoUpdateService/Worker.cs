using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using SimpleTCP;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace AutoUpdateService
{
    public class Worker : BackgroundService
    {
        private readonly SimpleTcpServer _server = new SimpleTcpServer();
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public Worker()
        {
            LoggerConfigure();
            _server.DataReceived += _server_DataReceived;
            var port = 9521;
            _server.Start(port);
            Console.WriteLine($"Server started on port {port}");
        }

        private void _server_DataReceived(object sender, Message e)
        {
            //var version = Convert.ToInt32(e.MessageString);
            //var dataVersions = GetVersions();
            //var nextVersion = dataVersions.FirstOrDefault(x => x.Version > version);
            //var replyData = "READY";
            //if (nextVersion != null)
            //{
            //    replyData = $"UPDATE update.zeroonlinevn.com patches/{nextVersion.Version}.exe";
            //}
            //e.Reply(replyData);
            //_logger.Info($"Server reply to {e.TcpClient.Client.RemoteEndPoint} {replyData}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Info($"Auto update running at: {DateTimeOffset.Now}");
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        private void LoggerConfigure()
        {
            XmlDocument log4netConfig = new XmlDocument();
            using (var fs = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"))
            {
                log4netConfig.Load(fs);

                var repo = LogManager.CreateRepository(
                        Assembly.GetEntryAssembly(),
                        typeof(log4net.Repository.Hierarchy.Hierarchy));

                XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
            }
        }
    }
}
