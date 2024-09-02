using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

namespace gaocheng_debug
{
    internal static class ConstValues
    {
        // 静态常量
        public const int MaxDataGroupNum = 256;
        public const int InitialDirectoriesConfigLines = 2;
        public const int PathLogLines = 8;

        public const char DataIDFlag = '[';
        public const char LineEndFlag = '\n';

        public const string ProjectNameFormatStr = "yyyy-MM-dd-HH-mm-ss";
        public const string ProjectCheckTimeFormat = "yyyy_MM_dd_HH_mm_ss_fffffff";

        public const string Warning = "警告";
        public const string DemoExePathTxtDefaultStr = "Demo Exe File Path";
        public const string YourExePathTxtDefaultStr = "Your Exe File Path";

        public const string BlankItemStr = "blank";
        public const string ProjectGeneratedFlagStr = "generated";

        public const string ReadMeHtml = "README.html";
        public const string InitialDirectoriesConfig = "initial_directories.config";
        public const string GetInputData = "get_input_data.exe";
        public const string TxtCompare = "txt_compare.exe";
        public const string PathLog = "__path.log";
        public const string TestData = "__test_data.txt";
        public const string CompareResult = "_compare_result.txt";
        public const string DemoExeResult = "_demo_exe_result.txt";
        public const string YourExeResult = "_your_exe_result.txt";
        public const string TestBat = "test.bat";
        
        public const string DefaultDirectory = @"C:\";
        public const string TestLogRelativePath = @".\test_log";

        // 动态常量
        public static readonly int ProjectNameLength = ProjectNameFormatStr.Length;

        public static readonly Encoding GB18030 = GetEncodingGB18030();

        public static readonly Random Rnd = new Random();

        public static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;

        public static readonly Comparer<string> CMP = Comparer<string>.Create((x, y) => y.CompareTo(x));

        public static readonly string ReadMeHtmlRelativePath = $".\\{ReadMeHtml}";
        public static readonly string InitialDirectoriesConfigRelativePath = $".\\rsc\\{InitialDirectoriesConfig}";
        public static readonly string PathLogFileName = $"\\{PathLog}";
        public static readonly string TestDataFileName = $"\\{TestData}";
        public static readonly string CompareResultFileName = $"\\{CompareResult}";
        public static readonly string TestBatFileName = $"\\{TestBat}";

        public static readonly string NewLine = Environment.NewLine;

        public static readonly string DataGroupTruncationWarning = $"数据组数大于{MaxDataGroupNum}，将舍弃第{MaxDataGroupNum + 1}组及之后的数据";
        public static readonly string ResultTxtNotExistExceptionStr = $"{CompareResult}文件不存在{NewLine}{NewLine}导致本异常的原因可能是：{NewLine}{CompareResult}被删除{NewLine}上次测试时遇到异常，导致{CompareResult}未能生成，但用户忽略了该情况{NewLine}{PathLog}第一个参数被非法修改为generated{NewLine}{NewLine}本异常不影响您继续使用该项目继续测试";
        public static readonly string PathLogExceptionStr = $"{PathLog}文件不存在、不合法或被篡改{NewLine}{NewLine}导致本异常的原因可能是：{NewLine}{PathLog}被删除{NewLine}您在test_log中手动创建了该文件夹{NewLine}您将1.5.0版本之前的项目放进了test_log{NewLine}{PathLog}被篡改{NewLine}{NewLine}解决方法：{NewLine}尝试在回收站中寻找本项目的{PathLog}文件并恢复{NewLine}删除本项目";
        public static readonly string TestProcessExceptionStr = $"cmd运行过程发生错误，生成{CompareResult}失败{NewLine}建议检查源程序逻辑问题 以及 每组测试数据是否合法";

        public static readonly string[] NewProjectStrSet = { "Ciallo～(∠・ω< )⌒★", "( ｀･ω･´)ゞ", "| ᐕ)⁾⁾", "٩( ╹▿╹ )۶", "ミ(ﾉ-∀-)ﾉ" };

        // 私有静态方法
        private static Encoding GetEncodingGB18030()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding("GB18030");
        }
    }
}
