using System;
using System.Text;

namespace gaocheng_debug
{
    internal static class ConstValues
    {
        // 静态常量
        public const int MaxDataGroupNum = 256;
        public const string Warning = "警告";
        public const string DefaultDirectory = @"C:\";

        // 动态常量
        public static readonly string NewLine = Environment.NewLine;
        public static readonly Encoding GB18030 = GetEncodingGB18030();
        public static readonly string DataGroupTruncationWarning = $"数据组数大于{MaxDataGroupNum}，将舍弃第{MaxDataGroupNum + 1}组及之后的数据";

        // 私有静态方法
        private static Encoding GetEncodingGB18030()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding("GB18030");
        }
    }
}
