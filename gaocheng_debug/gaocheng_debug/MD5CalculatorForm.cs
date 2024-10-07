using System;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace gaocheng_debug
{
    public partial class MD5CalculatorForm : Form
    {
        // 私有常量
        private const double BinaryFileSize = 1024.0;

        // 私有只读成员
        private readonly MainForm Master;

        // 构造函数
        public MD5CalculatorForm(in MainForm master)
        {
            Master = master ?? throw new ArgumentNullException(nameof(master));

            InitializeComponent();
        }

        // 私有静态方法
        private static string GetFileSize(in string fileName)
        {
            double file_size = Convert.ToDouble(new FileInfo(fileName).Length);
            if (file_size < BinaryFileSize)
            {
                return $"{file_size:F2} B";
            }
            file_size /= BinaryFileSize;
            if (file_size < BinaryFileSize)
            {
                return $"{file_size:F2} KiB";
            }
            file_size /= BinaryFileSize;
            if (file_size < BinaryFileSize)
            {
                return $"{file_size:F2} MiB";
            }
            return $"{file_size / BinaryFileSize:F2} GiB";
        }

        // 阻止释放
        private void MD5CalculatorFormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
            MutSync.BringToFrontAndFocus(Master);
        }

        // Button事件异步处理
        private async void BtnSelectFileAndCalculateMD5ClickAsync(object sender, EventArgs e)
        {
            if (ofdFilePathBrowser.ShowDialog() == DialogResult.OK)
            {
                string path = ofdFilePathBrowser.FileName;
                string file_size = GetFileSize(path);
                txtResultViewer.Text = $"文件：{path}{Global.NewLine}大小：{file_size}{Global.NewLine}计算中...";

                txtResultViewer.Text = await Task.Run(() =>
                {
                    DateTime start_time = DateTime.Now;
                    string hash = MutSync.GetMD5HashFromFile(path);
                    DateTime finish_time = DateTime.Now;
                    
                    string result = $"目录  ：{Path.GetDirectoryName(path)}{Global.NewLine}文件名：{Path.GetFileName(path)}{Global.NewLine}大小  ：{file_size}{Global.NewLine}";
                    if (hash.Length == 32)
                    {
                        result += $"MD5   ：{hash}{Global.NewLine}{Global.NewLine}开始  ：{start_time.ToString(Global.OperationTimeFormatStr)}{Global.NewLine}完成  ：{finish_time.ToString(Global.OperationTimeFormatStr)}{Global.NewLine}用时  ：{(finish_time - start_time).TotalSeconds:F4}s";
                    }
                    else
                    {
                        result += $"开始  ：{start_time.ToString(Global.OperationTimeFormatStr)}{Global.NewLine}中断  ：{finish_time.ToString(Global.OperationTimeFormatStr)}{Global.NewLine}历时  ：{(finish_time - start_time).TotalSeconds:F4}s{Global.NewLine}错误  ：{hash}";
                    }
                    return result;
                }).ConfigureAwait(true);
            }
        }

        private void TxtResultViewerTextChanged(object sender, EventArgs e)
        {
            btnSelectFileAndCalculateMD5.Visible = !btnSelectFileAndCalculateMD5.Visible;
        }
    }
}
