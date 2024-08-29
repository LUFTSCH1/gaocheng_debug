using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace gaocheng_debug
{
    public partial class Form1 : Form
    {
        private readonly string AbsoluteTestLogPath;

        private readonly Form2 FM2;
        private readonly Form3 FM3;
        private readonly Form4 FM4;

        private readonly Process CMD;

        private string default_demo_path, default_exe_path;

        private bool is_data_changed, is_mode_changed, is_path_changed;
        private string absolute_dir_path, project_dir_name;
        private string project_demo_path, project_exe_path;
        private string recorded_app_path;

        public string DefaultDemoPath
        {
            set { default_demo_path = value; }
        }

        public string DefaultExePath
        {
            set { default_exe_path = value; }
        }

        public bool IsDataChanged
        {
            set { is_data_changed = value; }
        }

        public Form1()
        { 
            AbsoluteTestLogPath = Directory.GetCurrentDirectory() + @"\test_log\";

            FM2 = new Form2();
            FM3 = new Form3();
            FM4 = new Form4();

            CMD = new Process();
            CMD.StartInfo.FileName = "cmd.exe";
            CMD.StartInfo.UseShellExecute = false;
            CMD.StartInfo.RedirectStandardInput = true;

            if (!Directory.Exists(AbsoluteTestLogPath))
            {
                Directory.CreateDirectory(AbsoluteTestLogPath);
            }

            InitializeComponent();

            {
                string[] form1_names = { "校对工具", "oop，启动！", "高程，启动！", "QAQ", "Ciallo～(∠・ω< )⌒★", "兄弟，写多久了？", "是兄弟，就来田野打架1捞我", "让我康康你的小红车" , "(✿╹◡╹)", "٩( ╹▿╹ )۶" };
                Random rnd = new Random();
                Text = form1_names[rnd.Next(0, form1_names.Length)];
            }

            {
                string[] paths = File.ReadAllLines(@".\rsc\initial_dirs.config");
                default_demo_path = paths[0];
                default_exe_path = paths[1];
            }

            RefreshFileList();
            comboBox1.SelectedIndex = 0;
        }

        private bool ExeErrorFilter()
        {
            bool isDemoExist = File.Exists(textBox1.Text);
            bool isYourExeExist = File.Exists(textBox2.Text);
            
            if (isDemoExist && isYourExeExist) 
            {
                return true;
            }
            else
            {
                if (!isDemoExist && !isYourExeExist)
                {
                    MutSync.ShowMessageToWarn("demo和作业exe文件均不存在");
                }
                else if (!isDemoExist)
                {
                    MutSync.ShowMessageToWarn("demo文件不存在");
                }
                else
                {
                    MutSync.ShowMessageToWarn("作业exe文件不存在");
                }

                return false;
            }
        }

        private void DisableComponent()
        {
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            richTextBox1.Text = string.Empty;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;

            return;
        }

        private void EnableComponent()
        {
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;

            return;
        }

        private void RefreshFileList()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("blank");

            string[] directories = Directory.GetDirectories(@".\test_log");
            int len = directories.Length;
            for (int i = 0; i < len; ++i)
            {
                directories[i] = Path.GetFileName(directories[i]);
            }
            Array.Sort(directories, delegate(string x, string y) { return y.CompareTo(x); });
            for (int i = 0; i < len; ++i)
            {
                comboBox1.Items.Add(directories[i]);
            }

            return;
        }

        private void TryToGetRecordedAppPath()
        {
            if (File.Exists(absolute_dir_path + @"\test.bat"))
            {
                string temp = File.ReadAllText(absolute_dir_path + @"\test.bat", ConstValues.GB18030);
                recorded_app_path = string.Empty;
                int i = 0, len = temp.Length;
                while (i < len && temp[i] != '\"')
                {
                    ++i;
                }
                for (++i; i < len && temp[i] != '\"'; ++i)
                {
                    recorded_app_path += temp[i];
                }
            }
            else
            {
                MutSync.ShowMessageToWarn("test.bat异常，您可能删除了该文件");
            }

            return;
        }

        private void TryToGetCompareResult()
        {
            if (File.Exists(absolute_dir_path + @"\_compare_result.txt"))
            {
                richTextBox1.Text = File.ReadAllText(absolute_dir_path + @"\_compare_result.txt", ConstValues.GB18030);
            }
            else
            {
                MutSync.ShowMessageToWarn($"项目\n{project_dir_name}\n生成的\n_compare_result.txt\n文件不存在");
                richTextBox1.Text = $"项目 {project_dir_name} 生成的_compare_result.txt文件不存在{ConstValues.NewLine}{ConstValues.NewLine}导致本异常的原因可能是：{ConstValues.NewLine}_compare_result.txt被删除{ConstValues.NewLine}__path.log第一个参数被非法修改为generated";
            }

            return;
        }

        private void TryToGetPathLogInfo()
        {
            if (File.Exists(absolute_dir_path + @"\__path.log"))
            {
                string[] pathInfo = File.ReadAllLines(absolute_dir_path + @"\__path.log");

                comboBox2.SelectedIndex = Convert.ToInt32(pathInfo[3]);
                comboBox3.SelectedIndex = Convert.ToInt32(pathInfo[4]);

                if (pathInfo[0] != "generated")
                {
                    textBox1.Text = string.Empty;
                    textBox2.Text = string.Empty;
                    richTextBox1.Text = string.Empty;
                }
                else
                {
                    textBox1.Text = project_demo_path = pathInfo[1];
                    textBox2.Text = project_exe_path = pathInfo[2];

                    TryToGetRecordedAppPath();

                    TryToGetCompareResult();
                }

                if (!button6.Enabled)
                {
                    EnableComponent();
                }
            }
            else
            {
                DisableComponent();
                MutSync.ShowMessageToWarn($"项目\n{project_dir_name}\n生成的\n__path.log\n文件不存在");
                richTextBox1.Text = $"项目 {project_dir_name} 生成的__path.log文件不存在{ConstValues.NewLine}{ConstValues.NewLine}导致本异常的原因可能是：{ConstValues.NewLine}__path.log被删除{ConstValues.NewLine}{ConstValues.NewLine}解决方法：{ConstValues.NewLine}尝试在回收站中寻找本项目的__path.log文件并恢复{ConstValues.NewLine}删除本项目";
                button7.Enabled = true;
            }

            return;
        }

        private string CompareCommandConstruct()
        {
            string compare = @"..\..\rsc\txt_compare --file1 _demo_result.txt --file2 _your_exe_result.txt --trim ";
            if (comboBox2.SelectedIndex == 0)
            {
                compare += "none ";
            }
            else if (comboBox2.SelectedIndex == 1)
            {
                compare += "right ";
            }
            else
            {
                compare += "left ";
            }
            compare += "--display ";
            if (comboBox3.SelectedIndex == 0)
            {
                compare += "normal ";
            }
            else
            {
                compare += "detailed ";
            }
            compare += ">_compare_result.txt";

            return compare;
        }

        private void GenerateAndCompare()
        {
            if (!ExeErrorFilter())
            {
                return;
            }

            if (is_mode_changed || is_path_changed)
            {
                if (is_path_changed)
                {
                    project_demo_path = textBox1.Text;
                    project_exe_path = textBox2.Text;
                    is_path_changed = false;
                }
                File.WriteAllText(absolute_dir_path + @"\__path.log", $"generated\n{project_demo_path}\n{project_exe_path}\n{comboBox2.SelectedIndex}\n{comboBox3.SelectedIndex}");
            }

            if (is_mode_changed || is_data_changed)
            {
                is_mode_changed = false;
                string[] bat_content = File.ReadAllLines(absolute_dir_path + @"\test.bat", ConstValues.GB18030);
                bat_content[bat_content.Length - 1] = CompareCommandConstruct();
                File.WriteAllLines(absolute_dir_path + @"\test.bat", bat_content, ConstValues.GB18030);
            }

            CMD.Start();
            CMD.StandardInput.WriteLine("\"" + absolute_dir_path + "\\test.bat\"");
            CMD.StandardInput.WriteLine("cls&exit");
            CMD.WaitForExit();
            CMD.Close();

            if (File.Exists(absolute_dir_path + @"\_compare_result.txt"))
            {
                richTextBox1.Text = File.ReadAllText(absolute_dir_path + @"\_compare_result.txt", ConstValues.GB18030);
            }
            else
            {
                richTextBox1.Text = $"cmd运行过程发生错误，生成失败{ConstValues.NewLine}建议检查源文件逻辑问题 以及 每组测试数据是否合法";
                MutSync.ShowMessageToWarn("cmd运行过程发生错误，生成失败");
            }
            
            return;
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FM2.SetPath(default_demo_path, default_exe_path);
            FM2.ShowDialog(this);

            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string NewPath = AbsoluteTestLogPath + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            Directory.CreateDirectory(NewPath);

            StreamWriter sw = new StreamWriter(NewPath + @"\__path.log");
            sw.Write("new\nno_data\nno_data\n0\n0");
            sw.Close();

            sw = new StreamWriter(NewPath + @"\__test_data.txt", false, ConstValues.GB18030);
            sw.Close();

            sw = new StreamWriter(NewPath + @"\_compare_result.txt", false, ConstValues.GB18030);
            sw.Close();

            sw = new StreamWriter(NewPath + @"\_demo_result.txt", false, ConstValues.GB18030);
            sw.Close();

            sw = new StreamWriter(NewPath + @"\_your_exe_result.txt", false, ConstValues.GB18030);
            sw.Close();

            RefreshFileList();
            comboBox1.SelectedIndex = 1;

            button1.Enabled = false;
            timer1.Enabled = true;

            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(default_demo_path))
            {
                openFileDialog1.InitialDirectory = default_demo_path;
            }
            else
            {
                MutSync.ShowMessageToWarn($"原demo默认浏览目录：\n{default_demo_path}\n不存在，建议点击“设置”进行更改");
                openFileDialog1.InitialDirectory = ConstValues.DEFAULT_DIRECTORY;
            }
            openFileDialog1.Title = "选择demo文件";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }

            return;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(default_exe_path))
            {
                openFileDialog1.InitialDirectory = default_exe_path;
            }
            else
            {
                MutSync.ShowMessageToWarn($"原作业exe默认浏览目录：\n{default_exe_path}\n不存在，建议点击“设置”进行更改");
                openFileDialog1.InitialDirectory = ConstValues.DEFAULT_DIRECTORY;
            }
            openFileDialog1.Title = "选择作业exe文件";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.FileName;
            }

            return;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty || textBox2.Text == string.Empty)
            {
                MutSync.ShowMessageToWarn("官方demo路径或作业exe路径为空");
            }
            else if (!ExeErrorFilter())
            {
                ;
            }
            else
            {
                FM3.SetPath(absolute_dir_path, textBox1.Text, textBox2.Text);
                FM3.ShowDialog(this);
                if (is_data_changed)
                {
                    recorded_app_path = absolute_dir_path;
                    GenerateAndCompare();
                    is_data_changed = false;
                }
            }

            return;
        }

        private void comboBox2_comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            is_mode_changed = true;

            return;
        }

        private void textBox1_textBox2_TextChanged(object sender, EventArgs e)
        {
            is_path_changed = (textBox1.Text != project_demo_path || textBox2.Text != project_exe_path);

            return;
        }

        private void MD5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FM4.ShowDialog(this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty || textBox2.Text == string.Empty)
            {
                MutSync.ShowMessageToWarn("官方demo路径或作业exe路径为空");
            }
            else if (!ExeErrorFilter())
            {
                ;
            }
            else if (!File.Exists(absolute_dir_path + @"\test.bat"))
            {
                MutSync.ShowMessageToWarn("批处理测试文件未生成\n请先完成创建/修改测试数据");
            }
            else if (is_path_changed)
            {
                MutSync.ShowMessageToWarn("官方demo路径或作业exe路径已变更\n请先完成创建/修改测试数据\n原因：相关批处理内容与变更的路径有关");
            }
            else
            {
                if (recorded_app_path != absolute_dir_path)
                {
                    string[] bat_content = File.ReadAllLines(absolute_dir_path + @"\test.bat", ConstValues.GB18030);
                    bat_content[0] = $"cd /d \"{absolute_dir_path}\"";
                    File.WriteAllLines(absolute_dir_path + @"\test.bat", bat_content, ConstValues.GB18030);

                    recorded_app_path = absolute_dir_path;
                }

                GenerateAndCompare();
            }

            return;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(absolute_dir_path))
            {
                MutSync.OpenFolder(absolute_dir_path);
            }
            else
            {
                MutSync.ShowMessageToWarn($"项目\n{project_dir_name}\n不存在");
                RefreshFileList();
                comboBox1.SelectedIndex = 0;
            }

            return;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            button1.Enabled = true;

            return;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("注意：本操作为永久删除，无法撤销\n是否要删除项目：" + project_dir_name, "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if (Directory.Exists(absolute_dir_path))
                {
                    Directory.Delete(absolute_dir_path, true);
                }

                RefreshFileList();
                comboBox1.SelectedIndex = 0;
            }

            return;
        }

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(@".\README.html"))
            {
                Process.Start(@".\README.html");
            }
            else
            {
                MutSync.ShowMessageToWarn("说明文件\nREADME.html\n已被删除");
            }

            return;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((project_dir_name = comboBox1.SelectedItem.ToString()) == "blank")
            {
                DisableComponent();
            }
            else
            {
                absolute_dir_path = AbsoluteTestLogPath + project_dir_name;
                if (Directory.Exists(absolute_dir_path))
                {
                    TryToGetPathLogInfo();
                }
                else
                {
                    MutSync.ShowMessageToWarn($"项目\n{project_dir_name}\n已被删除，即将刷新项目列表");
                    RefreshFileList();
                    comboBox1.SelectedIndex = 0;
                }
            }

            return;
        }
    }
}
