using System;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace gaocheng_debug
{
    public partial class Form3 : Form
    {
        private const int MAX_DATA_GROUP_NUM = 256;

        private readonly string NewLine;
        private readonly string DataGroupTruncationWarning;
        private readonly Encoding GB18030;
        
        private string project_dir_path, demo, exe;

        public Form3(in Encoding encoding)
        {
            NewLine = Environment.NewLine;
            DataGroupTruncationWarning = $"数据组数大于{MAX_DATA_GROUP_NUM}，将舍弃第{MAX_DATA_GROUP_NUM + 1}组及之后的数据";
            GB18030 = encoding;

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
            StreamReader sr = new StreamReader(project_dir_path + @"\__test_data.txt", GB18030);
            textBox1.Text = sr.ReadToEnd();
            sr.Close();

            textBox1.SelectAll();

            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string data_content = textBox1.Text;
            int cnt = 0, len = data_content.Length;

            if (data_content == string.Empty)
            {
                data_content = $"[{NewLine}";
                len = data_content.Length;
            }
            else
            {
                for (int i = 0; i < len; ++i)
                {
                    if (data_content[i] == '[' && ++cnt > MAX_DATA_GROUP_NUM)
                    {
                        MessageBox.Show(DataGroupTruncationWarning, ConstStrings.WARNING, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                }
                
                if (cnt < 1)
                {
                    data_content = "[" + NewLine + data_content;
                    len = data_content.Length;
                    cnt = 1;
                }
            }

            if (data_content[len - 1] != '\n')
            {
                data_content += NewLine;
                len += NewLine.Length;
            }

            cnt = 0;
            StreamWriter data_and_test_bat_SW = new StreamWriter(project_dir_path + @"\__test_data.txt", false, GB18030);
            for (int i = 0; i < len; ++i)
            {
                if (data_content[i] == '[')
                {
                    ++cnt;
                    if (cnt > MAX_DATA_GROUP_NUM)
                    {
                        cnt = MAX_DATA_GROUP_NUM;
                        break;
                    }
                    data_and_test_bat_SW.Write($"[{cnt}]{NewLine}");
                    while (i < len && data_content[i++] != '\n')
                        ;
                }

                if (i < len)
                {
                    data_and_test_bat_SW.Write(data_content[i]);
                }
            }
            data_and_test_bat_SW.Close();

            data_and_test_bat_SW = new StreamWriter(project_dir_path + @"\test.bat", false, GB18030);
            data_and_test_bat_SW.Write($"cd /d \"{project_dir_path}\"\n");
            data_and_test_bat_SW.Write($"..\\..\\rsc\\get_input_data __test_data.txt [1] | \"{demo}\" >_demo_result.txt\n");
            data_and_test_bat_SW.Write($"..\\..\\rsc\\get_input_data __test_data.txt [1] | \"{exe}\" >_your_exe_result.txt\n");
            data_and_test_bat_SW.Write($"for /l %%v in (2, 1, {cnt}) do (\n");
            data_and_test_bat_SW.Write($"..\\..\\rsc\\get_input_data.exe __test_data.txt [%%v] | \"{demo}\" >>_demo_result.txt\n");
            data_and_test_bat_SW.Write($"..\\..\\rsc\\get_input_data.exe __test_data.txt [%%v] | \"{exe}\" >>_your_exe_result.txt\n)\nmode");
            data_and_test_bat_SW.Close();

            Form1 fm1 = Owner as Form1;
            fm1.IsDataChanged = true;

            Close();
        }
    }
}
