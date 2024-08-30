namespace gaocheng_debug
{
    partial class NewOrEditTestDataForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewOrEditTestDataForm));
            this.txtTestData = new System.Windows.Forms.TextBox();
            this.lblDataInputTip = new System.Windows.Forms.Label();
            this.btnGenerateOrModifyThenTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtTestData
            // 
            this.txtTestData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTestData.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTestData.Location = new System.Drawing.Point(10, 48);
            this.txtTestData.Margin = new System.Windows.Forms.Padding(2);
            this.txtTestData.Multiline = true;
            this.txtTestData.Name = "txtTestData";
            this.txtTestData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTestData.Size = new System.Drawing.Size(630, 588);
            this.txtTestData.TabIndex = 0;
            // 
            // lblDataInputTip
            // 
            this.lblDataInputTip.AutoSize = true;
            this.lblDataInputTip.Location = new System.Drawing.Point(10, 18);
            this.lblDataInputTip.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDataInputTip.Name = "lblDataInputTip";
            this.lblDataInputTip.Size = new System.Drawing.Size(84, 20);
            this.lblDataInputTip.TabIndex = 1;
            this.lblDataInputTip.Text = "输入数据：";
            // 
            // btnGenerateOrModifyThenTest
            // 
            this.btnGenerateOrModifyThenTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(187)))), ((int)(((byte)(138)))));
            this.btnGenerateOrModifyThenTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateOrModifyThenTest.Location = new System.Drawing.Point(468, 10);
            this.btnGenerateOrModifyThenTest.Margin = new System.Windows.Forms.Padding(2);
            this.btnGenerateOrModifyThenTest.Name = "btnGenerateOrModifyThenTest";
            this.btnGenerateOrModifyThenTest.Size = new System.Drawing.Size(172, 33);
            this.btnGenerateOrModifyThenTest.TabIndex = 2;
            this.btnGenerateOrModifyThenTest.TabStop = false;
            this.btnGenerateOrModifyThenTest.Text = "生成/修改并开始测试";
            this.btnGenerateOrModifyThenTest.UseVisualStyleBackColor = false;
            this.btnGenerateOrModifyThenTest.Click += new System.EventHandler(this.BtnGenerateOrModifyThenTestClick);
            // 
            // NewOrEditTestDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(239)))));
            this.ClientSize = new System.Drawing.Size(650, 646);
            this.Controls.Add(this.btnGenerateOrModifyThenTest);
            this.Controls.Add(this.lblDataInputTip);
            this.Controls.Add(this.txtTestData);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.Name = "NewOrEditTestDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "创建/修改测试数据";
            this.Load += new System.EventHandler(this.NewOrEditTestDataFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTestData;
        private System.Windows.Forms.Label lblDataInputTip;
        private System.Windows.Forms.Button btnGenerateOrModifyThenTest;
    }
}