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
    public partial class ModifyAddrForm : Form
    {
        public ModifyAddrForm()
        {
            InitializeComponent();
        }

        private void textBoxaddr_KeyPress(object sender, KeyPressEventArgs e)
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

        private void buttonok_Click(object sender, EventArgs e)
        {
            if (textBoxaddr.Text == "")
            {
                MessageBox.Show("输入的地址数值为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            PublicDataClass.DevAddr = Convert.ToInt32(textBoxaddr.Text);
            this.DialogResult = DialogResult.OK;
        }

        private void ModifyAddrForm_Load(object sender, EventArgs e)
        {
            textBoxaddr.Text = Convert.ToString(PublicDataClass.DevAddr);
        }
    }
}
