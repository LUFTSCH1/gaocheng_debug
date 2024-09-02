using System;
using System.IO;
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
            IntegralityCheckerAsync.CheckIntegralityAsync().Wait();
            if (!Directory.Exists(ConstValues.TestLogRelativePath))
            {
                Directory.CreateDirectory(ConstValues.TestLogRelativePath);
            }
            if (!File.Exists(ConstValues.InitialDirectoriesConfigRelativePath))
            {
                File.WriteAllText(ConstValues.InitialDirectoriesConfigRelativePath, $"{ConstValues.DefaultDirectory}\n{ConstValues.DefaultDirectory}");
            }

            if (Environment.OSVersion.Version.Major >= 6)
            {
                // #define DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 -4
                SetProcessDpiAwarenessContext(-4);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        // 私有静态外部方法-dpi设置
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDpiAwarenessContext(int dpiFlag);
    }
}
