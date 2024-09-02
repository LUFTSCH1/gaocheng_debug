using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Globalization;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace gaocheng_debug
{
    internal static class MutSync
    {
        private static DateTime dT;

        // 公有静态方法
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

        public static bool CheckProjectNameIegitimacy(in string projectName)
        {
            return DateTime.TryParseExact(projectName, ConstValues.ProjectNameFormatStr, ConstValues.InvariantCulture, DateTimeStyles.None, out dT) && DateTime.Now >= dT;
        }

        public static void ShowMessageToWarn(in string msg) => MessageBox.Show(msg, ConstValues.Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        public static string PartHashWithSalt(in string fileName)
        {
            try
            {
                byte[] bytes_with_salt = Encoding.UTF8.GetBytes(ConstValues.HashSalt + MD5Hash(fileName));
                ConstValues.MD5.Initialize();
                byte[] hash_bytes = ConstValues.MD5.ComputeHash(bytes_with_salt);

                string rs = string.Empty;
                for (int i = 0; i < 8; ++i)
                {
                    rs += hash_bytes[i].ToString(ConstValues.HashConvertFormatStr);
                }

                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception("获取文件哈希失败" + ex.Message);
            }
        }

        // 公有静态异步方法
        public static async Task<string> GetMD5HashFromFileAsync(string fileName)
        {
            return await Task.Run(() => { 
                try
                {
                    using (MD5 md5 = MD5.Create())
                    {
                        FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        byte[] retVal = md5.ComputeHash(file);
                        file.Close();
 
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0, len = retVal.Length; i < len; ++i)
                        {
                            sb.Append(retVal[i].ToString(ConstValues.HashConvertFormatStr));
                        }
                
                        return sb.ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("获取文件MD5失败：" + ex.Message);
                }
            }).ConfigureAwait(true);
        }

        // 私有静态外部方法
        [System.Runtime.InteropServices.DllImport("shell32.dll")]
        private static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters = "", string lpDirectory = "", int show_style = 1);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        // 私有静态方法
        private static string MD5Hash(in string fileName)
        {
            try
            {
                ConstValues.MD5.Initialize();

                FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                byte[] retVal = ConstValues.MD5.ComputeHash(file);
                file.Close();
            
                StringBuilder sb = new StringBuilder();
                for (int i = 0, len = retVal.Length; i < len; ++i)
                {
                    sb.Append(retVal[i].ToString(ConstValues.HashConvertFormatStr));
                }

                return sb.ToString();
            }
            catch(Exception ex)
            {
                throw new Exception("获取文件哈希失败" + ex.Message);
            }
        }
    }
}
