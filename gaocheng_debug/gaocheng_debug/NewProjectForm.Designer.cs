namespace gaocheng_debug
{
    partial class NewProjectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewProjectForm));
            this.cboChapter = new System.Windows.Forms.ComboBox();
            this.cboProblem = new System.Windows.Forms.ComboBox();
            this.cboQuestion = new System.Windows.Forms.ComboBox();
            this.chkIsLastEnabled = new System.Windows.Forms.CheckBox();
            this.btnNewProject = new System.Windows.Forms.Button();
            this.lblChapterProblem = new System.Windows.Forms.Label();
            this.lblProblemQuestion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cboChapter
            // 
            this.cboChapter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboChapter.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboChapter.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboChapter.FormattingEnabled = true;
            this.cboChapter.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16"});
            this.cboChapter.Location = new System.Drawing.Point(12, 12);
            this.cboChapter.Name = "cboChapter";
            this.cboChapter.Size = new System.Drawing.Size(56, 30);
            this.cboChapter.TabIndex = 0;
            // 
            // cboProblem
            // 
            this.cboProblem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProblem.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboProblem.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboProblem.FormattingEnabled = true;
            this.cboProblem.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32"});
            this.cboProblem.Location = new System.Drawing.Point(112, 12);
            this.cboProblem.Name = "cboProblem";
            this.cboProblem.Size = new System.Drawing.Size(56, 30);
            this.cboProblem.TabIndex = 1;
            // 
            // cboQuestion
            // 
            this.cboQuestion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboQuestion.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboQuestion.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboQuestion.FormattingEnabled = true;
            this.cboQuestion.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16"});
            this.cboQuestion.Location = new System.Drawing.Point(201, 12);
            this.cboQuestion.Name = "cboQuestion";
            this.cboQuestion.Size = new System.Drawing.Size(56, 30);
            this.cboQuestion.TabIndex = 2;
            this.cboQuestion.Visible = false;
            // 
            // chkIsLastEnabled
            // 
            this.chkIsLastEnabled.AutoSize = true;
            this.chkIsLastEnabled.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkIsLastEnabled.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkIsLastEnabled.Location = new System.Drawing.Point(12, 54);
            this.chkIsLastEnabled.Name = "chkIsLastEnabled";
            this.chkIsLastEnabled.Size = new System.Drawing.Size(130, 25);
            this.chkIsLastEnabled.TabIndex = 3;
            this.chkIsLastEnabled.Text = "启用小题序号";
            this.chkIsLastEnabled.UseVisualStyleBackColor = true;
            this.chkIsLastEnabled.CheckedChanged += new System.EventHandler(this.ChkIsLastEnabledCheckedChanged);
            // 
            // btnNewProject
            // 
            this.btnNewProject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(190)))), ((int)(((byte)(138)))));
            this.btnNewProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewProject.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnNewProject.Location = new System.Drawing.Point(152, 51);
            this.btnNewProject.Name = "btnNewProject";
            this.btnNewProject.Size = new System.Drawing.Size(105, 29);
            this.btnNewProject.TabIndex = 4;
            this.btnNewProject.Text = "确认并新建";
            this.btnNewProject.UseVisualStyleBackColor = false;
            this.btnNewProject.Click += new System.EventHandler(this.BtnNewProjectClick);
            // 
            // lblChapterProblem
            // 
            this.lblChapterProblem.AutoSize = true;
            this.lblChapterProblem.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChapterProblem.Location = new System.Drawing.Point(74, 15);
            this.lblChapterProblem.Name = "lblChapterProblem";
            this.lblChapterProblem.Size = new System.Drawing.Size(32, 23);
            this.lblChapterProblem.TabIndex = 5;
            this.lblChapterProblem.Text = "-b";
            // 
            // lblProblemQuestion
            // 
            this.lblProblemQuestion.AutoSize = true;
            this.lblProblemQuestion.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProblemQuestion.Location = new System.Drawing.Point(174, 15);
            this.lblProblemQuestion.Name = "lblProblemQuestion";
            this.lblProblemQuestion.Size = new System.Drawing.Size(21, 23);
            this.lblProblemQuestion.TabIndex = 6;
            this.lblProblemQuestion.Text = "-";
            this.lblProblemQuestion.Visible = false;
            // 
            // NewProjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(239)))));
            this.ClientSize = new System.Drawing.Size(269, 91);
            this.Controls.Add(this.lblProblemQuestion);
            this.Controls.Add(this.lblChapterProblem);
            this.Controls.Add(this.btnNewProject);
            this.Controls.Add(this.chkIsLastEnabled);
            this.Controls.Add(this.cboQuestion);
            this.Controls.Add(this.cboProblem);
            this.Controls.Add(this.cboChapter);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewProjectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "命名新测试项目";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboChapter;
        private System.Windows.Forms.ComboBox cboProblem;
        private System.Windows.Forms.ComboBox cboQuestion;
        private System.Windows.Forms.CheckBox chkIsLastEnabled;
        private System.Windows.Forms.Button btnNewProject;
        private System.Windows.Forms.Label lblChapterProblem;
        private System.Windows.Forms.Label lblProblemQuestion;
    }
}