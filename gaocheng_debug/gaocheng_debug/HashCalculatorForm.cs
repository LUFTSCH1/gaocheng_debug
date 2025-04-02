using System;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace gaocheng_debug
{
    // 仅可同时存在一个实例
    public partial class HashCalculatorForm : Form
    {
        // 类定义

        public class HashAlgorithm
        {
            public delegate string HashAlgo(in string fileName);

            private readonly string Name;
            public readonly HashAlgo Algo;

            public HashAlgorithm(in string name, in HashAlgo algo)
            {
                Name = name;
                Algo = algo;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        // 私有常量

        private const double BinaryFileSize = 1024.0;

        // 私有只读成员

        private readonly MainForm Master;

        private readonly List<HashAlgorithm> HashAlgotithmList = new List<HashAlgorithm> {
            new HashAlgorithm("MD5   ", StaticTools.GetMD5HashFromFile),
            new HashAlgorithm("SHA1  ", StaticTools.GetSHA1HashFromFile),
            new HashAlgorithm("SHA256", StaticTools.GetSHA256HashFromFile)
        };

        // 构造函数

        public HashCalculatorForm(in MainForm master)
        {
            Master = master ?? throw new ArgumentNullException(nameof(master));

            InitializeComponent();
            cboHashAlgorithmSelector.DataSource = HashAlgotithmList;
            cboHashAlgorithmSelector.SelectedIndex = 0;
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
            StaticTools.BringToFrontAndFocus(Master);
        }

        // Button事件异步处理
        private async void BtnSelectFileAndCalculateMD5ClickAsync(object sender, EventArgs e)
        {
            if (ofdFilePathBrowser.ShowDialog() == DialogResult.OK)
            {
                btnSelectFileAndCalculateMD5.Visible = false;
                cboHashAlgorithmSelector.Enabled = false;

                string path = ofdFilePathBrowser.FileName;
                string file_size = GetFileSize(path);
                txtResultViewer.Text =   $"算法：{cboHashAlgorithmSelector.SelectedItem}{Global.NewLine}"
                                       + $"文件：{path}{Global.NewLine}"
                                       + $"大小：{file_size}{Global.NewLine}计算中...";

                Task<string> hashTask = Task.Run(() =>
                {
                    string file_info =   $"目录  ：{Path.GetDirectoryName(path)}{Global.NewLine}"
                                       + $"文件名：{Path.GetFileName(path)}{Global.NewLine}"
                                       + $"大小  ：{file_size}{Global.NewLine}";

                    DateTime start_time = DateTime.Now;
                    try
                    {
                        HashAlgorithm selectedAlgorithm = cboHashAlgorithmSelector.SelectedItem as HashAlgorithm;
                        string hash = selectedAlgorithm.Algo(path);
                        DateTime finish_time = DateTime.Now;
                        return   $"{file_info}{selectedAlgorithm}：{hash}{Global.NewLine}{Global.NewLine}"
                               + $"开始  ：{start_time.ToString(Global.OperationTimeFormatStr)}{Global.NewLine}"
                               + $"完成  ：{finish_time.ToString(Global.OperationTimeFormatStr)}{Global.NewLine}"
                               + $"用时  ：{(finish_time - start_time).TotalSeconds:F4}s";
                    }
                    catch (Exception ex)
                    {
                        DateTime break_time = DateTime.Now;
                        return   $"{file_info}开始  ：{start_time.ToString(Global.OperationTimeFormatStr)}{Global.NewLine}"
                               + $"中断  ：{break_time.ToString(Global.OperationTimeFormatStr)}{Global.NewLine}"
                               + $"历时  ：{(break_time - start_time).TotalSeconds:F4}s{Global.NewLine}"
                               + $"错误  ：{ex.Message}";
                    }
                });
                await hashTask.ContinueWith(t =>
                {
                    txtResultViewer.Text = t.Result;
                    btnSelectFileAndCalculateMD5.Visible = true;
                    cboHashAlgorithmSelector.Enabled = true;
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
    }
}
