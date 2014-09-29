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
    public partial class AddInfoAddrViewForm : Form
    {
        public AddInfoAddrViewForm()
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
                MessageBox.Show("起始点号记录项为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (PublicDataClass.Menu == 1)
            {
                if (Convert.ToInt32(textBoxpos.Text) > 16385 || Convert.ToInt32(textBoxpos.Text) < 20479)
                {
                    MessageBox.Show("点号范围有误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }
            else if (PublicDataClass.Menu == 2)
            {
                if (Convert.ToInt32(textBoxpos.Text) <=0 || Convert.ToInt32(textBoxpos.Text) > 1000)
                {
                    MessageBox.Show("点号范围有误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                    return;
                }

            }
            else if (PublicDataClass.Menu == 3)
            {
                if (Convert.ToInt32(textBoxpos.Text) > 49153 || Convert.ToInt32(textBoxpos.Text) < 53247)
                {
                    MessageBox.Show("点号范围有误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                    return;
                }

            }

            PublicDataClass._AddInfoRecord.Name = textBoxname.Text;
            PublicDataClass._AddInfoRecord.Pos  = textBoxpos.Text;// "0x" + textBox3.Text;

            this.DialogResult = DialogResult.OK;
        }

        private void AddInfoAddrViewForm_Load(object sender, EventArgs e)
        {
            if (PublicDataClass.Menu == 1)
            {
                label4.Text = "(范围:16385--20479)";

            }
            else if (PublicDataClass.Menu == 2)
            {
                label4.Text = "(范围:1--1000)";

            }
            else if (PublicDataClass.Menu == 3)
            {
                label4.Text = "(范围:49153--53247)";

            }
            textBox3.Enabled = false;
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

        private void textBoxpos_TextChanged(object sender, EventArgs e)
        {
            //PublicDataClass._AddInfoRecord.Pos = textBoxpos.Text;
            textBox3.Text = String.Format("{0:x}", Convert.ToInt32(textBoxpos.Text));
            
        }

        private void textBoxpos_Leave(object sender, EventArgs e)
        {
            if (textBoxpos.Focused == false)
            {
                if (textBoxpos.Text == "")
                    return;
                if (PublicDataClass.Menu == 1)
                {
                    if (Convert.ToInt32(textBoxpos.Text) <16385 || Convert.ToInt32(textBoxpos.Text) > 20479)
                    {
                        MessageBox.Show("点号范围有误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.DialogResult = DialogResult.None;
                        return;
                    }
                }
                else if (PublicDataClass.Menu == 2)
                {
                    if (Convert.ToInt32(textBoxpos.Text) <= 0 || Convert.ToInt32(textBoxpos.Text) >1000)
                    {
                        MessageBox.Show("点号范围有误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.DialogResult = DialogResult.None;
                        return;
                    }

                }
                else if (PublicDataClass.Menu == 3)
                {
                    if (Convert.ToInt32(textBoxpos.Text)< 49153 || Convert.ToInt32(textBoxpos.Text) > 53247)
                    {
                        MessageBox.Show("点号范围有误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.DialogResult = DialogResult.None;
                        return;
                    }

                }
            }
        }
        
    }
}
