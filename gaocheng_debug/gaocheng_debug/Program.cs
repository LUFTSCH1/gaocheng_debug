using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace gaocheng_debug
{
    internal static class Program
    {
        private const int DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = -4;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDpiAwarenessContext(int dpiFlag);

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            {
                string process_name = Process.GetCurrentProcess().ProcessName;
                new Mutex(true, process_name, out bool isNotRunning);
                if (!isNotRunning)
                {
                    MutSync.HandleRunningInstance(process_name);
                    Environment.Exit(0);
                }
            }

            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDpiAwarenessContext(DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
