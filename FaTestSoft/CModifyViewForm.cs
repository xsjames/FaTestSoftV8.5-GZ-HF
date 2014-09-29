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
    public partial class CModifyViewForm : Form
    {
        public CModifyViewForm()
        {
            InitializeComponent();
        }

        private void CModifyViewForm_Load(object sender, EventArgs e)
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
            if (textBox2.Text == @"")
            {
                MessageBox.Show("数值为空", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            PublicDataClass._Mystruct.row = Convert.ToInt16(textBox2.Text);
            PublicDataClass._Mystruct.value = textBox1.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

      
    }
}
