using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FaTestSoft.CommonData;

namespace FaTestSoft
{
    public partial class AddYkRecordForm : Form
    {
        public AddYkRecordForm()
        {
            InitializeComponent();
            
        }

        private void buttonok_Click(object sender, EventArgs e)
        {

            if (textBoxname.Text == "")
            {
                MessageBox.Show("遥控名称记录项为空！", "提示",
                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBoxpos.Text == "")
            {
                MessageBox.Show("起始点号记录项为空！", "提示",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Convert.ToInt32(textBoxpos.Text) > 24676 || Convert.ToInt32(textBoxpos.Text) < 24577)
            {
                MessageBox.Show("点号范围有误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            PublicDataClass._AddYkRecord.Name = textBoxname.Text;
            PublicDataClass._AddYkRecord.Pos = textBoxpos.Text;

            this.DialogResult = DialogResult.OK;
        }

        private void AddYkRecordForm_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
        }

        private void textBoxpos_TextChanged(object sender, EventArgs e)
        {
            PublicDataClass._AddYkRecord.Pos = textBoxpos.Text;
            textBox1.Text = String.Format("{0:x}", Convert.ToInt32(PublicDataClass._AddYkRecord.Pos));
        }

        private void textBoxpos_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字
                {
                    
                    MessageBox.Show("只能输入数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Handled = true;
                }
             }

        }

        private void textBoxpos_Leave(object sender, EventArgs e)
        {
            if (textBoxpos.Focused == false)
            {
                if (textBoxpos.Text == "")
                    return;
                if (Convert.ToInt32(textBoxpos.Text) > 24676 || Convert.ToInt32(textBoxpos.Text) < 24577)
                {
                    MessageBox.Show("点号范围有误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }
        }
    }
}
