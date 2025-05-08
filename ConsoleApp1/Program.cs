using System;
using System.Runtime.Versioning;
using System.Windows.Forms;

[assembly: SupportedOSPlatform("windows")]

namespace PersonalFinanceManager
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Enable high DPI support
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}