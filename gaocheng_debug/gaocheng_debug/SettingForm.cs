using System;
using System.IO;
using System.Windows.Forms;

namespace gaocheng_debug
{
    public partial class SettingForm : Form
    {
        // 构造函数
        public SettingForm(in string defaultDemoPath, in string defaultExePath)
        {
            InitializeComponent();

            txtDemoExeDefaultPath.Text = defaultDemoPath;
            txtYourExeDefaultPath.Text = defaultExePath;
        }

        // Button事件处理
        private void BtnSelectDemoExeDefaultPathClick(object sender, EventArgs e)
        {
            fbdDemoAndYourExeDefaultPathSelector.Description = "选取demo默认浏览目录";
            if (fbdDemoAndYourExeDefaultPathSelector.ShowDialog() == DialogResult.OK)
            {
                txtDemoExeDefaultPath.Text = fbdDemoAndYourExeDefaultPathSelector.SelectedPath;
            }
        }

        private void BtnSelectYourExeDefaultPathClick(object sender, EventArgs e)
        {
            fbdDemoAndYourExeDefaultPathSelector.Description = "选取用户exe默认浏览目录";
            if (fbdDemoAndYourExeDefaultPathSelector.ShowDialog() == DialogResult.OK)
            {
                txtYourExeDefaultPath.Text = fbdDemoAndYourExeDefaultPathSelector.SelectedPath;
            }
        }

        private void BtnSaveClick(object sender, EventArgs e)
        {
            File.WriteAllText(ConstValues.InitialDirectoriesConfigRelativePath, $"{txtDemoExeDefaultPath.Text}\n{txtYourExeDefaultPath.Text}");

            MainForm Master = Owner as MainForm;
            if (Master.DefaultDemoPath != txtDemoExeDefaultPath.Text)
            {
                Master.DefaultDemoPath = txtDemoExeDefaultPath.Text;
            }
            if (Master.DefaultExePath != txtYourExeDefaultPath.Text)
            {
                Master.DefaultExePath = txtYourExeDefaultPath.Text;
            }

            Close();
        }
    }
}
