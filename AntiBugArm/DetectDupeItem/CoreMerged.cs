using DetectDupeItem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DetectDupeItem
{
    internal class CoreMerged
    {

        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Encoding _encoding;
        public static async Task Tracking(FileSystemEventArgs e)
        {
            if (_encoding == null)
            {
                _encoding = Encoding.GetEncoding("GB2312");
            }
            var pathToFile = e.FullPath;
            var lastLine = 0;
            var lines = File.ReadLines(pathToFile, _encoding).Skip(lastLine).GetEnumerator();
            while (lines.MoveNext())
            {
                ++lastLine;
                var primary = lines.Current;
                if (!primary.StartsWith("主法宝的"))
                {
                    continue;
                }
                lines.MoveNext();
                ++lastLine;
                var second = lines.Current;
                if (!second.StartsWith("辅助法宝的"))
                {
                    continue;
                }
                lines.MoveNext();
                ++lastLine;
                var third = lines.Current;
                if (!third.StartsWith("合成后主法宝的[玩家名字"))
                {
                    continue;
                }
                var item = await GetItem(primary, second, third);
            }
        }

        static async Task<Item> GetItems(string primary, string second, string third)
        {
            return null;
        }

        static Item GetItem(string input)
        {
            Regex regex = new Regex(@"\d+");
            MatchCollection matches = regex.Matches(input);
            if(matches.Count > 0)
            {
                var item = new Item();
            }
            return null;
        }
    }
}
