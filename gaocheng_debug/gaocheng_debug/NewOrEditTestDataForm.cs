using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace gaocheng_debug
{
    public partial class NewOrEditTestDataForm : Form
    {
        // 私有成员变量
        private string projectDirPath;

        // 构造函数
        public NewOrEditTestDataForm()
        {
            InitializeComponent();
        }

        // 公有方法
        public void SetPath(in string absoluteDirPath)
        {
            projectDirPath = absoluteDirPath;
        }

        // 窗体事件处理
        private void NewOrEditTestDataFormLoad(object sender, EventArgs e)
        {
            if (File.Exists(projectDirPath + ConstValues.TestDataFileName))
            {
                txtTestData.Text = File.ReadAllText(projectDirPath + ConstValues.TestDataFileName, ConstValues.GB18030);
            }
            else
            {
                txtTestData.Text = string.Empty;
            }
        }

        // Button事件处理
        private void BtnGenerateOrModifyThenTestClick(object sender, EventArgs e)
        {
            string data_content = txtTestData.Text;
            int cnt = 0, len = data_content.Length;

            if (data_content == string.Empty)
            {
                data_content = ConstValues.DataIDFlag + ConstValues.NewLine;
                len = data_content.Length;
            }
            else
            {
                for (int i = 0; i < len; ++i)
                {
                    if (data_content[i] == ConstValues.DataIDFlag && ++cnt > ConstValues.MaxDataGroupNum)
                    {
                        MutSync.ShowMessageToWarn(ConstValues.DataGroupTruncationWarning);
                        break;
                    }
                }
                
                if (cnt < 1)
                {
                    data_content = ConstValues.DataIDFlag + ConstValues.NewLine + data_content;
                    len = data_content.Length;
                }
            }

            if (data_content[len - 1] != ConstValues.LineEndFlag)
            {
                data_content += ConstValues.NewLine;
                len = data_content.Length;
            }

            cnt = 0;
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < len && cnt < ConstValues.MaxDataGroupNum; )
            {
                while (i < len && data_content[i] == ConstValues.DataIDFlag)
                {
                    if (cnt >= ConstValues.MaxDataGroupNum)
                    {
                        break;
                    }
                    str.Append($"[{++cnt}]{ConstValues.NewLine}");
                    while (i < len && data_content[i++] != ConstValues.LineEndFlag)
                        ;
                }

                while (i < len && data_content[i] != ConstValues.DataIDFlag)
                {
                    str.Append(data_content[i++]);
                }
            }
            File.WriteAllText(projectDirPath + ConstValues.TestDataFileName, str.ToString(), ConstValues.GB18030);

            MainForm Master = Owner as MainForm;
            Master.DataGroupNum = cnt;
            DialogResult = DialogResult.OK;

            Close();
        }
    }
}
