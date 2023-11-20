using DetectDupeItem.Services;
using DetectDupeItemCore.Configurations;
using DetectDupeItemCore.Services;
using DetectDupeItemWorker.Events;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
        private readonly string ItemAdditionLog = "itemaddition_log";
        private readonly string CayThongLog = "CAYTHONG.log";
        private readonly string CoreMergedLog = "TrumpAssistantFunctionCombine";
        private string NowTime => DateTime.Now.ToString("yyyy-M-d");
        private string GetItemAddName() => $"{ItemAdditionLog} {NowTime}.log";
        private string GetCayThongName() => $"{CayThongLog} {NowTime}.log";
        private string GetCoreMergedName() => $"{CoreMergedLog} {NowTime}.log";
        public Worker(IOptions<AppSettings> options)
        {
            GmLogService.BaseAddress = new Uri(options.Value.GMLOG);
            DatabaseService.BaseAddress = new Uri(options.Value.DatabaseHost);
            WinService.BaseAddress = new Uri(options.Value.DatabaseHost);
            this.LoggerConfigure();
            _logger.Info($"Application started at {DateTime.Now}");
        }
        private async Task CoreMergedTimerHandle()
        {
            try
            {
                await CoreMerged.Tracking(GetCoreMergedName());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.GetBaseException().Message);
            }
        }
        private async Task ItemAdditionTimerHandle()
        {
            try
            {
                await ItemAddition.Tracking(GetItemAddName());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.GetBaseException().Message);
            }
        }
        private async Task CayThongTimerHandle()
        {
            try
            {
                await CayThong.Tracking(GetCayThongName());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.GetBaseException().Message);
            }
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var tasks = new List<Task>
                {
                    ExecuteTimer(CayThongTimerHandle, 2000, stoppingToken),
                    ExecuteTimer(ItemAdditionTimerHandle, 5000, stoppingToken),
                    ExecuteTimer(CoreMergedTimerHandle, 5000, stoppingToken)
                };
                Task.WaitAll(tasks.ToArray());
                await Task.Delay(1000, stoppingToken);
            }
        }

        private Task ExecuteTimer(Func<Task> callback, int interval, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await callback();
                    await Task.Delay(interval);
                }
            }, cancellationToken);
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
