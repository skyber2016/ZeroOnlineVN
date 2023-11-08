using HexEditor.Structures;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace HexEditor
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var source = File.ReadAllBytes(@"C:\Users\duynh2\Downloads\Shop.dat");
            var wrapper = SHOP_WRAPPER.Initialize(source);
            var json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
            File.WriteAllText("shop.json", json, Encoding.GetEncoding("gb2312"));
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
