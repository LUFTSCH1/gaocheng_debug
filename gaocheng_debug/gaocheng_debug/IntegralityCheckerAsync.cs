using System;
using System.IO;
using System.Threading.Tasks;

namespace gaocheng_debug
{
    internal static class IntegralityCheckerAsync
    {
        // 公有静态异步方法
        public static async Task CheckIntegralityAsync()
        {
            const long MaxFileSize = 2 * 1024 * 1024;

            string path = ".\\";
            if (!File.Exists(path + "gaocheng_debug.exe.config"))
            {
                ShowWarningThenExit("应用配置文件\ngaocheng_debug.exe.config\n缺失，请考虑重新下载应用");
            }

            if (new FileInfo(path + "gaocheng_debug.exe.config").Length > MaxFileSize || "98dbb4a9bc384dca6b79a47886c42891" != await MutSync.GetMD5HashFromFileAsync(path + "gaocheng_debug.exe.config"))
            {
                ShowWarningThenExit($"文件\ngaocheng_debug.exe.config\n被替换");
            }

            string[] necessary_files = { "System.Runtime.CompilerServices.Unsafe.dll", "System.Memory.dll", "System.Buffers.dll", "System.Text.Encoding.CodePages.dll", "System.Numerics.Vectors.dll" };
            string[] file_hash = { "c610e828b54001574d86dd2ed730e392", "f09441a1ee47fb3e6571a3a448e05baf", "ecdfe8ede869d2ccc6bf99981ea96400", "2c9e9cd5c6f31ebfdc8155efdc20f4f7", "aaa2cbf14e06e9d3586d8a4ed455db33" };
            for (int i = 0; i < necessary_files.Length; ++i)
            {
                if (!File.Exists(path + necessary_files[i]))
                {
                    ShowWarningThenExit($"必要动态链接库\n{necessary_files[i]}\n缺失，请考虑重新下载应用");
                }
                else if (new FileInfo(path + necessary_files[i]).Length > MaxFileSize || file_hash[i] != await MutSync.GetMD5HashFromFileAsync(path + necessary_files[i]))
                {
                    ShowWarningThenExit($"文件\n{necessary_files[i]}\n被替换");
                }
            }

            path += "rsc";
            if (!Directory.Exists(path))
            {
                ShowWarningThenExit("缺少rsc文件夹，请考虑重新下载应用");
            }

            path += "\\";
            necessary_files = new string[] { ConstValues.GetInputData, "msvcp140d.dll", ConstValues.TxtCompare, "ucrtbased.dll", "vcruntime140d.dll" };
            file_hash = new string[] { "0575ba8c3fcd1bd3fe7f2409325a3f98", "a66eaf437d3d8c53a127f77b2a896f0d", "665beeefe858a15c1f3d531baa64ee0d", "4d98940874d14692b02ece8f5b591362", "b907335a3619259f8aaf22c445de15ce" };
            for (int i = 0; i < necessary_files.Length; ++i)
            {
                if (!File.Exists(path + necessary_files[i]))
                {
                    ShowWarningThenExit($"rsc文件夹中必要的\n{necessary_files[i]}\n文件缺失，请考虑重新下载应用");
                }
                else if (new FileInfo(path + necessary_files[i]).Length > MaxFileSize || file_hash[i] != await MutSync.GetMD5HashFromFileAsync(path + necessary_files[i]))
                {
                    ShowWarningThenExit($"文件\n{necessary_files[i]}\n被替换");
                }
            }
        }

        // 私有静态方法
        private static void ShowWarningThenExit(in string msg)
        {
            MutSync.ShowMessageToWarn(msg);
            Environment.Exit(1);
        }
    }
}
