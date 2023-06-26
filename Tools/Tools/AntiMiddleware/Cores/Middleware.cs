using API.Configurations;
using API.Cores;
using API.Helpers;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MiddlewareTCP.Services;
using System;
using System.IO;
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
        public static object Locking { get; set; }
        private MiddlewareService MiddlewareService { get; set; }
        public Middleware(
            IOptions<AppSettings> options,
            IUnitOfWork unitOfWork,
            ILoggerManager logger
            )
        {
            if(Locking == null)
            {
                Locking = new object();
            }
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
                try
                {
                    if (this.MiddlewareService == null)
                    {
                        this.MiddlewareService = new MiddlewareService(UnitOfWork);
                        this.Logger.Status("Middleware started ");
                    }
                    else if (!this.MiddlewareService.GameServer.IsStarted && !this.MiddlewareService.LoginServer.IsStarted)
                    {
                        this.MiddlewareService = new MiddlewareService(UnitOfWork);
                    }
                    if (this.MiddlewareService.LoginServer.IsStarted)
                    {
                        this.UnitOfWork.Logger.Status($"LOGIN SERVER: ONLINE");
                    }
                    else
                    {
                        this.UnitOfWork.Logger.Status($"LOGIN SERVER: OFFLINE");
                    }
                    if (this.MiddlewareService.GameServer.IsStarted)
                    {
                        this.UnitOfWork.Logger.Status($"GAME SERVER: ONLINE");
                    }
                    else
                    {
                        this.UnitOfWork.Logger.Status($"GAME SERVER: OFFLINE");
                    }
                }
                catch (Exception ex)
                {
                    this.UnitOfWork.Logger.Error(ex.Message);
                    this.UnitOfWork.Logger.Error(ex.StackTrace);
                }
                await Task.Delay(30000, stoppingToken);
            }
            if(this.MiddlewareService != null)
            {
                this.MiddlewareService.Dispose();
            }
        }
    }
}
