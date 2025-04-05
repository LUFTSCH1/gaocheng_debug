using System;
using System.Text;

namespace gaocheng_debug
{
    internal enum ErrorCode
    {
        NoError,
        ApplicationAlreadyRunning,
        NecessaryFileNotFound,
        NecessaryFileReplaced,
        FileAccessError,
        HashComputeError
    }

    internal static class Global
    {
        // 静态常量

        public const int DefaultSettingsLines = 2;
        public const int ProjectGaochengLines = 7;

        public const string DefaultDirectory = @"C:\";
        
        public const string OperationTimeFormatStr = "yyyy/MM/dd HH:mm:ss.ffff";

        public const string ErrorTitle = "错误";

        // 自建文件

        public const string ReadMeHtml           = "README.html";
        public const string DefaultSettings      = "default.settings";
        public const string ProjectDirectoryLock = "protector.lock";

        // 资源和项目目录名

        public const string ResourceDirectory = "rsc";
        public const string ProjectDirectory  = "test_log";

        // 项目文件

        public const string ProjectGaocheng = "_project.gaocheng";
        public const string TestData        = "_test_data.txt";
        public const string CompareResult   = "compare_result.txt";
        public const string DemoExeResult   = "demo_exe_result.txt";
        public const string YourExeResult   = "your_exe_result.txt";

        // 渣哥的小工具

        public const string GetInputData = "get_input_data.exe";
        public const string TxtCompare   = "txt_compare.exe";

        // 我的小工具

        public const string OutputGroup = "output_group.exe";

        // 动态常量

        public static readonly string NewLine = Environment.NewLine;

        public static readonly string ProjectDirectoryRelativePath = $".\\{ProjectDirectory}";

        public static readonly string ReadMeHtmlRelativePath           = $".\\{ReadMeHtml}";
        public static readonly string DefaultSettingsRelativePath      = $".\\{ResourceDirectory}\\{DefaultSettings}";
        public static readonly string ProjectDirectoryLockRelativePath = $".\\{ProjectDirectory}\\{ProjectDirectoryLock}";

        public static readonly Encoding GB18030 = GetEncodingGB18030();

        // 私有静态方法-初始化GB18030编码
        private static Encoding GetEncodingGB18030()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding("GB18030");
        }
    }
}
