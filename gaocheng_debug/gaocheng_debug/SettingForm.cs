using System;
using System.IO;
using System.Windows.Forms;

namespace gaocheng_debug
{
    public partial class SettingForm : Form
    {
        // 私有只读成员
        private readonly MainForm Master;

        // 构造函数
        public SettingForm(in MainForm master, in string defaultDemoExeDirectory, in string defaultYourExeDirectory)
        {
            Master = master ?? throw new ArgumentNullException(nameof(master));

            InitializeComponent();

            txtDemoExeDefaultDirectory.Text = defaultDemoExeDirectory;
            txtYourExeDefaultDirectory.Text = defaultYourExeDirectory;
        }

        // Button事件处理
        private void BtnSelectDemoExeDefaultPathClick(object sender, EventArgs e)
        {
            fbdDemoAndYourExeDefaultPathSelector.Description = "选取官方demo默认浏览目录";
            if (fbdDemoAndYourExeDefaultPathSelector.ShowDialog() == DialogResult.OK)
            {
                txtDemoExeDefaultDirectory.Text = fbdDemoAndYourExeDefaultPathSelector.SelectedPath;
            }
        }

        private void BtnSelectYourExeDefaultPathClick(object sender, EventArgs e)
        {
            fbdDemoAndYourExeDefaultPathSelector.Description = "选取作业exe默认浏览目录";
            if (fbdDemoAndYourExeDefaultPathSelector.ShowDialog() == DialogResult.OK)
            {
                txtYourExeDefaultDirectory.Text = fbdDemoAndYourExeDefaultPathSelector.SelectedPath;
            }
        }

        private void BtnSaveClick(object sender, EventArgs e)
        {
            File.WriteAllText(Global.InitialDirectoriesConfigRelativePath, $"{txtDemoExeDefaultDirectory.Text}\n{txtYourExeDefaultDirectory.Text}");

            Master.DefaultDemoExeDirectory = txtDemoExeDefaultDirectory.Text;
            Master.DefaultYourExeDirectory = txtYourExeDefaultDirectory.Text;

            Close();
        }
    }
}
