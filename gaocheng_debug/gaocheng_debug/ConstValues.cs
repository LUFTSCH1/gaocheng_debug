using System;
using System.Text;

namespace gaocheng_debug
{
    internal static class ConstValues
    {
        // 静态常量
        public const int MaxDataGroupNum = 256;
        public const int InitialDirsConfigLines = 2;
        public const int PathLogLines = 5;
        public const int TestBatLines = 8;
        public const string Warning = "警告";
        public const string DefaultDirectory = @"C:\";

        // 动态常量
        public static readonly string NewLine = Environment.NewLine;
        public static readonly Encoding GB18030 = GetEncodingGB18030();
        public static readonly string DataGroupTruncationWarning = $"数据组数大于{MaxDataGroupNum}，将舍弃第{MaxDataGroupNum + 1}组及之后的数据";
        public static readonly string ResultTxtNotExistExceptionStr = $"_compare_result.txt文件不存在{NewLine}{NewLine}导致本异常的原因可能是：{NewLine}_compare_result.txt被删除{NewLine}上次测试时遇到异常，导致_compare_result.txt未能生成，但用户忽略了该情况{NewLine}__path.log第一个参数被非法修改为generated{NewLine}{NewLine}本异常不影响您继续使用该项目继续测试";
        public static readonly string PathLogExceptionStr = $"__path.log文件不存在或不合法{NewLine}{NewLine}导致本异常的原因可能是：{NewLine}__path.log被删除{NewLine}__path.log被篡改{NewLine}{NewLine}解决方法：{NewLine}尝试在回收站中寻找本项目的__path.log文件并恢复{NewLine}删除本项目";
        public static readonly string TestProcessExceptionStr = $"cmd运行过程发生错误，生成_compare_result.txt失败{NewLine}建议检查源程序逻辑问题 以及 每组测试数据是否合法";

        // 私有静态方法
        private static Encoding GetEncodingGB18030()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding("GB18030");
        }
    }
}
