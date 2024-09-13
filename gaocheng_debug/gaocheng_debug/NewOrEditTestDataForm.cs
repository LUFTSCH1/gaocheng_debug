using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace gaocheng_debug
{
    public partial class NewOrEditTestDataForm : Form
    {
        // 私有常量
        private const int MaxDataGroupNum = 256;

        private const char DataIDFlag  = '[';
        private const char LineEndFlag = '\n';

        // 私有静态只读成员
        private static readonly string DataGroupTruncationWarning = $"数据组数大于{MaxDataGroupNum}，将舍弃第{MaxDataGroupNum + 1}组及之后的数据";

        // 私有只读成员
        private readonly MainForm Master;

        private readonly StringBuilder TestDataBuilder;

        // 构造函数
        public NewOrEditTestDataForm(in MainForm master)
        {
            Master = master ?? throw new ArgumentNullException(nameof(master));

            TestDataBuilder = new StringBuilder();

            InitializeComponent();
        }

        // 公有方法
        public void LoadTestDataContent()
        {
            if (File.Exists(Master.AbsoluteDirPath + Global.TestDataFileName))
            {
                txtTestData.Text = File.ReadAllText(Master.AbsoluteDirPath + Global.TestDataFileName, Global.GB18030);
            }
            else
            {
                txtTestData.Text = string.Empty;
            }
        }

        // 阻止释放
        private void NewOrEditTestFormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Master.RecoverResultViewerText();

            Hide();

            Master.ChangeComponentEnabled();
            Master.BtnNewProjectEnabled = true;
            Master.FocusBackToMain();
        }

        // Button事件处理
        private void BtnGenerateOrModifyThenTestClick(object sender, EventArgs e)
        {
            string data_content = txtTestData.Text;

            if (!data_content.Contains(DataIDFlag))
            {
                data_content = DataIDFlag + Global.NewLine + data_content;
            }
            int len = data_content.Length;
            if (data_content[len - 1] != LineEndFlag)
            {
                data_content += Global.NewLine;
                len = data_content.Length;
            }

            int cnt = 0;
            for (int i = 0; i < len && cnt < MaxDataGroupNum; )
            {
                while (i < len && data_content[i] == DataIDFlag)
                {
                    if (cnt >= MaxDataGroupNum)
                    {
                        MutSync.ShowMessageToWarn(DataGroupTruncationWarning);
                        break;
                    }
                    TestDataBuilder.Append($"[{++cnt}]{Global.NewLine}");
                    while (i < len && data_content[i++] != LineEndFlag)
                        ;
                }

                while (i < len && data_content[i] != DataIDFlag)
                {
                    TestDataBuilder.Append(data_content[i++]);
                }
            }

            File.WriteAllText(Master.AbsoluteDirPath + Global.TestDataFileName, TestDataBuilder.ToString(), Global.GB18030);
            TestDataBuilder.Clear();

            Hide();

            Master.DataGroupNum = cnt;
            Master.ConstructAndTest();
            Master.ChangeComponentEnabled();
            Master.BtnNewProjectEnabled = true;
        }
    }
}
