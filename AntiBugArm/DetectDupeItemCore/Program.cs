using log4net;
using log4net.Config;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace DetectDupeItem
{
    internal class Program
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string ItemAdditionLog = "itemaddition_log";
        private static string CoreMergedLog = "TrumpAssistantFunctionCombine";
        private static string GetItemAddName() => $"{ItemAdditionLog} {DateTime.Now.ToString("yyyy-M-d")}.log";
        private static string GetCoreMergedName() => $"{CoreMergedLog} {DateTime.Now.AddDays(-1).ToString("yyyy-M-d")}.log";
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: DetectDupeItemCore.dll <input_directory> <api_url>");
                return;
            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    Console.WriteLine($"ARG-{i}: {args[i]}");
                }
            }
            LoggerConfigure();
            Console.OutputEncoding = Encoding.UTF8;
            var dirLog = args[0];
            if(!Directory.Exists(dirLog))
            {
                Console.WriteLine($"Đường dẫn {dirLog} không tồn tại.");
                Console.Read();
                Environment.Exit(1);
            }
            Console.Title = "PATH_LOG "+ dirLog;
            ItemAddition.Tracking(new FileSystemEventArgs(WatcherChangeTypes.Changed, dirLog, GetItemAddName())).Wait();
            CoreMerged.Tracking(new FileSystemEventArgs(WatcherChangeTypes.Changed, dirLog, GetCoreMergedName())).Wait();
            Console.WriteLine($"Started detect GMLOG {dirLog} at {DateTime.Now}");
            CreateFileWatcher(dirLog);
            _logger.Info($"Application started at {DateTime.Now}");
            Console.ReadLine();
        }



        private static void CreateFileWatcher(string path)
        {
            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Path = path;
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
            fileSystemWatcher.Filter = "*.log";
            fileSystemWatcher.Changed += Watcher_Changed;
            fileSystemWatcher.EnableRaisingEvents = true;
            _logger.Info((object)("Added watching file " + path));
        }

        private static void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                if (e.Name.StartsWith(ItemAdditionLog))
                {
                    ItemAddition.Tracking(e).Wait();
                }
                else if (e.Name.StartsWith(CoreMergedLog))
                {
                    CoreMerged.Tracking(e).Wait();
                }
            }
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
