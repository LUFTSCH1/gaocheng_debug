using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;

namespace gaocheng_debug
{
    internal static class Program
    {
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
                    Environment.Exit(1);
                }
            }

            {
                string path = ".\\";
                if (!File.Exists(path + "gaocheng_debug.exe.config"))
                {
                    MutSync.ShowMessageToWarn("应用配置文件\ngaocheng_debug.exe.config\n缺失，请考虑重新下载应用");
                    Environment.Exit(1);
                }

                string[] NECESSARY_FILES = { "System.Runtime.CompilerServices.Unsafe.dll", "System.Memory.dll", "System.Buffers.dll", "System.Text.Encoding.CodePages.dll", "System.Numerics.Vectors.dll" };
                for (int i = 0; i < NECESSARY_FILES.Length; ++i)
                {
                    if(!File.Exists(path + NECESSARY_FILES[i]))
                    {
                        MutSync.ShowMessageToWarn($"必要动态链接库\n{NECESSARY_FILES[i]}\n缺失，请考虑重新下载应用");
                        Environment.Exit(1);
                    }
                }

                path += "rsc";
                if (!Directory.Exists(path))
                {
                    MutSync.ShowMessageToWarn("缺少rsc文件夹，请考虑重新下载应用");
                    Environment.Exit(1);
                }

                path += "\\";
                NECESSARY_FILES = new string[]{ "get_input_data.exe", "initial_dirs.config", "msvcp140d.dll", "txt_compare.exe", "ucrtbased.dll", "vcruntime140d.dll" };
                for (int i = 0; i < NECESSARY_FILES.Length; ++i)
                {
                    if(!File.Exists(path + NECESSARY_FILES[i]))
                    {
                        MutSync.ShowMessageToWarn($"rsc文件夹中必要的\n{NECESSARY_FILES[i]}\n文件缺失，请考虑重新下载应用");
                        Environment.Exit(1);
                    }
                }
            }

            if (Environment.OSVersion.Version.Major >= 6)
            {
                const int DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = -4;
                SetProcessDpiAwarenessContext(DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
