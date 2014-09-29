namespace FaTestSoft
{
    partial class CallDataDocmentView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CallDataDocmentView));
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxPoint = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxData = new System.Windows.Forms.ComboBox();
            this.buttonCall = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxselete = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.domainUpDownb = new System.Windows.Forms.DomainUpDown();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.output = new System.Windows.Forms.Button();
            this.buttonclear = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.ListData = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonseach = new System.Windows.Forms.Button();
            this.seachpage = new System.Windows.Forms.TextBox();
            this.nowpage = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.totalpage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.LastPage = new System.Windows.Forms.Button();
            this.NextPage = new System.Windows.Forms.Button();
            this.PreviousPage = new System.Windows.Forms.Button();
            this.FistPage = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ListData)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "测量点:";
            // 
            // comboBoxPoint
            // 
            this.comboBoxPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxPoint.FormattingEnabled = true;
            this.comboBoxPoint.Location = new System.Drawing.Point(46, 11);
            this.comboBoxPoint.Name = "comboBoxPoint";
            this.comboBoxPoint.Size = new System.Drawing.Size(145, 24);
            this.comboBoxPoint.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(197, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "数据项:";
            // 
            // comboBoxData
            // 
            this.comboBoxData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxData.FormattingEnabled = true;
            this.comboBoxData.Items.AddRange(new object[] {
            "F0__遥测",
            "F1__遥信",
            "F2__版本号",
            "F3__时间",
            "F4__器件状态"});
            this.comboBoxData.Location = new System.Drawing.Point(250, 11);
            this.comboBoxData.Name = "comboBoxData";
            this.comboBoxData.Size = new System.Drawing.Size(176, 24);
            this.comboBoxData.TabIndex = 4;
            this.comboBoxData.SelectedIndexChanged += new System.EventHandler(this.comboBoxData_SelectedIndexChanged);
            // 
            // buttonCall
            // 
            this.buttonCall.Location = new System.Drawing.Point(568, 16);
            this.buttonCall.Name = "buttonCall";
            this.buttonCall.Size = new System.Drawing.Size(75, 23);
            this.buttonCall.TabIndex = 5;
            this.buttonCall.Text = "召  测";
            this.buttonCall.UseVisualStyleBackColor = true;
            this.buttonCall.Click += new System.EventHandler(this.buttonCall_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
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
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.comboBoxselete);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.domainUpDownb);
            this.splitContainer1.Panel1.Controls.Add(this.checkBox1);
            this.splitContainer1.Panel1.Controls.Add(this.output);
            this.splitContainer1.Panel1.Controls.Add(this.buttonclear);
            this.splitContainer1.Panel1.Controls.Add(this.comboBoxPoint);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.buttonCall);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.comboBoxData);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(828, 459);
            this.splitContainer1.SplitterDistance = 66;
            this.splitContainer1.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(222, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 16;
            this.label6.Text = "显示方式选择";
            // 
            // comboBoxselete
            // 
            this.comboBoxselete.FormattingEnabled = true;
            this.comboBoxselete.Items.AddRange(new object[] {
            "S0_按线路",
            "S1_按序号"});
            this.comboBoxselete.Location = new System.Drawing.Point(305, 38);
            this.comboBoxselete.Name = "comboBoxselete";
            this.comboBoxselete.Size = new System.Drawing.Size(121, 20);
            this.comboBoxselete.TabIndex = 15;
            this.comboBoxselete.SelectedIndexChanged += new System.EventHandler(this.comboBoxselete_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(432, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "召测间隔(s)";
            // 
            // domainUpDownb
            // 
            this.domainUpDownb.Location = new System.Drawing.Point(509, 37);
            this.domainUpDownb.Name = "domainUpDownb";
            this.domainUpDownb.Size = new System.Drawing.Size(37, 21);
            this.domainUpDownb.TabIndex = 12;
            this.domainUpDownb.Text = "5";
            this.domainUpDownb.Wrap = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(443, 12);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "循环召测";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // output
            // 
            this.output.Location = new System.Drawing.Point(649, 16);
            this.output.Name = "output";
            this.output.Size = new System.Drawing.Size(75, 23);
            this.output.TabIndex = 7;
            this.output.Text = "导  出";
            this.output.UseVisualStyleBackColor = true;
            this.output.Click += new System.EventHandler(this.output_Click);
            // 
            // buttonclear
            // 
            this.buttonclear.Location = new System.Drawing.Point(730, 16);
            this.buttonclear.Name = "buttonclear";
            this.buttonclear.Size = new System.Drawing.Size(75, 23);
            this.buttonclear.TabIndex = 6;
            this.buttonclear.Text = "清  空";
            this.buttonclear.UseVisualStyleBackColor = true;
            this.buttonclear.Click += new System.EventHandler(this.buttonclear_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.ListData);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.Color.Beige;
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Size = new System.Drawing.Size(828, 389);
            this.splitContainer2.SplitterDistance = 645;
            this.splitContainer2.TabIndex = 1;
            // 
            // ListData
            // 
            this.ListData.AllowUserToAddRows = false;
            this.ListData.BackgroundColor = System.Drawing.Color.Cornsilk;
            this.ListData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ListData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Cornsilk;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ListData.DefaultCellStyle = dataGridViewCellStyle2;
            this.ListData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListData.Location = new System.Drawing.Point(0, 0);
            this.ListData.Name = "ListData";
            this.ListData.RowTemplate.Height = 23;
            this.ListData.Size = new System.Drawing.Size(643, 387);
            this.ListData.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "序号";
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.Width = 60;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "名称";
            this.Column2.Name = "Column2";
            this.Column2.Width = 150;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "数值";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "单位";
            this.Column4.Name = "Column4";
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "倍率";
            this.Column5.Name = "Column5";
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.buttonseach);
            this.groupBox1.Controls.Add(this.seachpage);
            this.groupBox1.Controls.Add(this.nowpage);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.totalpage);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.LastPage);
            this.groupBox1.Controls.Add(this.NextPage);
            this.groupBox1.Controls.Add(this.PreviousPage);
            this.groupBox1.Controls.Add(this.FistPage);
            this.groupBox1.Location = new System.Drawing.Point(0, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(176, 380);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作类型";
            // 
            // buttonseach
            // 
            this.buttonseach.Image = ((System.Drawing.Image)(resources.GetObject("buttonseach.Image")));
            this.buttonseach.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonseach.Location = new System.Drawing.Point(79, 80);
            this.buttonseach.Name = "buttonseach";
            this.buttonseach.Size = new System.Drawing.Size(75, 23);
            this.buttonseach.TabIndex = 10;
            this.buttonseach.Text = " 查询页";
            this.buttonseach.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonseach.UseVisualStyleBackColor = true;
            this.buttonseach.Click += new System.EventHandler(this.buttonseach_Click);
            // 
            // seachpage
            // 
            this.seachpage.Location = new System.Drawing.Point(8, 80);
            this.seachpage.Name = "seachpage";
            this.seachpage.Size = new System.Drawing.Size(70, 21);
            this.seachpage.TabIndex = 9;
            this.seachpage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.seachpage_KeyPress);
            // 
            // nowpage
            // 
            this.nowpage.Enabled = false;
            this.nowpage.Location = new System.Drawing.Point(55, 52);
            this.nowpage.Name = "nowpage";
            this.nowpage.Size = new System.Drawing.Size(99, 21);
            this.nowpage.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 21);
            this.label4.TabIndex = 6;
            this.label4.Text = "当前页：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // totalpage
            // 
            this.totalpage.AcceptsTab = true;
            this.totalpage.Enabled = false;
            this.totalpage.Location = new System.Drawing.Point(55, 26);
            this.totalpage.Name = "totalpage";
            this.totalpage.Size = new System.Drawing.Size(99, 21);
            this.totalpage.TabIndex = 5;
            this.totalpage.Text = "1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "总页数：";
            // 
            // LastPage
            // 
            this.LastPage.Location = new System.Drawing.Point(30, 349);
            this.LastPage.Name = "LastPage";
            this.LastPage.Size = new System.Drawing.Size(107, 23);
            this.LastPage.TabIndex = 3;
            this.LastPage.Text = "尾  页";
            this.LastPage.UseVisualStyleBackColor = true;
            this.LastPage.Click += new System.EventHandler(this.LastPage_Click);
            // 
            // NextPage
            // 
            this.NextPage.Location = new System.Drawing.Point(30, 282);
            this.NextPage.Name = "NextPage";
            this.NextPage.Size = new System.Drawing.Size(107, 23);
            this.NextPage.TabIndex = 2;
            this.NextPage.Text = "下一页";
            this.NextPage.UseVisualStyleBackColor = true;
            this.NextPage.Click += new System.EventHandler(this.NextPage_Click);
            // 
            // PreviousPage
            // 
            this.PreviousPage.Location = new System.Drawing.Point(30, 214);
            this.PreviousPage.Name = "PreviousPage";
            this.PreviousPage.Size = new System.Drawing.Size(107, 23);
            this.PreviousPage.TabIndex = 1;
            this.PreviousPage.Text = "上一页";
            this.PreviousPage.UseVisualStyleBackColor = true;
            this.PreviousPage.Click += new System.EventHandler(this.PreviousPage_Click);
            // 
            // FistPage
            // 
            this.FistPage.Location = new System.Drawing.Point(30, 138);
            this.FistPage.Name = "FistPage";
            this.FistPage.Size = new System.Drawing.Size(107, 23);
            this.FistPage.TabIndex = 0;
            this.FistPage.Text = "首  页";
            this.FistPage.UseVisualStyleBackColor = true;
            this.FistPage.Click += new System.EventHandler(this.FistPage_Click);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // CallDataDocmentView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ClientSize = new System.Drawing.Size(828, 459);
            this.Controls.Add(this.splitContainer1);
            this.DockAreas = ((KD.WinFormsUI.Docking.DockAreas)(((((KD.WinFormsUI.Docking.DockAreas.Float | KD.WinFormsUI.Docking.DockAreas.DockLeft)
                        | KD.WinFormsUI.Docking.DockAreas.DockRight)
                        | KD.WinFormsUI.Docking.DockAreas.DockTop)
                        | KD.WinFormsUI.Docking.DockAreas.Document)));
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Name = "CallDataDocmentView";
            this.Text = "召测视图";
            this.Deactivate += new System.EventHandler(this.CallDataDocmentView_Deactivate);
            this.Load += new System.EventHandler(this.CallDataDocmentView_Load);
            this.SizeChanged += new System.EventHandler(this.CallDataDocmentView_SizeChanged);
            this.Activated += new System.EventHandler(this.CallDataDocmentView_Activated);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ListData)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxPoint;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxData;
        private System.Windows.Forms.Button buttonCall;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonclear;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button LastPage;
        private System.Windows.Forms.Button NextPage;
        private System.Windows.Forms.Button PreviousPage;
        private System.Windows.Forms.Button FistPage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox totalpage;
        private System.Windows.Forms.DataGridView ListData;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox nowpage;
        private System.Windows.Forms.Button buttonseach;
        private System.Windows.Forms.TextBox seachpage;
        private System.Windows.Forms.Button output;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DomainUpDown domainUpDownb;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ComboBox comboBoxselete;
        private System.Windows.Forms.Label label6;
    }
}