using DetectDupeItem.Properties;
using DetectDupeItem.Services;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DetectDupeItem
{
    internal class Program
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string ItemAdditionLog = "itemaddition_log";
        private static string CoreMergedLog = "TrumpAssistantFunctionCombine";
        private static string GetItemAddName() => $"{ItemAdditionLog} {DateTime.Now.ToString("yyyy-M-d")}.log";
        private static string GetCoreMergedName() => $"{CoreMergedLog} {DateTime.Now.AddDays(-1).ToString("yyyy-M-d")}.log";
        static void Main(string[] args)
        {
            var dirLog = string.Empty;
            if (!args.Any())
            {
                var processes = Process.GetProcessesByName("MSGServer");
                if (processes.Length == 1)
                {
                    var process = processes.FirstOrDefault();
                    dirLog = Path.Combine(Path.GetDirectoryName(process.MainModule.FileName), "GMLOG");
                }
                else
                {
                    foreach (var process in processes)
                    {
                        Console.WriteLine($"{process.Id} - {process.ProcessName} - {process.MainModule.FileName}");
                    }
                    Console.Write($"Vui lòng chọn ứng dụng: ");
                    var pid = Convert.ToInt32(Console.ReadLine());
                    dirLog = Path.Combine(Path.GetDirectoryName(Process.GetProcessById(pid).MainModule.FileName), "GMLOG");
                }
            }
            else
            {
                dirLog = args.FirstOrDefault();
            }
            Console.Title = dirLog;
            //ItemAddition.Tracking(new FileSystemEventArgs(WatcherChangeTypes.Changed, dirLog, "itemaddition_log 2023-7-3.log")).Wait();
            CoreMerged.Tracking(new FileSystemEventArgs(WatcherChangeTypes.Changed, dirLog, GetCoreMergedName())).Wait();
            Console.WriteLine(dirLog);
            CreateFileWatcher(dirLog);
            Console.ReadLine();
        }

        private static void CreateFileWatcher(string path)
        {
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher
            {
                Path = path,
                /* Watch for changes in LastAccess and LastWrite times, and 
                   the renaming of files or directories. */
                NotifyFilter = NotifyFilters.LastWrite,
                // Only watch text files.
                Filter = "*.log"
            };

            // Add event handlers.
            watcher.Changed += Watcher_Changed;

            // Begin watching.
            watcher.EnableRaisingEvents = true;
            _logger.Info($"Added watching file {path}");
        }

        private static void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            if (e.Name.StartsWith(ItemAdditionLog))
            {
                ItemAddition.Tracking(e).Wait();
                return;
            }
            if (e.Name.StartsWith(CoreMergedLog))
            {
                CoreMerged.Tracking(e);
            }
        }
    }
}
