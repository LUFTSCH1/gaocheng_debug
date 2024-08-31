using System;
using System.IO;
using System.Windows.Forms;

namespace gaocheng_debug
{
    public partial class MD5CalculatorForm : Form
    {
        // 私有常量
        private const double BinaryFileSize = 1024.0;

        // 构造函数
        public MD5CalculatorForm()
        {
            InitializeComponent();
        }

        // 私有方法
        private string GetFileSize(in string fileName)
        {
            double file_size = Convert.ToDouble(new FileInfo(fileName).Length);
            if (file_size < BinaryFileSize)
            {
                return $"{file_size:F2}B";
            }
            file_size /= BinaryFileSize;
            if (file_size < BinaryFileSize)
            {
                return $"{file_size:F2}KB";
            }
            file_size /= BinaryFileSize;
            if (file_size < BinaryFileSize)
            {
                return $"{file_size:F2}MB";
            }
            return $"{file_size / BinaryFileSize:F2}GB";
        }

        // Button事件异步处理
        private async void BtnSelectFileAndCalculateMD5ClickAsync(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                string file_size = GetFileSize(path);
                txtResultViewer.Text = $"文件：{path}{ConstValues.NewLine}大小：{file_size}{ConstValues.NewLine}计算中...";
                txtResultViewer.Text = $"目录  ：{Path.GetDirectoryName(path)}{ConstValues.NewLine}文件名：{Path.GetFileName(path)}{ConstValues.NewLine}大小  ：{file_size}{ConstValues.NewLine}MD5   ：{await MutSync.GetMD5HashFromFileAsync(path)}";
            }
        }
    }
}
