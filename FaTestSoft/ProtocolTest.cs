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
    public partial class ProtocolTest : Form
    {
        public ProtocolTest()
        {
            InitializeComponent();
        }
        bool test101 = false;
        bool test104 = false;
        int comflag = 0;
        int netflag = 0;

        private void ProtocolTest_Load(object sender, EventArgs e)
        {
            if(PublicDataClass._ProtocoltyFlag.NetProFlag == 1)//默认规约
             radioButton2.Checked = true;
            else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                radioButton1.Checked = true;

            if (PublicDataClass._ProtocoltyFlag.ComProFlag == 1)//默认规约
                 radioButton6.Checked = true;
           else  if (PublicDataClass._ProtocoltyFlag.ComProFlag == 2)//
                radioButton5.Checked = true;

            if (PublicDataClass._ProtocoltyFlag.Test101 == true)
                checkBox1.Checked = true;
            if (PublicDataClass._ProtocoltyFlag.Test104 == true)
                checkBox2.Checked = true;
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
                netflag = 2;
            else
                netflag = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
             if (radioButton2.Checked == true)
                 netflag = 1;
            else
                 netflag = 2;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked == true)
                comflag = 1;
            else
                comflag = 2;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked == true)
                comflag = 2;
            else
                comflag = 1;
            
        }





        private void button1_Click(object sender, EventArgs e)
        {
            PublicDataClass._ProtocoltyFlag.Test104 = test104;
            PublicDataClass._ProtocoltyFlag.Test101 = test101;
            PublicDataClass._ProtocoltyFlag.ComProFlag = comflag;
            PublicDataClass._ProtocoltyFlag.NetProFlag = netflag;
            PublicDataClass._ProtocoltyFlag.DelayTime = Convert.ToInt32(textBoxdelay.Text.Trim());
            PublicDataClass._ProtocoltyFlag.ZZTime = Convert.ToInt32(textBox1.Text.Trim());
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                test101 = true;
            else
                test101 = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
                test104 = true;
            else
                test104 = false;
        }
    }
}
