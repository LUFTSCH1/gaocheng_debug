using System;
using System.Text;

namespace gaocheng_debug
{
    internal enum ErrorCode
    {
        ApplicationAlreadyRunning = 1,
        NecessaryFileNotFound     = 2,
        NecessaryFileReplaced     = 3
    }

    internal static class Global
    {
        // 静态常量
        public const string DefaultDirectory = @"C:\";
        
        public const string OperationTimeFormatStr = "yyyy/MM/dd HH:mm:ss.ffff";

        // 自建文件
        public const string ReadMeHtml               = "README.html";
        public const string InitialDirectoriesConfig = "initial_directories.config";
        public const string ProjectDirectoryLock     = "protector.lock";

        // 资源和项目目录名
        public const string ResourceDirectory = "rsc";
        public const string ProjectDirectory  = "test_log";

        // 项目文件
        public const string ProjectGaocheng = "__project.gaocheng";
        public const string TestData        = "__test_data.txt";
        public const string CompareResult   = "_compare_result.txt";
        public const string DemoExeResult   = "_demo_exe_result.txt";
        public const string YourExeResult   = "_your_exe_result.txt";
        public const string TestBat         = "test.bat";

        // 渣哥的小工具
        public const string GetInputData = "get_input_data.exe";
        public const string TxtCompare   = "txt_compare.exe";

        // 动态常量
        public static readonly string NewLine = Environment.NewLine;

        public static readonly string ProjectDirectoryRelativePath = $".\\{ProjectDirectory}";

        public static readonly string ReadMeHtmlRelativePath               = $".\\{ReadMeHtml}";
        public static readonly string InitialDirectoriesConfigRelativePath = $".\\{ResourceDirectory}\\{InitialDirectoriesConfig}";
        public static readonly string ProjectDirectoryLockRelativePath     = $".\\{ProjectDirectory}\\{ProjectDirectoryLock}";

        public static readonly string ProjectGaochengFileName = $"\\{ProjectGaocheng}";
        public static readonly string TestDataFileName        = $"\\{TestData}";
        public static readonly string CompareResultFileName   = $"\\{CompareResult}";
        public static readonly string TestBatFileName         = $"\\{TestBat}";

        public static readonly Encoding GB18030 = GetEncodingGB18030();

        // 私有静态方法-初始化GB18030编码
        private static Encoding GetEncodingGB18030()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding("GB18030");
        }
    }
}
