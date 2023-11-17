using HexEditor.Structures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace HexEditor
{
    internal class Robot
    {
        public int id { get; set; }
    }
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Logging.LogAllExceptions();
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            IntPtrHelper.Release();
        }

        
    }
}
