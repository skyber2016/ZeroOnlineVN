using Core.Utils;
using System;
using System.IO;
using System.Reflection;

namespace LoginServer
{
    public class Program
    {
        public static readonly Logging Logging = Logging.Instance;
        static void Main(string[] args)
        {
            Logging.Setup(Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location));
            Logging.LogAllExceptions();
            Server.Start();
            while (Console.ReadLine() != "Exit")
            {
            }
        }

    }
}
