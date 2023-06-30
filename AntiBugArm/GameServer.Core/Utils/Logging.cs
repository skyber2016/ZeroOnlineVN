using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GameServer.Core.Utils
{
    public class Logging
    {
        public static Logging Instance => new Logging();
        #region  Methods
        private string _assemblyName { get; set; }
        public void Setup(string assemblyName)
        {
            _assemblyName = assemblyName;
        }
        public WriteDelegate Write([CallerMemberName] string memberName = "") =>
            (value, args) =>
            {
                object finalMessage = value;
                if (args.Length > 0)
                    try
                    {
                        finalMessage = string.Format(value.ToString(), args);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.GetBaseException().Message);
                    }

                Write(finalMessage.ToString(), memberName: memberName);
            };

        public WriteDelegate WriteData([CallerMemberName] string memberName = "") =>
            (value, args) =>
            {
                object finalMessage = value;
                if (args.Length > 0)
                    try
                    {
                        finalMessage = string.Format(value.ToString(), args);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.GetBaseException().Message);
                    }

                Write(finalMessage.ToString(), memberName: memberName, "data_{0}.log");
            };

        public void Write(object value, object[] args, [CallerMemberName] string memberName = "")
        {
            object finalMessage = value;
            if (args.Length > 0)
                try
                {
                    finalMessage = string.Format(value.ToString(), args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException().Message);
                }

            Write(finalMessage.ToString(), memberName);
        }

        public void LogAllExceptions()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                if (args.ExceptionObject is Exception exception)
                {
                    Write()(exception.Message);
                }
            };
        }
        private static readonly object o = new object();
        private void Write(string message, string memberName, string fileName = "info_{0}.log")
        {
            fileName = string.Format(fileName, _assemblyName + "-" + DateTime.Now.ToString("yyyy_MM_dd"));
            string format = $"[{DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")}]: [{memberName.PadLeft(30)}] -> {message}";

            try
            {
                var dir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "logs");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                Console.WriteLine(format);
                lock (o)
                {
                    using (StreamWriter writer = new StreamWriter(Path.Combine(dir, fileName), true))
                    {
                        writer.WriteLine(format);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException().Message);
            }
        }

        #endregion

        #region Nested Types, Enums, Delegates

        public delegate void WriteDelegate(object value, params object[] args);

        #endregion
    }
}
