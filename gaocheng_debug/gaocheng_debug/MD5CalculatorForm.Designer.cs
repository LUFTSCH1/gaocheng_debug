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
            this.ofdFilePathBrowser = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // btnSelectFileAndCalculateMD5
            // 
            this.btnSelectFileAndCalculateMD5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(190)))), ((int)(((byte)(138)))));
            this.btnSelectFileAndCalculateMD5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectFileAndCalculateMD5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectFileAndCalculateMD5.Location = new System.Drawing.Point(455, 12);
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
            this.txtResultViewer.Size = new System.Drawing.Size(584, 213);
            this.txtResultViewer.TabIndex = 1;
            this.txtResultViewer.TextChanged += new System.EventHandler(this.TxtResultViewerTextChanged);
            // 
            // ofdFilePathBrowser
            // 
            this.ofdFilePathBrowser.DefaultExt = "cpp";
            this.ofdFilePathBrowser.Filter = "所有文件|*.*|cpp源文件|*.cpp|c源文件|*.c|pdf文件|*.pdf|rar压缩文件|*.rar|pptx文件|*.pptx|exe文件|*exe" +
    "|zip压缩文件|*.zip|7z压缩文件|*.7z";
            this.ofdFilePathBrowser.Title = "选择需要计算MD5值的文件";
            // 
            // MD5CalculatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(239)))));
            this.ClientSize = new System.Drawing.Size(605, 274);
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MD5CalculatorFormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectFileAndCalculateMD5;
        private System.Windows.Forms.TextBox txtResultViewer;
        private System.Windows.Forms.OpenFileDialog ofdFilePathBrowser;
    }
}