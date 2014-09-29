namespace FaTestSoft
{
    partial class FuGuiCmdViewForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxpw = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxsl = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxpw);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Cornsilk;
            this.splitContainer1.Panel2.Controls.Add(this.comboBoxsl);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Size = new System.Drawing.Size(580, 262);
            this.splitContainer1.SplitterDistance = 53;
            this.splitContainer1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(295, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "执   行";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxpw
            // 
            this.textBoxpw.Location = new System.Drawing.Point(106, 11);
            this.textBoxpw.Name = "textBoxpw";
            this.textBoxpw.PasswordChar = '*';
            this.textBoxpw.Size = new System.Drawing.Size(164, 21);
            this.textBoxpw.TabIndex = 1;
            this.textBoxpw.TextChanged += new System.EventHandler(this.textBoxpw_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "密   码:";
            // 
            // comboBoxsl
            // 
            this.comboBoxsl.FormattingEnabled = true;
            this.comboBoxsl.Items.AddRange(new object[] {
            "G0__重合闸信号复归",
            "G1__过流信号复归",
            "G2__事故信号复归1",
            "G3__事故信号复归2",
            "G4__事故信号复归3",
            "G5__事故信号复归4",
            "G6__事故信号复归5",
            "G7__事故信号复归6",
            "G8__事故信号复归7",
            "G9__事故信号复归8",
            "G10__事故信号复归9",
            "G11__事故信号复归10",
            "G12__事故信号复归11",
            "G13__事故信号复归12",
            "G14__事故信号复归13",
            "G15__事故信号复归14",
            "G16__事故信号复归15"});
            this.comboBoxsl.Location = new System.Drawing.Point(106, 44);
            this.comboBoxsl.Name = "comboBoxsl";
            this.comboBoxsl.Size = new System.Drawing.Size(164, 20);
            this.comboBoxsl.TabIndex = 4;
            this.comboBoxsl.SelectedIndexChanged += new System.EventHandler(this.comboBoxsl_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "选   项";
            // 
            // FuGuiCmdViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 262);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FuGuiCmdViewForm";
            this.Text = "FuGuiCmdViewForm";
            this.Load += new System.EventHandler(this.FuGuiCmdViewForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxpw;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxsl;
        private System.Windows.Forms.Label label2;
    }
}