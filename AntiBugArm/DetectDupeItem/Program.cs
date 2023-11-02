using log4net;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

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
            Console.OutputEncoding = Encoding.UTF8;
            var dirLog = string.Empty;
            if (!args.Any())
            {
                var processes = Process.GetProcessesByName("MSGServer");
                if (processes.Length == 1)
                {
                    var process = processes.FirstOrDefault();
                    dirLog = Path.Combine(Path.GetDirectoryName(process.MainModule.FileName), "GMLOG");
                }
                else if(processes.Length >1)
                {
                    foreach (var process in processes)
                    {
                        Console.WriteLine($"{process.Id} - {process.ProcessName} - {process.MainModule.FileName}");
                    }
                    Console.Write($"Vui lòng chọn ứng dụng: ");
                    var pid = Convert.ToInt32(Console.ReadLine());
                    dirLog = Path.Combine(Path.GetDirectoryName(Process.GetProcessById(pid).MainModule.FileName), "GMLOG");
                }
                else
                {
                    Console.Write("Nhập đường dẫn tới thư mục GMLOG: ");
                    dirLog = Console.ReadLine();
                }
            }
            else
            {
                dirLog = args.FirstOrDefault();
            }
            if(!Directory.Exists(dirLog))
            {
                Console.WriteLine($"Đường dẫn {dirLog} không tồn tại.");
                Console.Read();
                Environment.Exit(1);
            }
            Console.Title = dirLog;
            ItemAddition.Tracking(new FileSystemEventArgs(WatcherChangeTypes.Changed, dirLog, GetItemAddName())).Wait();
            CoreMerged.Tracking(new FileSystemEventArgs(WatcherChangeTypes.Changed, dirLog, GetCoreMergedName())).Wait();
            Console.WriteLine(dirLog);
            CreateFileWatcher(dirLog);
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
    }
}
