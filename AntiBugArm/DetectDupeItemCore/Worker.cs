using DetectDupeItem.Services;
using DetectDupeItemCore.Configurations;
using DetectDupeItemCore.Services;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace DetectDupeItemCore
{
    internal class Worker : BackgroundService
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string ItemAdditionLog = "itemaddition_log";
        private string CoreMergedLog = "TrumpAssistantFunctionCombine";
        private string GetItemAddName() => $"{ItemAdditionLog} {DateTime.Now.ToString("yyyy-M-d")}.log";
        private string GetCoreMergedName() => $"{CoreMergedLog} {DateTime.Now.AddDays(-1).ToString("yyyy-M-d")}.log";
        public Worker(IOptions<AppSettings> options)
        {
            GmLogService.BaseAddress = new Uri(options.Value.GMLOG);
            DatabaseService.BaseAddress = new Uri(options.Value.DatabaseHost);
            this.LoggerConfigure();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested)
            {
                await CoreMerged.Tracking(GetCoreMergedName());
                await Task.Delay(5000);
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
