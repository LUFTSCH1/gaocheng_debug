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

        private const char DataIDFlag   = '[';
        private const char LF           = '\n';

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
            Text = $"创建/修改测试数据 <{Master.ProjectDirName}>";
            if (File.Exists(Master.AbsoluteTestDataPath))
            {
                rtxTestDataEditor.Text = MutSync.ReadAllText(Master.AbsoluteTestDataPath, Global.GB18030);
            }
            else
            {
                rtxTestDataEditor.Text = string.Empty;
            }
        }

        // 阻止释放
        private void NewOrEditTestFormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
            Master.DoWhileEditCanceled();
        }

        // rtxTestDataEditor事件处理

        private void RtxTestDataEditorKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                e.SuppressKeyPress = true;
                WriteTestDataWithTip();
            }
        }

        // Button事件处理
        private void BtnGenerateOrModifyThenTestClick(object sender, EventArgs e)
        {
            string data_content = rtxTestDataEditor.Text;

            if (!data_content.Contains(DataIDFlag))
            {
                data_content = $"{DataIDFlag}{LF}{data_content}";
            }
            int len = data_content.Length;
            if (data_content[len - 1] != LF)
            {
                data_content += LF;
                len = data_content.Length;
            }

            int cnt = 0;
            bool reach_limit = false;
            for (int i = 0; i < len; ++i)
            {
                while (i < len && data_content[i] != LF)
                {
                    while (i < len && data_content[i] == ' ')
                    {
                        ++i;
                    }
                    if (i >= len)
                    {
                        break;
                    }
                    if (data_content[i] == DataIDFlag)
                    {
                        if (cnt >= MaxDataGroupNum)
                        {
                            MutSync.ShowMessageToWarn(DataGroupTruncationWarning);
                            reach_limit = true;
                            break;
                        }
                        TestDataBuilder.Append($"[{++cnt}]");
                        while (i < len && data_content[i] != LF)
                        {
                            ++i;
                        }
                    }
                    else
                    {
                        while (i < len && data_content[i] != LF)
                        {
                            TestDataBuilder.Append(data_content[i++]);
                        }
                    }
                }
                TestDataBuilder.Append(Global.NewLine);
                if (reach_limit)
                {
                    break;
                }
            }

            WriteTestData(TestDataBuilder.ToString());
            TestDataBuilder.Clear();

            Hide();
            Master.DoWhileEdited(cnt);
        }

        private void BtnSaveClick(object sender, EventArgs e) =>
            WriteTestDataWithTip();

        private void TmrSaveTipControllerTick(object sender, EventArgs e)
        {
            lblSaveTip.Visible = false;
            tmrSaveTipController.Enabled = false;
        }

        // 私有工具函数
        private void WriteTestData(in string content) =>
            MutSync.WriteAllText(Master.AbsoluteTestDataPath, content, Global.GB18030);

        private void WriteTestDataWithTip()
        {
            WriteTestData(rtxTestDataEditor.Text.Replace("\n", Global.NewLine));
            lblSaveTip.Visible = true;
            tmrSaveTipController.Enabled = true;
        }

        //private int GetHighLightLength(in int pos)
        //{
        //    int len = 1;
        //    while (pos + len < rtxTestDataEditor.Text.Length &&
        //           rtxTestDataEditor.Text[pos + len] != CR   &&
        //           rtxTestDataEditor.Text[pos + len] != LF)
        //    {
        //        ++len;
        //    }

        //    return len;
        //}

        //private void HighLightDataId(in int startIndex, in int length)
        //{
        //    rtxTestDataEditor.SuspendLayout();

        //    rtxTestDataEditor.SelectAll();
        //    rtxTestDataEditor.SelectionColor = rtxTestDataEditor.ForeColor;

        //    for (int i = startIndex, len; i < rtxTestDataEditor.Text.Length && i < length; ++i)
        //    {
        //        if (rtxTestDataEditor.Text[i] == DataIDFlag)
        //        {
        //            len = GetHighLightLength(i);
        //            rtxTestDataEditor.Select(i, len);
        //            rtxTestDataEditor.SelectionColor = FlagColor;
        //            i += len - 1;
        //        }
        //    }
        //    rtxTestDataEditor.DeselectAll();

        //    rtxTestDataEditor.ResumeLayout();
        //}
    }
}
