using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace gaocheng_debug
{
    internal static class MutSync
    {
        // 私有类声明
        private class FileListItem
        {
            public readonly string FileName;
            public readonly string MD5Hash;

            public FileListItem(in string fileName, in string hash)
            {
                FileName = fileName;
                MD5Hash = hash;
            }
        }

        // 私有常量
        private const int MD5HashStringLength = 32;
        private const string HashConvertFormatStr = "x2";

        // 私有静态只读成员-顺序调用，不得冲突
        private static readonly MD5 MD5 = MD5.Create();
        private static readonly StringBuilder MD5StringBuilder = new StringBuilder(MD5HashStringLength);

        // 私有成员-检查完整性与加文件锁后释放，故不加readonly
        private static FileListItem[] FileList =
        {
            new FileListItem(@".\gaocheng_debug.exe.config"                         , "98dbb4a9bc384dca6b79a47886c42891"),
            new FileListItem(@".\System.Buffers.dll"                                , "ecdfe8ede869d2ccc6bf99981ea96400"),
            new FileListItem(@".\System.Memory.dll"                                 , "f09441a1ee47fb3e6571a3a448e05baf"),
            new FileListItem(@".\System.Numerics.Vectors.dll"                       , "aaa2cbf14e06e9d3586d8a4ed455db33"),
            new FileListItem(@".\System.Runtime.CompilerServices.Unsafe.dll"        , "c610e828b54001574d86dd2ed730e392"),
            new FileListItem(@".\System.Text.Encoding.CodePages.dll"                , "2c9e9cd5c6f31ebfdc8155efdc20f4f7"),
            new FileListItem($".\\{Global.ResourceDirectory}\\{Global.GetInputData}", "0575ba8c3fcd1bd3fe7f2409325a3f98"),
            new FileListItem($".\\{Global.ResourceDirectory}\\msvcp140d.dll"        , "a66eaf437d3d8c53a127f77b2a896f0d"),
            new FileListItem($".\\{Global.ResourceDirectory}\\{Global.TxtCompare}"  , "665beeefe858a15c1f3d531baa64ee0d"),
            new FileListItem($".\\{Global.ResourceDirectory}\\ucrtbased.dll"        , "4d98940874d14692b02ece8f5b591362"),
            new FileListItem($".\\{Global.ResourceDirectory}\\vcruntime140d.dll"    , "b907335a3619259f8aaf22c445de15ce")
        };

        // 公有静态方法
        public static void OpenFolder(in string fileFullName) => ShellExecute(IntPtr.Zero, "open", fileFullName);

        public static void HandleRunningInstance()
        {
            Process crproc = Process.GetCurrentProcess();
            int cr_id = crproc.Id;
            string cr_file_name = crproc.MainModule.FileName;
            Process[] processes = Process.GetProcessesByName(crproc.ProcessName);

            foreach (Process proc in processes)
            {
                if (proc.Id != cr_id && Assembly.GetExecutingAssembly().Location.Replace('/', '\\') == cr_file_name)
                {
                    ShowWindowAsync(proc.MainWindowHandle, 1);
                    SetForegroundWindow(proc.MainWindowHandle);
                    Environment.Exit((int)ErrorCode.ApplicationAlreadyRunning);
                }
            }
        }

        public static void ShowMessageToWarn(in string msg) => MessageBox.Show(msg, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        public static FileStream NewReadOnlyFileHandle(in string fileName) => new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

        // 此方法及调用此方法的其它方法都不得被异步/并行方法调用
        public static string MD5Hash(in string fileName)
        {
            try
            {
                FileStream file = NewReadOnlyFileHandle(fileName);
                byte[] hash_bytes = MD5.ComputeHash(file);
                file.Close();
                MD5.Initialize();

                MD5StringBuilder.Length = 0;
                for (int i = 0, len = hash_bytes.Length; i < len; ++i)
                {
                    MD5StringBuilder.Append(hash_bytes[i].ToString(HashConvertFormatStr));
                }

                return MD5StringBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("获取文件哈希失败" + ex.Message);
            }
        }

        // 此方法会创建更多对象，但更加安全，提供给异步/并行方法使用
        public static string GetMD5HashFromFile(in string fileName)
        {
            try
            {
                using (MD5 md5 = MD5.Create())
                {
                    FileStream file = NewReadOnlyFileHandle(fileName);
                    byte[] hash_bytes = md5.ComputeHash(file);
                    file.Close();

                    StringBuilder sb = new StringBuilder(MD5HashStringLength);
                    for (int i = 0, len = hash_bytes.Length; i < len; ++i)
                    {
                        sb.Append(hash_bytes[i].ToString(HashConvertFormatStr));
                    }

                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static List<FileStream> LockFilesAndDisposeFileList()
        {
            List<FileStream> locks = new List<FileStream>();
            for (int i = 0, len = FileList.Length; i < len; ++i)
            {
                locks.Add(NewReadOnlyFileHandle(FileList[i].FileName));
                FileList[i] = null;
            }
            locks.Add(NewReadOnlyFileHandle(Global.ProjectDirectoryLockRelativePath));
            FileList = null;

            return locks;
        }

        // 完整性并行检查
        public static void CheckIntegrality()
        {
            const long MaxFileSize = 2 * 1024 * 1024;

            if (!Directory.Exists($".\\{Global.ResourceDirectory}"))
            {
                ShowMessageToWarn($"缺少{Global.ResourceDirectory}文件夹，请考虑重新下载应用");
                Environment.Exit((int)ErrorCode.NecessaryFileNotFound);
            }

            Parallel.ForEach(FileList, file =>
            {
                if (!File.Exists(file.FileName))
                {
                    ShowMessageToWarn($"应用程序相对路径下必要的\n{file.FileName}\n文件缺失，请检查回收站或考虑重新下载应用");
                    Environment.Exit((int)ErrorCode.NecessaryFileNotFound);
                }
                else if(new FileInfo(file.FileName).Length > MaxFileSize || file.MD5Hash != GetMD5HashFromFile(file.FileName))
                {
                    ShowMessageToWarn($"应用程序相对路径下必要的\n{file.FileName}\n文件被替换或其它原因导致哈希计算失败");
                    Environment.Exit((int)ErrorCode.NecessaryFileReplaced);
                }
            });
        }

        // 私有静态外部方法
        [System.Runtime.InteropServices.DllImport("shell32.dll")]
        private static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters = "", string lpDirectory = "", int show_style = 1);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
