using System;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace gaocheng_debug
{
    public partial class Form1 : Form
    {
        private readonly Encoding GB18030;
        private readonly string absolute_test_log_path;

        private readonly Form2 fm2 = new Form2();
        private readonly Form3 fm3;
        private readonly Form4 fm4 = new Form4();

        private string default_demo_path, default_exe_path;

        private bool is_data_changed = false, is_mode_changed = false, is_path_changed = false;
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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            GB18030 = Encoding.GetEncoding("GB18030");

            fm3 = new Form3(GB18030);

            absolute_test_log_path = Directory.GetCurrentDirectory() + @"\test_log\";

            if (!Directory.Exists(absolute_test_log_path))
            {
                Directory.CreateDirectory(absolute_test_log_path);
            }

            InitializeComponent();

            BackColor = Color.FromArgb(234, 234, 239);
            button1.BackColor = button5.BackColor = Color.FromArgb(93, 190, 138);
            button2.BackColor = button3.BackColor = Color.FromArgb(99, 187, 208);
            button4.BackColor = Color.FromArgb(255, 255, 255);
            button6.BackColor = Color.FromArgb(249, 114, 61);
            button7.BackColor = Color.FromArgb(222, 28, 49);

            string[] form1_names = { "校对工具", "oop，启动！", "高程，启动！", "QAQ", "Ciallo～(∠・ω< )⌒★", "兄弟，写多久了？", "是兄弟，就来田野打架1捞我", "让我康康你的小红车" , "(✿╹◡╹)", "٩( ╹▿╹ )۶" };
            Random rnd = new Random();
            Text = form1_names[rnd.Next(0, form1_names.Length)];

            StreamReader sr = new StreamReader(@".\rsc\initial_dirs.config");
            string temp = sr.ReadToEnd();
            sr.Close();
            string[] paths = temp.Split('\n');
            default_demo_path = paths[0];
            default_exe_path = paths[1];

            rfFiles();
            comboBox1.SelectedIndex = 0;
        }

        private bool exe_error_filter(in string demo_path, in string your_exe_path)
        {
            bool isDemoExist = File.Exists(demo_path);
            bool isYourExeExist = File.Exists(your_exe_path);
            
            if (isDemoExist && isYourExeExist) 
            {
                return true;
            }
            else
            {
                if (!isDemoExist && !isYourExeExist)
                {
                    MessageBox.Show("demo和用户exe文件均不存在", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!isDemoExist)
                {
                    MessageBox.Show("demo不存在", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("用户exe文件不存在", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                return false;
            }
        }

        private void disableComponent()
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

        private void enableComponent()
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

        private string compareConstruct()
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

        private void rfFiles()
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

        private void generateAndCompare()
        {
            project_demo_path = textBox1.Text;
            project_exe_path = textBox2.Text;

            if (!exe_error_filter(project_demo_path, project_exe_path))
            {
                return;
            }

            StreamReader bat_resultSR;
            StreamWriter log_batSW;

            if (is_mode_changed || is_path_changed)
            {
                is_path_changed = false;
                log_batSW = new StreamWriter(absolute_dir_path + @"\__path.log");
                log_batSW.Write(string.Format("generated\n{0}\n{1}\n{2}\n{3}", project_demo_path, project_exe_path, comboBox2.SelectedIndex, comboBox3.SelectedIndex));
                log_batSW.Close();
            }

            if (is_mode_changed || is_data_changed)
            {
                is_mode_changed = false;
                bat_resultSR = new StreamReader(absolute_dir_path + @"\test.bat", GB18030);
                string test_content = bat_resultSR.ReadToEnd();
                bat_resultSR.Close();

                string[] test_command = test_content.Split('\n');
                log_batSW = new StreamWriter(absolute_dir_path + @"\test.bat", false, GB18030);
                for (int i = 0, len = test_command.Length - 1; i < len; ++i)
                {
                    log_batSW.WriteLine(test_command[i].Trim('\r'));
                }
                log_batSW.Write(compareConstruct());
                log_batSW.Close();
            }

            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.RedirectStandardError = false;
            p.StartInfo.CreateNoWindow = false;

            p.Start();
            p.StandardInput.WriteLine("\"" + absolute_dir_path + "\\test.bat\"&exit");
            p.WaitForExit();
            p.Close();

            bat_resultSR = new StreamReader(absolute_dir_path + @"\_compare_result.txt", GB18030);
            richTextBox1.Text = bat_resultSR.ReadToEnd();
            bat_resultSR.Close();
            
            return;
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fm2.SetPath(default_demo_path, default_exe_path);
            fm2.ShowDialog(this);

            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string NewPath = absolute_test_log_path + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            Directory.CreateDirectory(NewPath);

            StreamWriter sw = new StreamWriter(NewPath + @"\__path.log");
            sw.Write("new\nno_data\nno_data\n0\n0");
            sw.Close();

            sw = new StreamWriter(NewPath + @"\__test_data.txt", false, GB18030);
            sw.Close();

            sw = new StreamWriter(NewPath + @"\_compare_result.txt", false, GB18030);
            sw.Close();

            sw = new StreamWriter(NewPath + @"\_demo_result.txt", false, GB18030);
            sw.Close();

            sw = new StreamWriter(NewPath + @"\_your_exe_result.txt", false, GB18030);
            sw.Close();

            rfFiles();
            comboBox1.SelectedIndex = 1;

            button1.Enabled = false;
            timer1.Enabled = true;

            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = default_demo_path;
            openFileDialog1.Title = "选择demo文件";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }

            return;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = default_exe_path;
            openFileDialog1.Title = "选择用户exe文件";
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
                MessageBox.Show("官方demo路径或测试exe路径为空", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                fm3.SetPath(absolute_dir_path, textBox1.Text, textBox2.Text);
                fm3.ShowDialog(this);
                if (is_data_changed)
                {
                    recorded_app_path = absolute_dir_path;
                    generateAndCompare();
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
            fm4.ShowDialog(this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty || textBox2.Text == string.Empty)
            {
                MessageBox.Show("官方demo路径或测试exe路径为空", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!File.Exists(absolute_dir_path + @"\test.bat"))
            {
                MessageBox.Show("批处理测试文件未生成\n请先完成创建/修改测试数据", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!exe_error_filter(textBox1.Text, textBox2.Text))
            {
                ;
            }
            else if (is_path_changed)
            {
                MessageBox.Show("官方demo路径或测试exe路径已变更\n请先完成创建/修改测试数据\n原因：相关批处理内容与变更的路径有关", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (recorded_app_path != absolute_dir_path)
            {
                MessageBox.Show("本应用位置已变更\n请先完成创建/修改测试数据\n原因：相关批处理内容与变更的路径有关", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                generateAndCompare();
            }

            return;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MutSync.OpenFolder(absolute_dir_path);

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
                Directory.Delete(absolute_dir_path, true);

                rfFiles();
                comboBox1.SelectedIndex = 0;
            }

            return;
        }

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(@".\README.html");

            return;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((project_dir_name = comboBox1.SelectedItem.ToString()) == "blank")
            {
                disableComponent();
            }
            else
            {
                absolute_dir_path = absolute_test_log_path + project_dir_name;

                StreamReader sr = new StreamReader(absolute_dir_path + @"\__path.log");
                string pathI = sr.ReadToEnd();
                sr.Close();
                string[] pathInfo = pathI.Split('\n');

                comboBox2.SelectedIndex = Convert.ToInt32(pathInfo[3]);
                comboBox3.SelectedIndex = Convert.ToInt32(pathInfo[4]);

                if (pathInfo[0] != "generated")
                {
                    textBox1.Text = string.Empty;
                    textBox2.Text = string.Empty;
                    richTextBox1.Text = string.Empty;
                    richTextBox1.Text = string.Empty;
                }
                else
                {
                    textBox1.Text = project_demo_path = pathInfo[1];
                    textBox2.Text = project_exe_path = pathInfo[2];

                    try
                    {
                        sr = new StreamReader(absolute_dir_path + @"\test.bat", GB18030);
                        string temp = sr.ReadLine();
                        sr.Close();
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
                    catch (Exception ex)
                    {
                        throw new Exception("test.bat异常，您可能删除了该文件：\n" + ex.Message);
                    }

                    sr = new StreamReader(absolute_dir_path + @"\_compare_result.txt", GB18030);
                    richTextBox1.Text = sr.ReadToEnd();
                    sr.Close();
                }

                enableComponent();
            }

            return;
        }
    }
}
