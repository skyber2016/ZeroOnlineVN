using API.Configurations;
using API.Cores;
using API.Helpers;
using AutoAnswer.Services;
using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace AutoZero
{
    class Program
    {
        public static AppSettings AppSettings { get; set; }
        public static ILoggerManager Logger { get; set; }
        public static IUnitOfWork UnitOfWork { get; set; }
        private static MiddlewareService MiddlewareService { get; set; }
        static void Main(string[] args)
        {
            UnitOfWork = new UnitOfWork();
            Logger = UnitOfWork.Logger;
            Logger.Info($"LOGIN SERVER: ONLINE");
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

            if (MiddlewareService == null)
            {
                Logger.Status("Middleware started ");
                MiddlewareService = new MiddlewareService(UnitOfWork);
            }
            Console.ReadLine();
        }
    }
}
