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
    public partial class CModifyViewForm1 : Form
    {
        public CModifyViewForm1()
        {
            InitializeComponent();
        }

        private void CModifyViewForm1_Load(object sender, EventArgs e)
        {
            for (int ch = 0; ch < PublicDataClass._MyYcDotstruct.epos; ch++)
            {
                domainUpDownb.Items.Add(ch);
                domainUpDowne.Items.Add(ch);
            }
            domainUpDownb.SelectedIndex = PublicDataClass._MyYcDotstruct.bpos;
            domainUpDowne.SelectedIndex = PublicDataClass._MyYcDotstruct.epos - 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PublicDataClass._MyYcDotstruct.bpos = Convert.ToInt16(domainUpDownb.Text);
            PublicDataClass._MyYcDotstruct.epos = Convert.ToInt16(domainUpDowne.Text);


            if (PublicDataClass._MyYcDotstruct.bpos > PublicDataClass._MyYcDotstruct.epos)
            {
                MessageBox.Show("起始序号>截止序号", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            if (textBox1.Text == @"")
                PublicDataClass._MyYcDotstruct.BusNum_notchange = true;
            else
            {
                PublicDataClass._MyYcDotstruct.BusNum_notchange = false;
                PublicDataClass._MyYcDotstruct.BusNum = textBox1.Text;
            }
            if (textBox2.Text == @"")
                PublicDataClass._MyYcDotstruct.CardNum_notchange = true;
            else
            {
                PublicDataClass._MyYcDotstruct.CardNum_notchange = false;
                PublicDataClass._MyYcDotstruct.CardNum = textBox2.Text;
            }

            if (comboBox1.Text == @"不变")
                PublicDataClass._MyYcDotstruct.UBusmode_notchange = true;
            else
            {
                PublicDataClass._MyYcDotstruct.UBusmode_notchange = false;
                PublicDataClass._MyYcDotstruct.UBusmode = comboBox1.Text;
            }

            if (comboBox2.Text == @"不变")
                PublicDataClass._MyYcDotstruct.IBusmode_notchange = true;
            else
            {
                PublicDataClass._MyYcDotstruct.IBusmode_notchange = false;
                PublicDataClass._MyYcDotstruct.IBusmode = comboBox2.Text;
            }           

            this.DialogResult = DialogResult.OK;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键
            {

                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字
                {

                    MessageBox.Show("只能输入数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //listView1.RemoveEmbeddedControl(tbox);
                    e.Handled = true;
                }


            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键
            {

                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字
                {

                    MessageBox.Show("只能输入数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //listView1.RemoveEmbeddedControl(tbox);
                    e.Handled = true;
                }


            }
        }
    }
}
