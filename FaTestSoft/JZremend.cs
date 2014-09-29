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
    public partial class JZremend : Form
    {
        public JZremend()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                PublicDataClass._AIParam.remendquality = comboBox1.Text;
                PublicDataClass._AIParam.remendvalue = Convert.ToSingle(textBox1.Text);
                if (radioButton1.Checked == true)
                {
                    PublicDataClass._AIParam.updown = 1;
                }
                else if (radioButton2.Checked == true)
                {
                    PublicDataClass._AIParam.updown = 2;
                }

                if (radioButton3.Checked == true)
                {
                    PublicDataClass._AIParam.remendtype = 1;
                }
                else if (radioButton4.Checked == true)
                {
                    PublicDataClass._AIParam.remendtype = 2;
                }
                this.DialogResult = DialogResult.OK;
            }
            catch
            {
                MessageBox.Show("参数输入有误！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void JZremend_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

    }
}
