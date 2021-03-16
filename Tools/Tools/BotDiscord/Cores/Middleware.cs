using API.Configurations;
using API.Cores;
using API.Helpers;
using API.Services.Interfaces;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MiddlewareTCP.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace BotDiscord
{
    public class Middleware : BackgroundService
    {
        public IOptions<AppSettings> AppSettings { get; set; }
        public ILoggerManager Logger { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }

        private MiddlewareService MiddlewareService { get; set; }
        public Middleware(
            IOptions<AppSettings> options,
            IUnitOfWork unitOfWork,
            ILoggerManager logger
            )
        {
            this.AppSettings = options;
            this.UnitOfWork = unitOfWork;
            this.Logger = logger;
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if(this.MiddlewareService == null)
                {
                    this.Logger.Info("Middleware started");
                    this.MiddlewareService = new MiddlewareService(UnitOfWork);
                }
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
