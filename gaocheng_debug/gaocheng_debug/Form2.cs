using System;
using System.IO;
using System.Windows.Forms;

namespace gaocheng_debug
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public void SetPath(in string default_demo_path, in string default_exe_path)
        {
            textBox1.Text = default_demo_path;
            textBox2.Text = default_exe_path;

            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "选取demo默认浏览目录";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }

            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "选取用户exe默认浏览目录";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }

            return;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            File.WriteAllText(@".\rsc\initial_dirs.config", textBox1.Text + "\n" + textBox2.Text);

            Form1 Master = Owner as Form1;
            Master.DefaultDemoPath = textBox1.Text;
            Master.DefaultExePath = textBox2.Text;

            Close();
        }
    }
}
