using System;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace gaocheng_debug
{
    internal static class MutSync
    {
        //防止文件夹重复打开
        [DllImport("shell32.dll")]
        private static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters = "", string lpDirectory = "", int show_style = 1);

        //防止应用重复打开
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void OpenFolder(in string fileFullName) => ShellExecute(IntPtr.Zero, "open", fileFullName);

        public static void HandleRunningInstance(in string process_name)
        {
            Process crproc = Process.GetCurrentProcess();
            Process[] Processes = Process.GetProcessesByName(process_name);
            foreach (Process proc in Processes)
            {
                if (proc.Id != crproc.Id && Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == crproc.MainModule.FileName)
                {
                    crproc = proc;
                    break;
                }
            }

            ShowWindowAsync(crproc.MainWindowHandle, 1);
            SetForegroundWindow(crproc.MainWindowHandle);

            return ;
        }

        public static void ShowMessageToWarn(in string msg) => MessageBox.Show(msg, ConstValues.WARNING, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
