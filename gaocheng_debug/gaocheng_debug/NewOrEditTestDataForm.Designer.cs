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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewOrEditTestDataForm));
            this.lblDataInputTip = new System.Windows.Forms.Label();
            this.btnGenerateOrModifyThenTest = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblTip = new System.Windows.Forms.Label();
            this.tmrSaveTipController = new System.Windows.Forms.Timer(this.components);
            this.rtxTestDataEditor = new System.Windows.Forms.RichTextBox();
            this.btnCopy = new System.Windows.Forms.Button();
            this.SuspendLayout();
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
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(114)))), ((int)(((byte)(61)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(348, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(115, 33);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "暂存(Ctrl+S)";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.BtnSaveClick);
            // 
            // lblTip
            // 
            this.lblTip.AutoSize = true;
            this.lblTip.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTip.ForeColor = System.Drawing.Color.DarkCyan;
            this.lblTip.Location = new System.Drawing.Point(183, 15);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(66, 25);
            this.lblTip.TabIndex = 4;
            this.lblTip.Text = "已保存";
            this.lblTip.Visible = false;
            // 
            // tmrSaveTipController
            // 
            this.tmrSaveTipController.Interval = 256;
            this.tmrSaveTipController.Tick += new System.EventHandler(this.TmrSaveTipControllerTick);
            // 
            // rtxTestDataEditor
            // 
            this.rtxTestDataEditor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxTestDataEditor.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxTestDataEditor.Location = new System.Drawing.Point(12, 49);
            this.rtxTestDataEditor.Name = "rtxTestDataEditor";
            this.rtxTestDataEditor.Size = new System.Drawing.Size(626, 585);
            this.rtxTestDataEditor.TabIndex = 5;
            this.rtxTestDataEditor.Text = "";
            this.rtxTestDataEditor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RtxTestDataEditorKeyDown);
            // 
            // btnCopy
            // 
            this.btnCopy.BackColor = System.Drawing.Color.White;
            this.btnCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopy.Location = new System.Drawing.Point(255, 10);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(87, 33);
            this.btnCopy.TabIndex = 6;
            this.btnCopy.Text = "一键复制";
            this.btnCopy.UseVisualStyleBackColor = false;
            this.btnCopy.Click += new System.EventHandler(this.BtnCopyClick);
            // 
            // NewOrEditTestDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(239)))));
            this.ClientSize = new System.Drawing.Size(650, 646);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.rtxTestDataEditor);
            this.Controls.Add(this.lblTip);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnGenerateOrModifyThenTest);
            this.Controls.Add(this.lblDataInputTip);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.Name = "NewOrEditTestDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "创建/修改测试数据";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewOrEditTestFormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblDataInputTip;
        private System.Windows.Forms.Button btnGenerateOrModifyThenTest;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblTip;
        private System.Windows.Forms.Timer tmrSaveTipController;
        private System.Windows.Forms.RichTextBox rtxTestDataEditor;
        private System.Windows.Forms.Button btnCopy;
    }
}