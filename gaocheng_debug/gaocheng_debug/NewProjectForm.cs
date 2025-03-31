using System;
using System.Windows.Forms;

namespace gaocheng_debug
{
    public partial class NewProjectForm : Form
    {
        // 私有只读成员

        private readonly MainForm Master;

        // 构造函数
        public NewProjectForm(in MainForm master)
        {
            Master = master ?? throw new ArgumentNullException(nameof(master));
            DialogResult = DialogResult.Cancel;

            InitializeComponent();
            cboChapter.SelectedIndex = 0;
            cboProblem.SelectedIndex = 0;
            cboQuestion.SelectedIndex = 0;
        }

        // Button事件处理

        private void BtnNewProjectClick(object sender, EventArgs e)
        {
            string cbq = $"{cboChapter.SelectedItem}-b{cboProblem.SelectedItem}";
            if (chkIsLastEnabled.Checked)
            {
                cbq += $"-{cboQuestion.SelectedItem}";
            }
            Master.NewProjectDirName = cbq;
            DialogResult = DialogResult.OK;
            Close();
        }

        // Checkbox事件处理

        private void ChkIsLastEnabledCheckedChanged(object sender, EventArgs e)
        {
            lblProblemQuestion.Visible = cboQuestion.Visible = chkIsLastEnabled.Checked;
        }
    }
}
