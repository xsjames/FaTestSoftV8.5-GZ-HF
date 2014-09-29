namespace FaTestSoft
{
    partial class AddYkRecordForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxname = new System.Windows.Forms.TextBox();
            this.textBoxpos = new System.Windows.Forms.TextBox();
            this.buttonok = new System.Windows.Forms.Button();
            this.buttoncel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "遥控名称：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "起始点号：";
            // 
            // textBoxname
            // 
            this.textBoxname.Location = new System.Drawing.Point(83, 24);
            this.textBoxname.Name = "textBoxname";
            this.textBoxname.Size = new System.Drawing.Size(150, 21);
            this.textBoxname.TabIndex = 2;
            // 
            // textBoxpos
            // 
            this.textBoxpos.Location = new System.Drawing.Point(83, 74);
            this.textBoxpos.Name = "textBoxpos";
            this.textBoxpos.Size = new System.Drawing.Size(150, 21);
            this.textBoxpos.TabIndex = 3;
            this.textBoxpos.TextChanged += new System.EventHandler(this.textBoxpos_TextChanged);
            this.textBoxpos.Leave += new System.EventHandler(this.textBoxpos_Leave);
            this.textBoxpos.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxpos_KeyPress);
            // 
            // buttonok
            // 
            this.buttonok.Location = new System.Drawing.Point(23, 188);
            this.buttonok.Name = "buttonok";
            this.buttonok.Size = new System.Drawing.Size(75, 23);
            this.buttonok.TabIndex = 4;
            this.buttonok.Text = "确  定";
            this.buttonok.UseVisualStyleBackColor = true;
            this.buttonok.Click += new System.EventHandler(this.buttonok_Click);
            // 
            // buttoncel
            // 
            this.buttoncel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttoncel.Location = new System.Drawing.Point(158, 188);
            this.buttoncel.Name = "buttoncel";
            this.buttoncel.Size = new System.Drawing.Size(75, 23);
            this.buttoncel.TabIndex = 5;
            this.buttoncel.Text = "取  消";
            this.buttoncel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "原    码：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(83, 138);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(150, 21);
            this.textBox1.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(83, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "(范围:24577--24676)";
            // 
            // AddYkRecordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ClientSize = new System.Drawing.Size(251, 227);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttoncel);
            this.Controls.Add(this.buttonok);
            this.Controls.Add(this.textBoxpos);
            this.Controls.Add(this.textBoxname);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddYkRecordForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "增加记录";
            this.Load += new System.EventHandler(this.AddYkRecordForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxname;
        private System.Windows.Forms.TextBox textBoxpos;
        private System.Windows.Forms.Button buttonok;
        private System.Windows.Forms.Button buttoncel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
    }
}