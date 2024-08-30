using System;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;

namespace gaocheng_debug
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        public static void Main()
        {
            { // 防多开
                string process_name = Process.GetCurrentProcess().ProcessName;
                new Mutex(true, process_name, out bool is_not_running);
                if (!is_not_running)
                {
                    MutSync.HandleRunningInstance(process_name);
                    Environment.Exit(1);
                }
            }

            // 完整性检查
            IntegralityCheckerAsync.CheckIntegrality().ConfigureAwait(true);

            if (Environment.OSVersion.Version.Major >= 6)
            {
                // #define DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 -4
                SetProcessDpiAwarenessContext(-4);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        // dpi设置
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDpiAwarenessContext(int dpiFlag);
    }
}
