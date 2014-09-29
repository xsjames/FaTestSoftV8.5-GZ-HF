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
    public partial class CModifyViewForm2 : Form
    {
        public CModifyViewForm2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PublicDataClass._Mystruct.bpos = Convert.ToInt16(domainUpDownb.Text);
            PublicDataClass._Mystruct.epos = Convert.ToInt16(domainUpDowne.Text);


            if (PublicDataClass._Mystruct.bpos > PublicDataClass._Mystruct.epos)
            {
                MessageBox.Show("起始序号>截止序号", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            if (textBox1.Text == @"")
            {
                MessageBox.Show("数值为空", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            PublicDataClass._Mystruct.row = Convert.ToInt16(textBox2.Text);
            PublicDataClass._Mystruct.value = textBox1.Text;
            if (checkBox1.Checked == true)
                PublicDataClass._Mystruct.bl = true;
            else
                PublicDataClass._Mystruct.bl = false;


            this.DialogResult = DialogResult.OK;
            

        }

        private void CModifyViewForm2_Load(object sender, EventArgs e)
        {
            for (int ch = 0; ch < PublicDataClass._Mystruct.epos; ch++)
            {
                domainUpDownb.Items.Add(ch);
                domainUpDowne.Items.Add(ch);
            }
            domainUpDownb.SelectedIndex = PublicDataClass._Mystruct.bpos;
            domainUpDowne.SelectedIndex = PublicDataClass._Mystruct.epos - 1;

            textBox1.Text = "0";
            textBox2.Text = "0";
            checkBox1.Checked = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox2.Checked = false;
                //PublicDataClass._MyYcInformationstruct.bl = true;
            }
            else
            {
                checkBox2.Checked = true;
                //PublicDataClass._MyYcInformationstruct.bl = false;
            }
               
    
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox1.Checked = false;
                //PublicDataClass._MyYcInformationstruct.bl = false;
            }
            else
            {
                checkBox1.Checked = true;
                //PublicDataClass._MyYcInformationstruct.bl = true;
            }
               
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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

      
    }
}
