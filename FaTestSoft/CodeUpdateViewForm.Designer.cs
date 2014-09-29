namespace FaTestSoft
{
    partial class CodeUpdateViewForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.filename = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxloadstate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.labelmsg = new System.Windows.Forms.Label();
            this.buttonstop = new System.Windows.Forms.Button();
            this.textBoxendseg = new System.Windows.Forms.TextBox();
            this.buttonstart = new System.Windows.Forms.Button();
            this.textBoxstartseg = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboBoxaddr = new System.Windows.Forms.ComboBox();
            this.buttonupdata = new System.Windows.Forms.Button();
            this.buttonjy = new System.Windows.Forms.Button();
            this.buttonselfile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Beige;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.checkBox1);
            this.splitContainer1.Panel2.Controls.Add(this.comboBoxaddr);
            this.splitContainer1.Panel2.Controls.Add(this.buttonupdata);
            this.splitContainer1.Panel2.Controls.Add(this.buttonjy);
            this.splitContainer1.Panel2.Controls.Add(this.buttonselfile);
            this.splitContainer1.Size = new System.Drawing.Size(774, 434);
            this.splitContainer1.SplitterDistance = 580;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Size = new System.Drawing.Size(580, 434);
            this.splitContainer2.SplitterDistance = 160;
            this.splitContainer2.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.filename);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(572, 152);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文件信息";
            // 
            // filename
            // 
            this.filename.AutoSize = true;
            this.filename.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.filename.Location = new System.Drawing.Point(29, 20);
            this.filename.Name = "filename";
            this.filename.Size = new System.Drawing.Size(46, 13);
            this.filename.TabIndex = 0;
            this.filename.Text = "无信息";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.textBoxloadstate);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.progressBar1);
            this.groupBox2.Controls.Add(this.labelmsg);
            this.groupBox2.Controls.Add(this.buttonstop);
            this.groupBox2.Controls.Add(this.textBoxendseg);
            this.groupBox2.Controls.Add(this.buttonstart);
            this.groupBox2.Controls.Add(this.textBoxstartseg);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(2, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(573, 262);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选择信息";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(269, 104);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "完成";
            // 
            // textBoxloadstate
            // 
            this.textBoxloadstate.Location = new System.Drawing.Point(100, 101);
            this.textBoxloadstate.Name = "textBoxloadstate";
            this.textBoxloadstate.Size = new System.Drawing.Size(163, 21);
            this.textBoxloadstate.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "下载状态：";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(33, 155);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(534, 23);
            this.progressBar1.TabIndex = 6;
            // 
            // labelmsg
            // 
            this.labelmsg.AutoSize = true;
            this.labelmsg.Location = new System.Drawing.Point(31, 134);
            this.labelmsg.Name = "labelmsg";
            this.labelmsg.Size = new System.Drawing.Size(65, 12);
            this.labelmsg.TabIndex = 3;
            this.labelmsg.Text = "下载状态..";
            this.labelmsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonstop
            // 
            this.buttonstop.Location = new System.Drawing.Point(395, 62);
            this.buttonstop.Name = "buttonstop";
            this.buttonstop.Size = new System.Drawing.Size(75, 23);
            this.buttonstop.TabIndex = 5;
            this.buttonstop.Text = "中断下载";
            this.buttonstop.UseVisualStyleBackColor = true;
            this.buttonstop.Click += new System.EventHandler(this.buttonstop_Click);
            // 
            // textBoxendseg
            // 
            this.textBoxendseg.Location = new System.Drawing.Point(100, 62);
            this.textBoxendseg.Name = "textBoxendseg";
            this.textBoxendseg.Size = new System.Drawing.Size(163, 21);
            this.textBoxendseg.TabIndex = 3;
            this.textBoxendseg.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxendseg_KeyPress);
            // 
            // buttonstart
            // 
            this.buttonstart.Location = new System.Drawing.Point(395, 20);
            this.buttonstart.Name = "buttonstart";
            this.buttonstart.Size = new System.Drawing.Size(75, 23);
            this.buttonstart.TabIndex = 4;
            this.buttonstart.Text = "开始下载";
            this.buttonstart.UseVisualStyleBackColor = true;
            this.buttonstart.Click += new System.EventHandler(this.buttonstart_Click);
            // 
            // textBoxstartseg
            // 
            this.textBoxstartseg.Location = new System.Drawing.Point(100, 22);
            this.textBoxstartseg.Name = "textBoxstartseg";
            this.textBoxstartseg.Size = new System.Drawing.Size(163, 21);
            this.textBoxstartseg.TabIndex = 2;
            this.textBoxstartseg.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxstartseg_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "结束号段：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "起始段号：";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(26, 23);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "地址选择";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // comboBoxaddr
            // 
            this.comboBoxaddr.Enabled = false;
            this.comboBoxaddr.FormattingEnabled = true;
            this.comboBoxaddr.Location = new System.Drawing.Point(26, 53);
            this.comboBoxaddr.Name = "comboBoxaddr";
            this.comboBoxaddr.Size = new System.Drawing.Size(104, 20);
            this.comboBoxaddr.TabIndex = 3;
            this.comboBoxaddr.SelectedIndexChanged += new System.EventHandler(this.comboBoxaddr_SelectedIndexChanged);
            // 
            // buttonupdata
            // 
            this.buttonupdata.Location = new System.Drawing.Point(26, 187);
            this.buttonupdata.Name = "buttonupdata";
            this.buttonupdata.Size = new System.Drawing.Size(104, 23);
            this.buttonupdata.TabIndex = 2;
            this.buttonupdata.Text = "升    级";
            this.buttonupdata.UseVisualStyleBackColor = true;
            this.buttonupdata.Click += new System.EventHandler(this.buttonupdata_Click);
            // 
            // buttonjy
            // 
            this.buttonjy.Location = new System.Drawing.Point(26, 141);
            this.buttonjy.Name = "buttonjy";
            this.buttonjy.Size = new System.Drawing.Size(104, 23);
            this.buttonjy.TabIndex = 1;
            this.buttonjy.Text = "校    验";
            this.buttonjy.UseVisualStyleBackColor = true;
            this.buttonjy.Click += new System.EventHandler(this.buttonjy_Click);
            // 
            // buttonselfile
            // 
            this.buttonselfile.Location = new System.Drawing.Point(26, 95);
            this.buttonselfile.Name = "buttonselfile";
            this.buttonselfile.Size = new System.Drawing.Size(104, 23);
            this.buttonselfile.TabIndex = 0;
            this.buttonselfile.Text = "选择文件";
            this.buttonselfile.UseVisualStyleBackColor = true;
            this.buttonselfile.Click += new System.EventHandler(this.buttonselfile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-1, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 12);
            this.label1.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Interval = 1;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // CodeUpdateViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ClientSize = new System.Drawing.Size(774, 434);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CodeUpdateViewForm";
            this.Text = "CodeUpdateViewForm";
            this.Deactivate += new System.EventHandler(this.CodeUpdateViewForm_Deactivate);
            this.Load += new System.EventHandler(this.CodeUpdateViewForm_Load);
            this.SizeChanged += new System.EventHandler(this.CodeUpdateViewForm_SizeChanged);
            this.Activated += new System.EventHandler(this.CodeUpdateViewForm_Activated);
            this.VisibleChanged += new System.EventHandler(this.CodeUpdateViewForm_VisibleChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label labelmsg;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonupdata;
        private System.Windows.Forms.Button buttonjy;
        private System.Windows.Forms.Button buttonselfile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonstop;
        private System.Windows.Forms.Button buttonstart;
        private System.Windows.Forms.TextBox textBoxendseg;
        private System.Windows.Forms.TextBox textBoxstartseg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label filename;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxloadstate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox comboBoxaddr;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;

    }
}