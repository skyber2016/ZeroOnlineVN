using log4net;
using log4net.Config;
using SimpleTCP;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace AutoUpdateServer
{
    internal class Program
    {
        private static readonly SimpleTcpServer _server = new SimpleTcpServer();
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            LoggerConfigure();
            _server.DataReceived += _server_DataReceived;
            var port = 9521;
            _server.Start(port);
            Console.WriteLine($"Server started on port {port}");
            Console.ReadLine();
        }

        private static void _server_DataReceived(object sender, Message e)
        {
            _logger.Info($"{e.TcpClient.Client.RemoteEndPoint} sent {e.MessageString}");
            var version = Convert.ToInt32(e.MessageString);
            e.Reply("UPDATE 103.188.166.96:9520 Patches/1001.exe");
            _logger.Info($"Server reply {e.MessageString}");
        }
        private static void LoggerConfigure()
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
