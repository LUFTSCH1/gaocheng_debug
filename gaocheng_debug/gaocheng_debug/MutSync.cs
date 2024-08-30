using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
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

        public static void HandleRunningInstance(in string processName)
        {
            Process crproc = Process.GetCurrentProcess();
            Process[] Processes = Process.GetProcessesByName(processName);
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
        }

        public static void ShowMessageToWarn(in string msg) => MessageBox.Show(msg, ConstValues.Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        public static async Task<string> GetMD5HashFromFileAsync(string fileName)
        {
            return await Task.Run(() => { 
                try
                {
                    System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    byte[] retVal = md5.ComputeHash(file);
                    file.Close();
 
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0, len = retVal.Length; i < len; ++i)
                    {
                        sb.Append(retVal[i].ToString("x2"));
                    }
                
                    return sb.ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception("获取文件MD5失败：" + ex.Message);
                }
            }).ConfigureAwait(true);
        }
    }
}
