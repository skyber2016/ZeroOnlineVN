using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ReplaceUnicode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            foreach (string arg in args)
            {
                Console.WriteLine(arg);
            }
            if (!args.Any())
            {
                return;
            }
            var pathToFile = args[0];
            var sourceBytes = File.ReadAllBytes(pathToFile);
            var cp1258 = Encoding.GetEncoding(1258);
            var text = cp1258.GetString(sourceBytes);
            text = StringHelper.RemoveDiacritics(text);
            File.WriteAllText(pathToFile, text, cp1258);
            Console.WriteLine("DONE!!!");
        }
    }
}
