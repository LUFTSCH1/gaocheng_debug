using System;
using System.Text;

namespace gaocheng_debug
{
    internal static class ConstValues
    {
        private static Encoding GetEncodingGB18030()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding("GB18030");
        }

        public const int MAX_DATA_GROUP_NUM = 256;
        public const string WARNING = "警告";
        public const string DEFAULT_DIRECTORY = @"C:\";

        public static readonly string NewLine = Environment.NewLine;
        public static readonly Encoding GB18030 = GetEncodingGB18030();
        public static readonly string DataGroupTruncationWarning = $"数据组数大于{MAX_DATA_GROUP_NUM}，将舍弃第{MAX_DATA_GROUP_NUM + 1}组及之后的数据";
    }
}
