using AutoPatchesApi.Models;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace AutoPatchesApi
{
    public class Program
    {
        private static readonly SimpleTcpServer _server = new SimpleTcpServer();
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void Main(string[] args)
        {
            LoggerConfigure();
            _server.DataReceived += _server_DataReceived; ;
            var port = 9521;
            _server.Start(port);
            Console.WriteLine($"Server started on port {port}");
            CreateHostBuilder(args).Build().Run();
        }

        private static List<VersionConfiguration> GetVersions()
        {
            var json = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "patches.json"));
            return JsonConvert.DeserializeObject<List<VersionConfiguration>>(json);
        }

        private static void _server_DataReceived(object sender, Message e)
        {
            try
            {
                _logger.Info("----------------------------------------------------------------------------------");
                _logger.Info($"{e.TcpClient.Client.RemoteEndPoint} sent {e.MessageString}");
                var version = Convert.ToInt32(e.MessageString);
                var dataVersions = GetVersions().OrderBy(x => x.Version).ToList();
                var nextVersion = dataVersions.FirstOrDefault(x => x.Version > version);
                var replyData = "READY";
                if (nextVersion != null)
                {
                    replyData = $"UPDATE update.zeroonlinevn.com patches/{nextVersion.File}";
                }
                e.Reply(replyData);
                _logger.Info($"Server reply to {e.TcpClient.Client.RemoteEndPoint} {replyData}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Server error {ex.GetBaseException().Message}");
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

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
