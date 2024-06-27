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
        private Encoding GB18030;

        public Form3(in string absolute_dir_path, in string demo_path, in string exe_path)
        {
            project_dir_path = absolute_dir_path;
            demo = demo_path;
            exe = exe_path;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            GB18030 = Encoding.GetEncoding("GB18030");
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            BackColor = Color.FromArgb(234, 234, 239);
            button1.BackColor = Color.FromArgb(85, 187, 138);

            StreamReader sr = new StreamReader(project_dir_path + @"\__test_data.txt", GB18030);
            textBox1.Text = sr.ReadToEnd();
            sr.Close();

            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string data_content = textBox1.Text, newLine = Environment.NewLine;
            int cnt = 0, len = data_content.Length;
            if (data_content == "" || data_content[len - 1] != '\n')
            {
                data_content += newLine;
                len += Environment.NewLine.Length;
            }

            for (int i = 0; i < len; ++i)
            {
                if (data_content[i] == '[')
                {
                    ++cnt;
                }
            }

            if (cnt > 99)
            {
                MessageBox.Show("数据组数大于99，将舍弃第100组及之后的数据", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            cnt = 0;
            StreamWriter dataSW = new StreamWriter(project_dir_path + @"\__test_data.txt", false, GB18030);
            for (int i = 0; i < len; ++i)
            {
                if (data_content[i] == '[')
                {
                    ++cnt;
                    if (cnt > 99)
                    {
                        cnt = 99;
                        break;
                    }
                    dataSW.Write(string.Format("[{0}]{1}", cnt, newLine));
                    while (i < len && data_content[i++] != '\n')
                        ;
                }

                if (i < len)
                {
                    dataSW.Write(data_content[i]);
                }
            }
            dataSW.Close();

            StreamWriter test_batSW = new StreamWriter(project_dir_path + @"\test.bat", false, GB18030);
            test_batSW.Write(string.Format("cd /d \"{0}\"\n", project_dir_path));
            test_batSW.Write(string.Format("..\\..\\rsc\\get_input_data __test_data.txt [1] | \"{0}\" >_demo_result.txt\n", demo));
            test_batSW.Write(string.Format("..\\..\\rsc\\get_input_data __test_data.txt [1] | \"{0}\" >_your_exe_result.txt\n", exe));
            test_batSW.Write(string.Format("for /l %%v in (2, 1, {0}) do (\n", cnt));
            test_batSW.Write(string.Format("..\\..\\rsc\\get_input_data.exe __test_data.txt [%%v] | \"{0}\" >>_demo_result.txt\n", demo));
            test_batSW.Write(string.Format("..\\..\\rsc\\get_input_data.exe __test_data.txt [%%v] | \"{0}\" >>_your_exe_result.txt\n)\nmode", exe));
            test_batSW.Close();

            Form1 fm1 = Owner as Form1;
            fm1.is_dc = true;

            Close();
        }
    }
}
