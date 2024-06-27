using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace gaocheng_debug
{
    public partial class Form2 : Form
    {
        public Form2(in string default_demo_path, in string default_exe_path)
        {
            InitializeComponent();
            textBox1.Text = default_demo_path;
            textBox2.Text = default_exe_path;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            BackColor = Color.FromArgb(234, 234, 239);
            button1.BackColor = button2.BackColor = Color.FromArgb(99, 187, 208);
            button3.BackColor = Color.FromArgb(85, 187, 138);

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
            StreamWriter sw = new StreamWriter(@".\rsc\initial_dirs.config");
            sw.Write(textBox1.Text + "\n" + textBox2.Text);
            sw.Close();

            Form1 f1 = Owner as Form1;
            f1.default_dp = textBox1.Text;
            f1.default_ep = textBox2.Text;

            Close();
        }
    }
}
