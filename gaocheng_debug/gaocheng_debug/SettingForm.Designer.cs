namespace gaocheng_debug
{
    partial class SettingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
            this.btnSelectDemoExeDefaultPath = new System.Windows.Forms.Button();
            this.lblDemoExeDefaultDirectoryTip = new System.Windows.Forms.Label();
            this.txtDemoExeDefaultDirectory = new System.Windows.Forms.TextBox();
            this.lblYourExeDefaultDirectoryTip = new System.Windows.Forms.Label();
            this.txtYourExeDefaultDirectory = new System.Windows.Forms.TextBox();
            this.btnSelectYourExeDefaultPath = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.fbdDemoAndYourExeDefaultPathSelector = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // btnSelectDemoExeDefaultPath
            // 
            this.btnSelectDemoExeDefaultPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(187)))), ((int)(((byte)(208)))));
            this.btnSelectDemoExeDefaultPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectDemoExeDefaultPath.Location = new System.Drawing.Point(649, 22);
            this.btnSelectDemoExeDefaultPath.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnSelectDemoExeDefaultPath.Name = "btnSelectDemoExeDefaultPath";
            this.btnSelectDemoExeDefaultPath.Size = new System.Drawing.Size(70, 32);
            this.btnSelectDemoExeDefaultPath.TabIndex = 0;
            this.btnSelectDemoExeDefaultPath.TabStop = false;
            this.btnSelectDemoExeDefaultPath.Text = "选择";
            this.btnSelectDemoExeDefaultPath.UseVisualStyleBackColor = false;
            this.btnSelectDemoExeDefaultPath.Click += new System.EventHandler(this.BtnSelectDemoExeDefaultPathClick);
            // 
            // lblDemoExeDefaultDirectoryTip
            // 
            this.lblDemoExeDefaultDirectoryTip.AutoSize = true;
            this.lblDemoExeDefaultDirectoryTip.Location = new System.Drawing.Point(7, 6);
            this.lblDemoExeDefaultDirectoryTip.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDemoExeDefaultDirectoryTip.Name = "lblDemoExeDefaultDirectoryTip";
            this.lblDemoExeDefaultDirectoryTip.Size = new System.Drawing.Size(172, 20);
            this.lblDemoExeDefaultDirectoryTip.TabIndex = 1;
            this.lblDemoExeDefaultDirectoryTip.Text = "官方demo默认浏览目录";
            // 
            // txtDemoExeDefaultDirectory
            // 
            this.txtDemoExeDefaultDirectory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDemoExeDefaultDirectory.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDemoExeDefaultDirectory.Location = new System.Drawing.Point(11, 28);
            this.txtDemoExeDefaultDirectory.Margin = new System.Windows.Forms.Padding(2);
            this.txtDemoExeDefaultDirectory.Name = "txtDemoExeDefaultDirectory";
            this.txtDemoExeDefaultDirectory.ReadOnly = true;
            this.txtDemoExeDefaultDirectory.Size = new System.Drawing.Size(634, 25);
            this.txtDemoExeDefaultDirectory.TabIndex = 2;
            this.txtDemoExeDefaultDirectory.TabStop = false;
            // 
            // lblYourExeDefaultDirectoryTip
            // 
            this.lblYourExeDefaultDirectoryTip.AutoSize = true;
            this.lblYourExeDefaultDirectoryTip.Location = new System.Drawing.Point(7, 64);
            this.lblYourExeDefaultDirectoryTip.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblYourExeDefaultDirectoryTip.Name = "lblYourExeDefaultDirectoryTip";
            this.lblYourExeDefaultDirectoryTip.Size = new System.Drawing.Size(155, 20);
            this.lblYourExeDefaultDirectoryTip.TabIndex = 3;
            this.lblYourExeDefaultDirectoryTip.Text = "作业exe默认浏览目录";
            // 
            // txtYourExeDefaultDirectory
            // 
            this.txtYourExeDefaultDirectory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtYourExeDefaultDirectory.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtYourExeDefaultDirectory.Location = new System.Drawing.Point(11, 86);
            this.txtYourExeDefaultDirectory.Margin = new System.Windows.Forms.Padding(2);
            this.txtYourExeDefaultDirectory.Name = "txtYourExeDefaultDirectory";
            this.txtYourExeDefaultDirectory.ReadOnly = true;
            this.txtYourExeDefaultDirectory.Size = new System.Drawing.Size(634, 25);
            this.txtYourExeDefaultDirectory.TabIndex = 4;
            this.txtYourExeDefaultDirectory.TabStop = false;
            // 
            // btnSelectYourExeDefaultPath
            // 
            this.btnSelectYourExeDefaultPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(187)))), ((int)(((byte)(208)))));
            this.btnSelectYourExeDefaultPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectYourExeDefaultPath.Location = new System.Drawing.Point(649, 80);
            this.btnSelectYourExeDefaultPath.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectYourExeDefaultPath.Name = "btnSelectYourExeDefaultPath";
            this.btnSelectYourExeDefaultPath.Size = new System.Drawing.Size(70, 32);
            this.btnSelectYourExeDefaultPath.TabIndex = 5;
            this.btnSelectYourExeDefaultPath.TabStop = false;
            this.btnSelectYourExeDefaultPath.Text = "选择";
            this.btnSelectYourExeDefaultPath.UseVisualStyleBackColor = false;
            this.btnSelectYourExeDefaultPath.Click += new System.EventHandler(this.BtnSelectYourExeDefaultPathClick);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(187)))), ((int)(((byte)(138)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(649, 135);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(70, 32);
            this.btnSave.TabIndex = 6;
            this.btnSave.TabStop = false;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.BtnSaveClick);
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(239)))));
            this.ClientSize = new System.Drawing.Size(731, 178);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnSelectYourExeDefaultPath);
            this.Controls.Add(this.txtYourExeDefaultDirectory);
            this.Controls.Add(this.lblYourExeDefaultDirectoryTip);
            this.Controls.Add(this.txtDemoExeDefaultDirectory);
            this.Controls.Add(this.lblDemoExeDefaultDirectoryTip);
            this.Controls.Add(this.btnSelectDemoExeDefaultPath);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.Name = "SettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectDemoExeDefaultPath;
        private System.Windows.Forms.Label lblDemoExeDefaultDirectoryTip;
        private System.Windows.Forms.TextBox txtDemoExeDefaultDirectory;
        private System.Windows.Forms.Label lblYourExeDefaultDirectoryTip;
        private System.Windows.Forms.TextBox txtYourExeDefaultDirectory;
        private System.Windows.Forms.Button btnSelectYourExeDefaultPath;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.FolderBrowserDialog fbdDemoAndYourExeDefaultPathSelector;
    }
}