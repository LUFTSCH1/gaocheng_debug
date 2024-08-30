namespace gaocheng_debug
{
    partial class MD5CalculatorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MD5CalculatorForm));
            this.btnSelectFileAndCalculateMD5 = new System.Windows.Forms.Button();
            this.txtResultViewer = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // btnSelectFileAndCalculateMD5
            // 
            this.btnSelectFileAndCalculateMD5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(190)))), ((int)(((byte)(138)))));
            this.btnSelectFileAndCalculateMD5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectFileAndCalculateMD5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectFileAndCalculateMD5.Location = new System.Drawing.Point(475, 11);
            this.btnSelectFileAndCalculateMD5.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnSelectFileAndCalculateMD5.Name = "btnSelectFileAndCalculateMD5";
            this.btnSelectFileAndCalculateMD5.Size = new System.Drawing.Size(139, 33);
            this.btnSelectFileAndCalculateMD5.TabIndex = 0;
            this.btnSelectFileAndCalculateMD5.Text = "选择文件并计算";
            this.btnSelectFileAndCalculateMD5.UseVisualStyleBackColor = false;
            this.btnSelectFileAndCalculateMD5.Click += new System.EventHandler(this.BtnSelectFileAndCalculateMD5ClickAsync);
            // 
            // txtResultViewer
            // 
            this.txtResultViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtResultViewer.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResultViewer.Location = new System.Drawing.Point(10, 50);
            this.txtResultViewer.Margin = new System.Windows.Forms.Padding(2);
            this.txtResultViewer.Multiline = true;
            this.txtResultViewer.Name = "txtResultViewer";
            this.txtResultViewer.ReadOnly = true;
            this.txtResultViewer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultViewer.Size = new System.Drawing.Size(604, 194);
            this.txtResultViewer.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "cpp";
            this.openFileDialog1.Filter = "所有文件|*.*|cpp源文件|*.cpp|c源文件|*.c|rar压缩文件|*.rar|zip压缩文件|*.zip";
            this.openFileDialog1.Title = "选择需要计算MD5的文件";
            // 
            // MD5CalculatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(239)))));
            this.ClientSize = new System.Drawing.Size(624, 254);
            this.Controls.Add(this.txtResultViewer);
            this.Controls.Add(this.btnSelectFileAndCalculateMD5);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.Name = "MD5CalculatorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "计算文件MD5";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectFileAndCalculateMD5;
        private System.Windows.Forms.TextBox txtResultViewer;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}