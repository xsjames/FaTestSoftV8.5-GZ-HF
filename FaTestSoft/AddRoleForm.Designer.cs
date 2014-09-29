namespace FaTestSoft
{
    partial class AddRoleForm
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
            this.label3 = new System.Windows.Forms.Label();
            this.buttonok = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBoxuser = new System.Windows.Forms.TextBox();
            this.textBoxpassword1 = new System.Windows.Forms.TextBox();
            this.textBoxpassword2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "密  码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "确认密码：";
            // 
            // buttonok
            // 
            this.buttonok.Location = new System.Drawing.Point(23, 179);
            this.buttonok.Name = "buttonok";
            this.buttonok.Size = new System.Drawing.Size(75, 23);
            this.buttonok.TabIndex = 3;
            this.buttonok.Text = "确  定";
            this.buttonok.UseVisualStyleBackColor = true;
            this.buttonok.Click += new System.EventHandler(this.buttonok_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(157, 179);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "取  消";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // textBoxuser
            // 
            this.textBoxuser.Location = new System.Drawing.Point(88, 18);
            this.textBoxuser.Name = "textBoxuser";
            this.textBoxuser.Size = new System.Drawing.Size(144, 21);
            this.textBoxuser.TabIndex = 5;
            // 
            // textBoxpassword1
            // 
            this.textBoxpassword1.Location = new System.Drawing.Point(88, 65);
            this.textBoxpassword1.Name = "textBoxpassword1";
            this.textBoxpassword1.PasswordChar = '*';
            this.textBoxpassword1.Size = new System.Drawing.Size(144, 21);
            this.textBoxpassword1.TabIndex = 6;
            // 
            // textBoxpassword2
            // 
            this.textBoxpassword2.Location = new System.Drawing.Point(88, 117);
            this.textBoxpassword2.Name = "textBoxpassword2";
            this.textBoxpassword2.PasswordChar = '*';
            this.textBoxpassword2.Size = new System.Drawing.Size(144, 21);
            this.textBoxpassword2.TabIndex = 7;
            // 
            // AddRoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ClientSize = new System.Drawing.Size(253, 224);
            this.Controls.Add(this.textBoxpassword2);
            this.Controls.Add(this.textBoxpassword1);
            this.Controls.Add(this.textBoxuser);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buttonok);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddRoleForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddRoleForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonok;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBoxuser;
        private System.Windows.Forms.TextBox textBoxpassword1;
        private System.Windows.Forms.TextBox textBoxpassword2;
    }
}