using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

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
            { // 阻止多开
                string process_name = Process.GetCurrentProcess().ProcessName;
                new Mutex(true, process_name, out bool is_not_running);
                if (!is_not_running)
                {
                    MutSync.HandleRunningInstance(process_name);
                    Environment.Exit((int)ErrorCode.ApplicationAlreadyRunning);
                }
            }

            // 完整性检查
            MutSync.CheckIntegrality();
            if (!Directory.Exists(Global.ProjectDirectoryRelativePath))
            {
                Directory.CreateDirectory(Global.ProjectDirectoryRelativePath);
            }
            if (!File.Exists(Global.InitialDirectoriesConfigRelativePath))
            {
                File.WriteAllText(Global.InitialDirectoriesConfigRelativePath, $"{Global.DefaultDirectory}\n{Global.DefaultDirectory}");
            }
            if (!File.Exists(Global.ProjectDirectoryLockRelativePath))
            {
                File.WriteAllText(Global.ProjectDirectoryLockRelativePath, "protector");
            }

            // 完整则创建文件锁以锁定文件
            List<FileStream> fileLocks = MutSync.LockFilesThenDisposeFileList();

            // 设置DPI感知
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDpiAwarenessContext(-4); // #define DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 ((DPI_AWARENESS_CONTEXT)-4)
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            // 释放文件锁
            for (int i = 0, len = fileLocks.Count; i < len; ++i)
            {
                fileLocks[i].Close();
            }
        }

        // 私有静态外部方法-DPI感知设置
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDpiAwarenessContext(int dpiFlag);
    }
}
