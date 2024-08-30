using System;
using System.IO;
using System.Windows.Forms;

namespace gaocheng_debug
{
    public partial class MD5CalculatorForm : Form
    {
        // 构造函数
        public MD5CalculatorForm()
        {
            InitializeComponent();
        }

        // Button事件异步处理
        private async void BtnSelectFileAndCalculateMD5ClickAsync(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                txtResultViewer.Text = $"文件：{path}{ConstValues.NewLine}计算中...";
                txtResultViewer.Text = $"目录  ：{Path.GetDirectoryName(path)}{ConstValues.NewLine}文件名：{Path.GetFileName(path)}{ConstValues.NewLine}MD5   ：{await MutSync.GetMD5HashFromFileAsync(path)}";
            }
        }
    }
}
