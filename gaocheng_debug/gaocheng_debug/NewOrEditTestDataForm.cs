using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace gaocheng_debug
{
    public partial class NewOrEditTestDataForm : Form
    {
        // 私有成员变量
        private string projectDirPath, demo, exe;

        // 构造函数
        public NewOrEditTestDataForm()
        {
            InitializeComponent();
        }

        // 公共方法
        public void SetPath(in string absoluteDirPath, in string demoPath, in string exePath)
        {
            projectDirPath = absoluteDirPath;
            demo = demoPath;
            exe = exePath;
        }

        // 窗体事件处理
        private void NewOrEditTestDataFormLoad(object sender, EventArgs e)
        {
            if (File.Exists(projectDirPath + @"\__test_data.txt"))
            {
                txtTestData.Text = File.ReadAllText(projectDirPath + @"\__test_data.txt", ConstValues.GB18030);
            }
            else
            {
                MutSync.ShowMessageToWarn("虽然应用依旧能运行，但不建议手动删除__test_data.txt");
                txtTestData.Text = string.Empty;
            }

            txtTestData.SelectAll();
        }

        // Button事件处理
        private void BtnGenerateOrModifyThenTestClick(object sender, EventArgs e)
        {
            string data_content = txtTestData.Text;
            int cnt = 0, len = data_content.Length;

            if (data_content == string.Empty)
            {
                data_content = $"[{ConstValues.NewLine}";
                len = data_content.Length;
            }
            else
            {
                for (int i = 0; i < len; ++i)
                {
                    if (data_content[i] == '[' && ++cnt > ConstValues.MaxDataGroupNum)
                    {
                        MutSync.ShowMessageToWarn(ConstValues.DataGroupTruncationWarning);
                        break;
                    }
                }
                
                if (cnt < 1)
                {
                    data_content = "[" + ConstValues.NewLine + data_content;
                    len = data_content.Length;
                }
            }

            if (data_content[len - 1] != '\n')
            {
                data_content += ConstValues.NewLine;
                len += ConstValues.NewLine.Length;
            }

            cnt = 0;
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < len; ++i)
            {
                if (data_content[i] == '[')
                {
                    ++cnt;
                    if (cnt > ConstValues.MaxDataGroupNum)
                    {
                        cnt = ConstValues.MaxDataGroupNum;
                        break;
                    }
                    str.Append($"[{cnt}]{ConstValues.NewLine}");
                    while (i < len && data_content[i++] != '\n')
                        ;
                }

                if (i < len)
                {
                    str.Append(data_content[i]);
                }
            }
            File.WriteAllText(projectDirPath + @"\__test_data.txt", str.ToString(), ConstValues.GB18030);

            string content = $"cd /d \"{projectDirPath}\"{ConstValues.NewLine}";
            content += $"..\\..\\rsc\\get_input_data __test_data.txt [1] | \"{demo}\" >_demo_result.txt{ConstValues.NewLine}";
            content += $"..\\..\\rsc\\get_input_data __test_data.txt [1] | \"{exe}\" >_your_exe_result.txt{ConstValues.NewLine}";
            content += $"for /l %%v in (2, 1, {cnt}) do ({ConstValues.NewLine}";
            content += $"..\\..\\rsc\\get_input_data.exe __test_data.txt [%%v] | \"{demo}\" >>_demo_result.txt{ConstValues.NewLine}";
            content += $"..\\..\\rsc\\get_input_data.exe __test_data.txt [%%v] | \"{exe}\" >>_your_exe_result.txt{ConstValues.NewLine}){ConstValues.NewLine}mode";
            File.WriteAllText(projectDirPath + @"\test.bat", content, ConstValues.GB18030);

            MainForm Master = Owner as MainForm;
            Master.DataChanged = true;

            Close();
        }
    }
}
