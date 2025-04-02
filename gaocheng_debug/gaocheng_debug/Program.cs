using System;
using System.IO;
using System.Text;
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
            // 阻止多开
            StaticTools.HandleRunningInstance();

            // 完整性检查
            StaticTools.CheckIntegrality();
            if (!Directory.Exists(Global.ProjectDirectoryRelativePath))
            {
                Directory.CreateDirectory(Global.ProjectDirectoryRelativePath);
            }
            if (!File.Exists(Global.DefaultSettingsRelativePath))
            {
                StaticTools.WriteAllText(Global.DefaultSettingsRelativePath, $"{Global.DefaultDirectory}\n{Global.DefaultDirectory}", Encoding.UTF8);
            }
            if (!File.Exists(Global.ProjectDirectoryLockRelativePath))
            {
                StaticTools.WriteAllText(Global.ProjectDirectoryLockRelativePath, string.Empty, Encoding.UTF8);
            }

            // 完整则创建文件锁以锁定文件
            List<FileStream> file_locks = StaticTools.LockFilesAndDisposeFileList();

            // 设置DPI感知
            if (Environment.OSVersion.Version.Major >= 6)
            {
                // #define DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 ((DPI_AWARENESS_CONTEXT)-4)
                SetProcessDpiAwarenessContext(-4);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            // 释放文件锁
            for (int i = 0, len = file_locks.Count; i < len; ++i)
            {
                file_locks[i].Close();
                file_locks[i] = null;
            }
        }

        // 私有静态外部方法-DPI感知设置
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDpiAwarenessContext(int dpiFlag);
    }
}
