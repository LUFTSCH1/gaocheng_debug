using System;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace gaocheng_debug
{
    public partial class Form3 : Form
    {
        private string project_dir_path, demo, exe;

        public Form3()
        {
            InitializeComponent();

            BackColor = Color.FromArgb(234, 234, 239);
            button1.BackColor = Color.FromArgb(85, 187, 138);
        }

        public void SetPath(in string absolute_dir_path, in string demo_path, in string exe_path)
        {
            project_dir_path = absolute_dir_path;
            demo = demo_path;
            exe = exe_path;

            return;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            textBox1.Text = File.ReadAllText(project_dir_path + @"\__test_data.txt", ConstValues.GB18030);

            textBox1.SelectAll();

            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string data_content = textBox1.Text;
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
                    if (data_content[i] == '[' && ++cnt > ConstValues.MAX_DATA_GROUP_NUM)
                    {
                        MessageBox.Show(ConstValues.DataGroupTruncationWarning, ConstValues.WARNING, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    if (cnt > ConstValues.MAX_DATA_GROUP_NUM)
                    {
                        cnt = ConstValues.MAX_DATA_GROUP_NUM;
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
            File.WriteAllText(project_dir_path + @"\__test_data.txt", str.ToString(), ConstValues.GB18030);

            string content = $"cd /d \"{project_dir_path}\"{ConstValues.NewLine}";
            content += $"..\\..\\rsc\\get_input_data __test_data.txt [1] | \"{demo}\" >_demo_result.txt{ConstValues.NewLine}";
            content += $"..\\..\\rsc\\get_input_data __test_data.txt [1] | \"{exe}\" >_your_exe_result.txt{ConstValues.NewLine}";
            content += $"for /l %%v in (2, 1, {cnt}) do ({ConstValues.NewLine}";
            content += $"..\\..\\rsc\\get_input_data.exe __test_data.txt [%%v] | \"{demo}\" >>_demo_result.txt{ConstValues.NewLine}";
            content += $"..\\..\\rsc\\get_input_data.exe __test_data.txt [%%v] | \"{exe}\" >>_your_exe_result.txt{ConstValues.NewLine}){ConstValues.NewLine}mode";
            File.WriteAllText(project_dir_path + @"\test.bat", content, ConstValues.GB18030);

            Form1 Master = Owner as Form1;
            Master.IsDataChanged = true;

            Close();
        }
    }
}
