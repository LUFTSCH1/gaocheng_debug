using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
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
        private const int SHA256HashStringLength = 64;

        private const double MaxHidePart = 0.6;

        private const string HashConvertFormatStr = "x2";

        private const string HashSalt = "到底是什么呢？";

        private const string ConfirmTitle = "操作确认";

        private const string GetMD5ErrorStr    = "获取文件哈希失败，是否重试？\n错误信息：";
        private const string ReadFileErrorStr  = "读取文件失败，是否重试？\n错误信息：";
        private const string WriteFileErrorStr = "写入文件失败，是否重试？\n错误信息：";

        // 私有静态只读成员-顺序调用，不得冲突
        private static readonly MD5 MD5 = MD5.Create();
        private static readonly StringBuilder MD5StringBuilder = new StringBuilder(MD5HashStringLength);

        private static readonly MD5 MD5ForFile = MD5.Create();
        private static readonly SHA1 SHA1ForFile = SHA1.Create();
        private static readonly SHA256 SHA256ForFile = SHA256.Create();
        private static readonly StringBuilder HashStringBuilder = new StringBuilder(SHA256HashStringLength);

        // 私有成员-检查完整性与加文件锁后释放，故不加readonly
        private static FileListItem[] FileList = {
            new FileListItem(@".\gaocheng_debug.exe.config"                         , "d18c574c943a73b85361040965c28e13"),
            new FileListItem(@".\System.Buffers.dll"                                , "ecdfe8ede869d2ccc6bf99981ea96400"),
            new FileListItem(@".\System.Memory.dll"                                 , "f09441a1ee47fb3e6571a3a448e05baf"),
            new FileListItem(@".\System.Numerics.Vectors.dll"                       , "aaa2cbf14e06e9d3586d8a4ed455db33"),
            new FileListItem(@".\System.Runtime.CompilerServices.Unsafe.dll"        , "c610e828b54001574d86dd2ed730e392"),
            new FileListItem(@".\System.Text.Encoding.CodePages.dll"                , "2c9e9cd5c6f31ebfdc8155efdc20f4f7"),
            new FileListItem($".\\{Global.ResourceDirectory}\\{Global.GetInputData}", "89f356fa5854ba3a0dbdd69835decf86"),
            new FileListItem($".\\{Global.ResourceDirectory}\\{Global.TxtCompare}"  , "d7a6a869781090b13ffeb05e849b0e74"),
            new FileListItem($".\\{Global.ResourceDirectory}\\msvcp140d.dll"        , "a66eaf437d3d8c53a127f77b2a896f0d"),
            new FileListItem($".\\{Global.ResourceDirectory}\\ucrtbased.dll"        , "4d98940874d14692b02ece8f5b591362"),
            new FileListItem($".\\{Global.ResourceDirectory}\\vcruntime140d.dll"    , "b907335a3619259f8aaf22c445de15ce")
        };

        // 公有静态方法
        public static void OpenFolder(in string fileFullName) =>
            ShellExecute(IntPtr.Zero, "open", fileFullName);

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

        public static void ShowMessageToWarn(in string msg) =>
            MessageBox.Show(msg, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        public static bool CheckOperation(in string msg, in MessageBoxIcon icon = MessageBoxIcon.Information,
                                          in string title = ConfirmTitle,
                                          in MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button2) =>
            MessageBox.Show(msg, title, MessageBoxButtons.YesNo, icon, defaultButton) == DialogResult.Yes;

        public static FileStream NewReadOnlyFileHandle(in string fileName)
        {
            while (true)
            {
                try
                {
                    return new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                }
                catch (Exception ex)
                {
                    if (!CheckOperation($"锁定文件失败，是否重试？\n错误信息：{ex.Message}",
                                        MessageBoxIcon.Error,
                                        Global.ErrorTitle,
                                        MessageBoxDefaultButton.Button1))
                    {
                        Environment.Exit((int)ErrorCode.FileAccessError);
                    }
                }
            }
        }

        public static string ReadAllText(in string fileName, in Encoding encoding)
        {
            while (true)
            {
                try
                {
                    return File.ReadAllText(fileName, encoding);
                }
                catch (Exception ex)
                {
                    if (!CheckOperation($"{ReadFileErrorStr}{ex.Message}",
                                        MessageBoxIcon.Error,
                                        Global.ErrorTitle,
                                        MessageBoxDefaultButton.Button1))
                    {
                        Environment.Exit((int)ErrorCode.FileAccessError);
                    }
                }
            }
        }

        public static string[] ReadAllLines(in string fileName)
        {
            while (true)
            {
                try
                {
                    return File.ReadAllLines(fileName);
                }
                catch (Exception ex)
                {
                    if (!CheckOperation($"{ReadFileErrorStr}{ex.Message}",
                                        MessageBoxIcon.Error,
                                        Global.ErrorTitle,
                                        MessageBoxDefaultButton.Button1))
                    {
                        Environment.Exit((int)ErrorCode.FileAccessError);
                    }
                }
            }
        }

        public static void WriteAllText(in string fileName, in string content, in Encoding encoding)
        {
            while (true)
            {
                try
                {
                    File.WriteAllText(fileName, content, encoding);
                    return;
                }
                catch (Exception ex)
                {
                    if (!CheckOperation($"{WriteFileErrorStr}{ex.Message}",
                                        MessageBoxIcon.Error,
                                        Global.ErrorTitle,
                                        MessageBoxDefaultButton.Button1))
                    {
                        Environment.Exit((int)ErrorCode.FileAccessError);
                    }
                }
            }
        }

        public static void BringToFrontAndFocus(in Form form)
        {
            Rectangle working_area = Screen.FromControl(form).WorkingArea;
            if (form.Left   + MaxHidePart * form.Width  < working_area.Left  ||
                form.Right  - MaxHidePart * form.Width  > working_area.Right ||
                form.Top    + MaxHidePart * form.Height < working_area.Top   ||
                form.Bottom - MaxHidePart * form.Height > working_area.Bottom)
            {
                form.Location = new Point(working_area.Width - form.Width >> 1,
                                          working_area.Height - form.Height >> 1);
            }

            if (!form.Visible)
            {
                form.Show();
            }

            if (form.WindowState == FormWindowState.Minimized)
            {
                form.WindowState = FormWindowState.Normal;
            }
            form.BringToFront();
            form.Focus();
        }

        // 此方法及调用此方法的其它方法都不得被异步/并行方法调用
        public static string MD5Hash(in string fileName)
        {
            while (true)
            {
                try
                {
                    FileStream file = NewReadOnlyFileHandle(fileName);
                    byte[] hash_bytes = MD5.ComputeHash(file);
                    file.Close();
                    MD5.Initialize();

                    return ConvertBytesToMD5(hash_bytes);
                }
                catch (Exception ex)
                {
                    if (!CheckOperation($"{GetMD5ErrorStr}{ex.Message}",
                                        MessageBoxIcon.Error,
                                        Global.ErrorTitle,
                                        MessageBoxDefaultButton.Button1))
                    {
                        Environment.Exit((int)ErrorCode.HashComputeError);
                    }
                }
            }
        }

        // 此方法及调用此方法的其它方法都不得被异步/并行方法调用
        public static string MD5HashWithSalt(in string str)
        {
            while (true)
            {
                try
                {
                    byte[] hash_bytes = MD5.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(HashSalt, str)));
                    MD5.Initialize();

                    return ConvertBytesToMD5(hash_bytes);
                }
                catch (Exception ex)
                {
                    if (!CheckOperation($"{GetMD5ErrorStr}{ex.Message}",
                                        MessageBoxIcon.Error,
                                        Global.ErrorTitle,
                                        MessageBoxDefaultButton.Button1))
                    {
                        Environment.Exit((int)ErrorCode.HashComputeError);
                    }
                }
            }
        }

        // 文件哈希，以下三个方法不可同时调用
        public static string GetMD5HashFromFile(in string fileName)
        {
            FileStream file = NewReadOnlyFileHandle(fileName);
            byte[] hash_bytes = MD5ForFile.ComputeHash(file);
            file.Close();
            MD5ForFile.Initialize();
            return ConvertBytesToHash(hash_bytes);
        }

        public static string GetSHA1HashFromFile(in string fileName)
        {
            FileStream file = NewReadOnlyFileHandle(fileName);
            byte[] hash_bytes = SHA1ForFile.ComputeHash(file);
            file.Close();
            SHA1ForFile.Initialize();
            return ConvertBytesToHash(hash_bytes);
        }

        public static string GetSHA256HashFromFile(in string fileName)
        {
            FileStream file = NewReadOnlyFileHandle(fileName);
            byte[] hash_bytes = SHA256ForFile.ComputeHash(file);
            file.Close();
            SHA256ForFile.Initialize();
            return ConvertBytesToHash(hash_bytes);
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

        // 哈希资产检查
        public static void CheckIntegrality()
        {
            const long MaxFileSize =  (1024 + 512) * 1024;

            if (!Directory.Exists($".\\{Global.ResourceDirectory}"))
            {
                ShowMessageToWarn($"缺少{Global.ResourceDirectory}文件夹，请考虑重新下载应用");
                Environment.Exit((int)ErrorCode.NecessaryFileNotFound);
            }

            foreach (FileListItem file in FileList)
            {
                while (!File.Exists(file.FileName))
                {
                    if (!CheckOperation($"应用程序相对路径下必要的\n{file.FileName}\n文件缺失，请检查回收站或考虑重新下载应用。\n是否重试检查？",
                                        MessageBoxIcon.Error,
                                        Global.ErrorTitle,
                                        MessageBoxDefaultButton.Button1))
                    {
                        Environment.Exit((int)ErrorCode.NecessaryFileNotFound);
                    }
                }

                while (new FileInfo(file.FileName).Length > MaxFileSize)
                {
                    if (!CheckOperation($"应用程序相对路径下必要的\n{file.FileName}\n文件被替换\n是否重试检查？",
                                        MessageBoxIcon.Error,
                                        Global.ErrorTitle,
                                        MessageBoxDefaultButton.Button1))
                    {
                        Environment.Exit((int)ErrorCode.NecessaryFileReplaced);
                    }
                }

                while (true)
                {
                    string hash;
                    while (true)
                    {
                        try
                        {
                            hash = GetMD5HashFromFile(file.FileName);
                            break;
                        }
                        catch (Exception ex)
                        {
                            if (!CheckOperation($"{GetMD5ErrorStr}{ex.Message}",
                                                MessageBoxIcon.Error,
                                                Global.ErrorTitle,
                                                MessageBoxDefaultButton.Button1))
                            {
                                Environment.Exit((int)ErrorCode.HashComputeError);
                            }
                        }
                    }
                    if (hash == file.MD5Hash)
                    {
                        break;
                    }
                    else if (!CheckOperation($"应用程序相对路径下必要的\n{file.FileName}\n文件被替换\n是否重试检查？",
                                             MessageBoxIcon.Error,
                                             Global.ErrorTitle,
                                             MessageBoxDefaultButton.Button1))
                    {
                        Environment.Exit((int)ErrorCode.NecessaryFileReplaced);
                    }
                }
            }
        }

        // 私有静态外部方法
        [System.Runtime.InteropServices.DllImport("shell32.dll")]
        private static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation,
                                                  string lpFile, string lpParameters = "",
                                                  string lpDirectory = "", int show_style = 1);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        // 私有静态方法
        // 此方法及调用此方法的其它方法都不得被异步/并行方法调用
        private static string ConvertBytesToMD5(in byte[] bytes)
        {
            MD5StringBuilder.Length = 0;
            for (int i = 0, len = bytes.Length; i < len; ++i)
            {
                MD5StringBuilder.Append(bytes[i].ToString(HashConvertFormatStr));
            }
            return MD5StringBuilder.ToString();
        }

        private static string ConvertBytesToHash(in byte[] bytes)
        {
            HashStringBuilder.Length = 0;
            for (int i = 0, len = bytes.Length; i < len; ++i)
            {
                HashStringBuilder.Append(bytes[i].ToString(HashConvertFormatStr));
            }
            return HashStringBuilder.ToString();
        }
    }
}
