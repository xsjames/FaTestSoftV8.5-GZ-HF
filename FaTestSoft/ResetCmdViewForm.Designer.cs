﻿namespace FaTestSoft
{
    partial class ResetCmdViewForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radiocleardata = new System.Windows.Forms.RadioButton();
            this.radioinitparam = new System.Windows.Forms.RadioButton();
            this.radioreset = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxpassword = new System.Windows.Forms.TextBox();
            this.buttonexe = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.radioclearled = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.BackColor = System.Drawing.Color.Cornsilk;
            this.groupBox1.Controls.Add(this.radioclearled);
            this.groupBox1.Controls.Add(this.radiocleardata);
            this.groupBox1.Controls.Add(this.radioinitparam);
            this.groupBox1.Controls.Add(this.radioreset);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(589, 211);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择项";
            // 
            // radiocleardata
            // 
            this.radiocleardata.AutoSize = true;
            this.radiocleardata.Location = new System.Drawing.Point(23, 113);
            this.radiocleardata.Name = "radiocleardata";
            this.radiocleardata.Size = new System.Drawing.Size(83, 16);
            this.radiocleardata.TabIndex = 2;
            this.radiocleardata.TabStop = true;
            this.radiocleardata.Text = "数据区清空";
            this.radiocleardata.UseVisualStyleBackColor = true;
            this.radiocleardata.CheckedChanged += new System.EventHandler(this.radiocleardata_CheckedChanged);
            // 
            // radioinitparam
            // 
            this.radioinitparam.AutoSize = true;
            this.radioinitparam.Location = new System.Drawing.Point(23, 72);
            this.radioinitparam.Name = "radioinitparam";
            this.radioinitparam.Size = new System.Drawing.Size(119, 16);
            this.radioinitparam.TabIndex = 1;
            this.radioinitparam.TabStop = true;
            this.radioinitparam.Text = "参数恢复出厂设置";
            this.radioinitparam.UseVisualStyleBackColor = true;
            this.radioinitparam.CheckedChanged += new System.EventHandler(this.radioinitparam_CheckedChanged);
            // 
            // radioreset
            // 
            this.radioreset.AutoSize = true;
            this.radioreset.Location = new System.Drawing.Point(23, 30);
            this.radioreset.Name = "radioreset";
            this.radioreset.Size = new System.Drawing.Size(71, 16);
            this.radioreset.TabIndex = 0;
            this.radioreset.TabStop = true;
            this.radioreset.Text = "系统复位";
            this.radioreset.UseVisualStyleBackColor = true;
            this.radioreset.CheckedChanged += new System.EventHandler(this.radioreset_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "密  码：";
            // 
            // textBoxpassword
            // 
            this.textBoxpassword.Location = new System.Drawing.Point(75, 11);
            this.textBoxpassword.Name = "textBoxpassword";
            this.textBoxpassword.PasswordChar = '*';
            this.textBoxpassword.Size = new System.Drawing.Size(178, 21);
            this.textBoxpassword.TabIndex = 4;
            this.textBoxpassword.TextChanged += new System.EventHandler(this.textBoxpassword_TextChanged);
            // 
            // buttonexe
            // 
            this.buttonexe.Location = new System.Drawing.Point(273, 9);
            this.buttonexe.Name = "buttonexe";
            this.buttonexe.Size = new System.Drawing.Size(112, 23);
            this.buttonexe.TabIndex = 5;
            this.buttonexe.Text = "执     行";
            this.buttonexe.UseVisualStyleBackColor = true;
            this.buttonexe.Click += new System.EventHandler(this.buttonexe_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.buttonexe);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxpassword);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(591, 266);
            this.splitContainer1.SplitterDistance = 49;
            this.splitContainer1.TabIndex = 6;
            // 
            // radioclearled
            // 
            this.radioclearled.AutoSize = true;
            this.radioclearled.Location = new System.Drawing.Point(23, 152);
            this.radioclearled.Name = "radioclearled";
            this.radioclearled.Size = new System.Drawing.Size(107, 16);
            this.radioclearled.TabIndex = 3;
            this.radioclearled.TabStop = true;
            this.radioclearled.Text = "故障报警灯复位";
            this.radioclearled.UseVisualStyleBackColor = true;
            this.radioclearled.CheckedChanged += new System.EventHandler(this.radioclearled_CheckedChanged);
            // 
            // ResetCmdViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(591, 266);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ResetCmdViewForm";
            this.Text = "ResetCmdViewForm";
            this.Load += new System.EventHandler(this.ResetCmdViewForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radiocleardata;
        private System.Windows.Forms.RadioButton radioinitparam;
        private System.Windows.Forms.RadioButton radioreset;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxpassword;
        private System.Windows.Forms.Button buttonexe;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RadioButton radioclearled;
    }
}