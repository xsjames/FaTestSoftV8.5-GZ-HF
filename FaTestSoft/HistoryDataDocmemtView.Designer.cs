namespace FaTestSoft
{
    partial class HistoryDataDocmemtView
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxPoint = new System.Windows.Forms.ComboBox();
            this.comboBoxData = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePickerbegindata = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePickerbegintime = new System.Windows.Forms.DateTimePicker();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBoxto = new System.Windows.Forms.TextBox();
            this.textBoxfrom = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonoutput = new System.Windows.Forms.Button();
            this.buttonclear = new System.Windows.Forms.Button();
            this.dUpDown = new System.Windows.Forms.DomainUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.dateTimePickerendtime = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerenddata = new System.Windows.Forms.DateTimePicker();
            this.buttoncall = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "测量点：";
            // 
            // comboBoxPoint
            // 
            this.comboBoxPoint.FormattingEnabled = true;
            this.comboBoxPoint.Location = new System.Drawing.Point(95, 9);
            this.comboBoxPoint.Name = "comboBoxPoint";
            this.comboBoxPoint.Size = new System.Drawing.Size(161, 20);
            this.comboBoxPoint.TabIndex = 1;
            // 
            // comboBoxData
            // 
            this.comboBoxData.FormattingEnabled = true;
            this.comboBoxData.Items.AddRange(new object[] {
            "H0__遥信变位",
            "H1__复位记录",
            "H2__遥控记录",
            "H3__历史记录",
            "H4__统计数据-最大最小值",
            "H5__电压日合格率"});
            this.comboBoxData.Location = new System.Drawing.Point(343, 11);
            this.comboBoxData.Name = "comboBoxData";
            this.comboBoxData.Size = new System.Drawing.Size(121, 20);
            this.comboBoxData.TabIndex = 2;
            this.comboBoxData.SelectedIndexChanged += new System.EventHandler(this.comboBoxData_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(284, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "数据项：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "From：";
            // 
            // dateTimePickerbegindata
            // 
            this.dateTimePickerbegindata.Location = new System.Drawing.Point(95, 35);
            this.dateTimePickerbegindata.Name = "dateTimePickerbegindata";
            this.dateTimePickerbegindata.Size = new System.Drawing.Size(161, 21);
            this.dateTimePickerbegindata.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(49, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "To：";
            // 
            // dateTimePickerbegintime
            // 
            this.dateTimePickerbegintime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerbegintime.Location = new System.Drawing.Point(286, 35);
            this.dateTimePickerbegintime.Name = "dateTimePickerbegintime";
            this.dateTimePickerbegintime.ShowUpDown = true;
            this.dateTimePickerbegintime.Size = new System.Drawing.Size(178, 21);
            this.dateTimePickerbegintime.TabIndex = 7;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(863, 245);
            this.listView1.TabIndex = 8;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "数据项";
            this.columnHeader2.Width = 250;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "数值";
            this.columnHeader3.Width = 150;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "单位";
            this.columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "倍率";
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
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.splitContainer1.Panel1.Controls.Add(this.textBoxto);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxfrom);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.buttonoutput);
            this.splitContainer1.Panel1.Controls.Add(this.buttonclear);
            this.splitContainer1.Panel1.Controls.Add(this.dUpDown);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.dateTimePickerendtime);
            this.splitContainer1.Panel1.Controls.Add(this.dateTimePickerenddata);
            this.splitContainer1.Panel1.Controls.Add(this.buttoncall);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.comboBoxPoint);
            this.splitContainer1.Panel1.Controls.Add(this.comboBoxData);
            this.splitContainer1.Panel1.Controls.Add(this.dateTimePickerbegintime);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.dateTimePickerbegindata);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listView1);
            this.splitContainer1.Size = new System.Drawing.Size(865, 344);
            this.splitContainer1.SplitterDistance = 93;
            this.splitContainer1.TabIndex = 9;
            // 
            // textBoxto
            // 
            this.textBoxto.Location = new System.Drawing.Point(585, 8);
            this.textBoxto.Name = "textBoxto";
            this.textBoxto.Size = new System.Drawing.Size(54, 21);
            this.textBoxto.TabIndex = 18;
            // 
            // textBoxfrom
            // 
            this.textBoxfrom.Location = new System.Drawing.Point(504, 7);
            this.textBoxfrom.Name = "textBoxfrom";
            this.textBoxfrom.Size = new System.Drawing.Size(54, 21);
            this.textBoxfrom.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(562, 14);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "至";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(481, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "从";
            // 
            // buttonoutput
            // 
            this.buttonoutput.Location = new System.Drawing.Point(585, 58);
            this.buttonoutput.Name = "buttonoutput";
            this.buttonoutput.Size = new System.Drawing.Size(94, 23);
            this.buttonoutput.TabIndex = 14;
            this.buttonoutput.Text = "导  出";
            this.buttonoutput.UseVisualStyleBackColor = true;
            this.buttonoutput.Click += new System.EventHandler(this.buttonoutput_Click);
            // 
            // buttonclear
            // 
            this.buttonclear.Location = new System.Drawing.Point(698, 58);
            this.buttonclear.Name = "buttonclear";
            this.buttonclear.Size = new System.Drawing.Size(88, 23);
            this.buttonclear.TabIndex = 13;
            this.buttonclear.Text = "清  空";
            this.buttonclear.UseVisualStyleBackColor = true;
            this.buttonclear.Click += new System.EventHandler(this.buttonclear_Click);
            // 
            // dUpDown
            // 
            this.dUpDown.Location = new System.Drawing.Point(520, 34);
            this.dUpDown.Name = "dUpDown";
            this.dUpDown.Size = new System.Drawing.Size(50, 21);
            this.dUpDown.TabIndex = 12;
            this.dUpDown.Text = "domainUpDown1";
            this.dUpDown.Wrap = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(470, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "间隔(分)";
            // 
            // dateTimePickerendtime
            // 
            this.dateTimePickerendtime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerendtime.Location = new System.Drawing.Point(286, 62);
            this.dateTimePickerendtime.Name = "dateTimePickerendtime";
            this.dateTimePickerendtime.ShowUpDown = true;
            this.dateTimePickerendtime.Size = new System.Drawing.Size(178, 21);
            this.dateTimePickerendtime.TabIndex = 10;
            // 
            // dateTimePickerenddata
            // 
            this.dateTimePickerenddata.Location = new System.Drawing.Point(95, 62);
            this.dateTimePickerenddata.Name = "dateTimePickerenddata";
            this.dateTimePickerenddata.Size = new System.Drawing.Size(161, 21);
            this.dateTimePickerenddata.TabIndex = 9;
            // 
            // buttoncall
            // 
            this.buttoncall.Location = new System.Drawing.Point(472, 58);
            this.buttoncall.Name = "buttoncall";
            this.buttoncall.Size = new System.Drawing.Size(98, 23);
            this.buttoncall.TabIndex = 8;
            this.buttoncall.Text = "召  测";
            this.buttoncall.UseVisualStyleBackColor = true;
            this.buttoncall.Click += new System.EventHandler(this.buttoncall_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // HistoryDataDocmemtView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 344);
            this.Controls.Add(this.splitContainer1);
            this.DockAreas = ((KD.WinFormsUI.Docking.DockAreas)(((((KD.WinFormsUI.Docking.DockAreas.Float | KD.WinFormsUI.Docking.DockAreas.DockLeft)
                        | KD.WinFormsUI.Docking.DockAreas.DockRight)
                        | KD.WinFormsUI.Docking.DockAreas.DockTop)
                        | KD.WinFormsUI.Docking.DockAreas.Document)));
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Name = "HistoryDataDocmemtView";
            this.Text = "历史数据视图";
            this.Deactivate += new System.EventHandler(this.HistoryDataDocmemtView_Deactivate);
            this.Load += new System.EventHandler(this.HistoryDataDocmemtView_Load);
            this.Activated += new System.EventHandler(this.HistoryDataDocmemtView_Activated);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxPoint;
        private System.Windows.Forms.ComboBox comboBoxData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePickerbegindata;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePickerbegintime;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttoncall;
        private System.Windows.Forms.DateTimePicker dateTimePickerendtime;
        private System.Windows.Forms.DateTimePicker dateTimePickerenddata;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DomainUpDown dUpDown;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button buttonclear;
        private System.Windows.Forms.Button buttonoutput;
        private System.Windows.Forms.TextBox textBoxto;
        private System.Windows.Forms.TextBox textBoxfrom;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
    }
}