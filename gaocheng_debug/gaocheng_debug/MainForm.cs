using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace gaocheng_debug
{
    public partial class MainForm : Form
    {
        // 私有只读成员，在构造函数中初始化
        private readonly string AbsoluteTestLogPath;

        private readonly Process CMD;

        private readonly SettingForm OwnSettingForm;
        private readonly NewOrEditTestDataForm OwnNewOrEditTestDataForm;
        private readonly MD5CalculatorForm OwnMD5CalculatorForm;

        // 私有成员变量
        private string defaultDemoPath, defaultExePath;

        private bool isDataChanged, isModeChanged, isPathChanged;
        private string absoluteDirPath, projectDirName;
        private string projectDemoPath, projectExePath;
        private string recordedAppPath;

        // 属性
        public string DefaultDemoPath
        {
            set { defaultDemoPath = value; }
        }

        public string DefaultExePath
        {
            set { defaultExePath = value; }
        }

        public bool DataChanged
        {
            set { isDataChanged = value; }
        }

        // 构造函数
        public MainForm()
        { 
            AbsoluteTestLogPath = Directory.GetCurrentDirectory() + @"\test_log\";

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
                defaultDemoPath = paths[0];
                defaultExePath = paths[1];
            }

            OwnSettingForm = new SettingForm(defaultDemoPath, defaultExePath);
            OwnNewOrEditTestDataForm = new NewOrEditTestDataForm();
            OwnMD5CalculatorForm = new MD5CalculatorForm();

            RefreshFileList();
            cboProjectSelector.SelectedIndex = 0;
        }

        // 私有工具函数
        private void DisableComponent()
        {
            btnBrowseDemoExe.Enabled = false;
            btnBrowseYourExe.Enabled = false;
            btnRetest.Enabled = false;
            btnNewOrEditTestData.Enabled = false;
            btnOpenProjectDirectory.Enabled = false;
            btnDeleteProject.Enabled = false;
            txtDemoExePath.Text = string.Empty;
            txtYourExePath.Text = string.Empty;
            rtxResultViewer.Text = string.Empty;
            cboTrimSelector.SelectedIndex = 0;
            cboDisplaySelector.SelectedIndex = 0;
            cboTrimSelector.Enabled = false;
            cboDisplaySelector.Enabled = false;
        }

        private void EnableComponent()
        {
            btnBrowseDemoExe.Enabled = true;
            btnBrowseYourExe.Enabled = true;
            btnRetest.Enabled = true;
            btnNewOrEditTestData.Enabled = true;
            btnOpenProjectDirectory.Enabled = true;
            btnDeleteProject.Enabled = true;
            cboTrimSelector.Enabled = true;
            cboDisplaySelector.Enabled = true;
        }

        private void RefreshFileList()
        {
            cboProjectSelector.Items.Clear();
            cboProjectSelector.Items.Add("blank");

            string[] directories = Directory.GetDirectories(@".\test_log");
            int len = directories.Length;
            for (int i = 0; i < len; ++i)
            {
                directories[i] = Path.GetFileName(directories[i]);
            }
            Array.Sort(directories, delegate(string x, string y) { return y.CompareTo(x); });
            for (int i = 0; i < len; ++i)
            {
                cboProjectSelector.Items.Add(directories[i]);
            }
        }

        private bool ExeErrorFilter()
        {
            bool is_demo_exist = File.Exists(txtDemoExePath.Text);
            bool is_exe_exist = File.Exists(txtYourExePath.Text);
            
            if (is_demo_exist && is_exe_exist) 
            {
                return true;
            }
            else
            {
                if (!is_demo_exist && !is_exe_exist)
                {
                    MutSync.ShowMessageToWarn("demo和作业exe文件均不存在");
                }
                else if (!is_demo_exist)
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

        private void TryToGetRecordedAppPath()
        {
            if (File.Exists(absoluteDirPath + @"\test.bat"))
            {
                string temp = File.ReadAllText(absoluteDirPath + @"\test.bat", ConstValues.GB18030);
                recordedAppPath = string.Empty;
                int i = 0, len = temp.Length;
                while (i < len && temp[i] != '\"')
                {
                    ++i;
                }
                for (++i; i < len && temp[i] != '\"'; ++i)
                {
                    recordedAppPath += temp[i];
                }
            }
            else
            {
                MutSync.ShowMessageToWarn("test.bat异常，您可能删除了该文件");
            }
        }

        private void TryToGetCompareResult()
        {
            if (File.Exists(absoluteDirPath + @"\_compare_result.txt"))
            {
                rtxResultViewer.Text = File.ReadAllText(absoluteDirPath + @"\_compare_result.txt", ConstValues.GB18030);
            }
            else
            {
                MutSync.ShowMessageToWarn($"项目\n{projectDirName}\n生成的\n_compare_result.txt\n文件不存在");
                rtxResultViewer.Text = $"项目 {projectDirName} 生成的_compare_result.txt文件不存在{ConstValues.NewLine}{ConstValues.NewLine}导致本异常的原因可能是：{ConstValues.NewLine}_compare_result.txt被删除{ConstValues.NewLine}__path.log第一个参数被非法修改为generated";
            }
        }

        private void TryToGetPathLogInfo()
        {
            if (File.Exists(absoluteDirPath + @"\__path.log"))
            {
                string[] path_info = File.ReadAllLines(absoluteDirPath + @"\__path.log");

                cboTrimSelector.SelectedIndex = Convert.ToInt32(path_info[3]);
                cboDisplaySelector.SelectedIndex = Convert.ToInt32(path_info[4]);

                if (path_info[0] != "generated")
                {
                    txtDemoExePath.Text = string.Empty;
                    txtYourExePath.Text = string.Empty;
                    rtxResultViewer.Text = string.Empty;
                }
                else
                {
                    txtDemoExePath.Text = projectDemoPath = path_info[1];
                    txtYourExePath.Text = projectExePath = path_info[2];

                    TryToGetRecordedAppPath();

                    TryToGetCompareResult();
                }

                if (!btnOpenProjectDirectory.Enabled)
                {
                    EnableComponent();
                }
            }
            else
            {
                DisableComponent();
                MutSync.ShowMessageToWarn($"项目\n{projectDirName}\n生成的\n__path.log\n文件不存在");
                rtxResultViewer.Text = $"项目 {projectDirName} 生成的__path.log文件不存在{ConstValues.NewLine}{ConstValues.NewLine}导致本异常的原因可能是：{ConstValues.NewLine}__path.log被删除{ConstValues.NewLine}{ConstValues.NewLine}解决方法：{ConstValues.NewLine}尝试在回收站中寻找本项目的__path.log文件并恢复{ConstValues.NewLine}删除本项目";
                btnDeleteProject.Enabled = true;
            }
        }

        private string CompareCommandConstruct()
        {
            string compare = @"..\..\rsc\txt_compare --file1 _demo_result.txt --file2 _your_exe_result.txt --trim ";
            if (cboTrimSelector.SelectedIndex == 0)
            {
                compare += "none ";
            }
            else if (cboTrimSelector.SelectedIndex == 1)
            {
                compare += "right ";
            }
            else
            {
                compare += "left ";
            }
            compare += "--display ";
            if (cboDisplaySelector.SelectedIndex == 0)
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

            if (isModeChanged || isPathChanged)
            {
                if (isPathChanged)
                {
                    projectDemoPath = txtDemoExePath.Text;
                    projectExePath = txtYourExePath.Text;
                    isPathChanged = false;
                }
                File.WriteAllText(absoluteDirPath + @"\__path.log", $"generated\n{projectDemoPath}\n{projectExePath}\n{cboTrimSelector.SelectedIndex}\n{cboDisplaySelector.SelectedIndex}");
            }

            if (isModeChanged || isDataChanged)
            {
                isModeChanged = false;
                string[] bat_content = File.ReadAllLines(absoluteDirPath + @"\test.bat", ConstValues.GB18030);
                bat_content[bat_content.Length - 1] = CompareCommandConstruct();
                File.WriteAllLines(absoluteDirPath + @"\test.bat", bat_content, ConstValues.GB18030);
            }

            CMD.Start();
            CMD.StandardInput.WriteLine("\"" + absoluteDirPath + "\\test.bat\"");
            CMD.StandardInput.WriteLine("cls&exit");
            CMD.WaitForExit();
            CMD.Close();

            if (File.Exists(absoluteDirPath + @"\_compare_result.txt"))
            {
                rtxResultViewer.Text = File.ReadAllText(absoluteDirPath + @"\_compare_result.txt", ConstValues.GB18030);
            }
            else
            {
                rtxResultViewer.Text = $"cmd运行过程发生错误，生成失败{ConstValues.NewLine}建议检查源文件逻辑问题 以及 每组测试数据是否合法";
                MutSync.ShowMessageToWarn("cmd运行过程发生错误，生成失败");
            }
        }

        // ComboBox事件共用函数
        private void CboTrimSelectorOrCboDisplaySelectorSelectedIndexChanged(object sender, EventArgs e)
        {
            isModeChanged = true;
        }

        // TextBox事件共用函数
        private void TxtDemoExePathOrTxtYourExePathTextChanged(object sender, EventArgs e)
        {
            isPathChanged = (txtDemoExePath.Text != projectDemoPath || txtYourExePath.Text != projectExePath);
        }

        // ToolStripMenuItem事件处理函数
        private void TsmiSettingsClick(object sender, EventArgs e)
        {
            OwnSettingForm.ShowDialog(this);
        }

        private void TsmiMD5CalculatorClick(object sender, EventArgs e)
        {
            OwnMD5CalculatorForm.ShowDialog(this);
        }

        private void TsmiHelpClick(object sender, EventArgs e)
        {
            if (File.Exists(@".\README.html"))
            {
                Process.Start(@".\README.html");
            }
            else
            {
                MutSync.ShowMessageToWarn("说明文件\nREADME.html\n已被删除");
            }
        }

        // Button事件处理函数
        private void BtnNewProjectClick(object sender, EventArgs e)
        {
            string new_path = AbsoluteTestLogPath + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            Directory.CreateDirectory(new_path);

            StreamWriter sw = new StreamWriter(new_path + @"\__path.log");
            sw.Write("new\nno_data\nno_data\n0\n0");
            sw.Close();

            sw = new StreamWriter(new_path + @"\__test_data.txt", false, ConstValues.GB18030);
            sw.Close();

            sw = new StreamWriter(new_path + @"\_compare_result.txt", false, ConstValues.GB18030);
            sw.Close();

            sw = new StreamWriter(new_path + @"\_demo_result.txt", false, ConstValues.GB18030);
            sw.Close();

            sw = new StreamWriter(new_path + @"\_your_exe_result.txt", false, ConstValues.GB18030);
            sw.Close();

            RefreshFileList();
            cboProjectSelector.SelectedIndex = 1;

            btnNewProject.Enabled = false;
            tmrWarrantyOfProjectUniqueness.Enabled = true;
        }

        private void BtnDeleteProjectClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("注意：本操作为永久删除，无法撤销\n是否要删除项目：" + projectDirName, "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if (Directory.Exists(absoluteDirPath))
                {
                    Directory.Delete(absoluteDirPath, true);
                }

                RefreshFileList();
                cboProjectSelector.SelectedIndex = 0;
            }
        }

        private void BtnOpenProjectDirectoryClick(object sender, EventArgs e)
        {
            if (Directory.Exists(absoluteDirPath))
            {
                MutSync.OpenFolder(absoluteDirPath);
            }
            else
            {
                MutSync.ShowMessageToWarn($"项目\n{projectDirName}\n不存在");
                RefreshFileList();
                cboProjectSelector.SelectedIndex = 0;
            }
        }

        private void BtnBrowseDemoExeClick(object sender, EventArgs e)
        {
            if (Directory.Exists(defaultDemoPath))
            {
                ofdDemoAndYourExeSelector.InitialDirectory = defaultDemoPath;
            }
            else
            {
                MutSync.ShowMessageToWarn($"原demo默认浏览目录：\n{defaultDemoPath}\n不存在，建议点击“设置”进行更改");
                ofdDemoAndYourExeSelector.InitialDirectory = ConstValues.DefaultDirectory;
            }
            ofdDemoAndYourExeSelector.Title = "选择demo文件";
            if (ofdDemoAndYourExeSelector.ShowDialog() == DialogResult.OK)
            {
                txtDemoExePath.Text = ofdDemoAndYourExeSelector.FileName;
            }
        }

        private void BtnBrowseYourExeClick(object sender, EventArgs e)
        {
            if (Directory.Exists(defaultExePath))
            {
                ofdDemoAndYourExeSelector.InitialDirectory = defaultExePath;
            }
            else
            {
                MutSync.ShowMessageToWarn($"原作业exe默认浏览目录：\n{defaultExePath}\n不存在，建议点击“设置”进行更改");
                ofdDemoAndYourExeSelector.InitialDirectory = ConstValues.DefaultDirectory;
            }
            ofdDemoAndYourExeSelector.Title = "选择作业exe文件";
            if (ofdDemoAndYourExeSelector.ShowDialog() == DialogResult.OK)
            {
                txtYourExePath.Text = ofdDemoAndYourExeSelector.FileName;
            }
        }

        private void BtnNewOrEditTestDataClick(object sender, EventArgs e)
        {
            if (txtDemoExePath.Text == string.Empty || txtYourExePath.Text == string.Empty)
            {
                MutSync.ShowMessageToWarn("官方demo路径或作业exe路径为空");
            }
            else if (!ExeErrorFilter())
            {
                ;
            }
            else
            {
                OwnNewOrEditTestDataForm.SetPath(absoluteDirPath, txtDemoExePath.Text, txtYourExePath.Text);
                OwnNewOrEditTestDataForm.ShowDialog(this);
                if (isDataChanged)
                {
                    recordedAppPath = absoluteDirPath;
                    GenerateAndCompare();
                    isDataChanged = false;
                }
            }
        }

        private void BtnRetestClick(object sender, EventArgs e)
        {
            if (txtDemoExePath.Text == string.Empty || txtYourExePath.Text == string.Empty)
            {
                MutSync.ShowMessageToWarn("官方demo路径或作业exe路径为空");
            }
            else if (!ExeErrorFilter())
            {
                ;
            }
            else if (!File.Exists(absoluteDirPath + @"\test.bat"))
            {
                MutSync.ShowMessageToWarn("批处理测试文件未生成\n请先完成创建/修改测试数据");
            }
            else if (isPathChanged)
            {
                MutSync.ShowMessageToWarn("官方demo路径或作业exe路径已变更\n请先完成创建/修改测试数据\n原因：相关批处理内容与变更的路径有关");
            }
            else
            {
                if (recordedAppPath != absoluteDirPath)
                {
                    string[] bat_content = File.ReadAllLines(absoluteDirPath + @"\test.bat", ConstValues.GB18030);
                    bat_content[0] = $"cd /d \"{absoluteDirPath}\"";
                    File.WriteAllLines(absoluteDirPath + @"\test.bat", bat_content, ConstValues.GB18030);

                    recordedAppPath = absoluteDirPath;
                }

                GenerateAndCompare();
            }
        }

        // ComboBox事件处理函数
        private void CboProjectSelectorSelectedIndexChanged(object sender, EventArgs e)
        {
            if ((projectDirName = cboProjectSelector.SelectedItem.ToString()) == "blank")
            {
                DisableComponent();
            }
            else
            {
                absoluteDirPath = AbsoluteTestLogPath + projectDirName;
                if (Directory.Exists(absoluteDirPath))
                {
                    TryToGetPathLogInfo();
                }
                else
                {
                    MutSync.ShowMessageToWarn($"项目\n{projectDirName}\n已被删除，即将刷新项目列表");
                    RefreshFileList();
                    cboProjectSelector.SelectedIndex = 0;
                }
            }
        }

        // Timer事件处理函数
        private void TmrWarrantyOfProjectUniquenessTick(object sender, EventArgs e)
        {
            tmrWarrantyOfProjectUniqueness.Enabled = false;
            btnNewProject.Enabled = true;
        }
    }
}
