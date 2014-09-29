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
    public partial class SysParSet : Form
    {
        public SysParSet()
        {
            InitializeComponent();
        }

        private void SysParSet_Load(object sender, EventArgs e)
        {
            int tempdata;
            comboBox1.Items.Clear();
            comboBox1.Items.Add("NULL");
            for (int i = 0; i < 4; i++)
            {
                comboBox1.Items.Add("U" + (i + 1));
            }
            tempdata = Convert.ToInt32(PublicDataClass._SysParam.ValueTable[PublicDataClass._SysParam.selindex]);
            if ((tempdata & 0x0f) <= comboBox1.Items.Count)
            {
                comboBox1.SelectedIndex = (tempdata & 0x0f);
            }
            if ((tempdata & 0x10) == 0x10)
                checkBox1.Checked = true;
            if ((tempdata & 0x20) == 0x20)
                checkBox2.Checked = true;
            if ((tempdata & 0x40) == 0x40)
                checkBox3.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte data1, data2;
            int data3;
            data1 = (byte)(comboBox1.SelectedIndex);
            data2 = 0;
            if (checkBox1.Checked == true)
                data2 += 1;
            if (checkBox2.Checked == true)
                data2 += 2;
            if (checkBox3.Checked == true)
                data2 += 4;
            data3 = ((data2 << 4) + data1);
            PublicDataClass._SysParam.ValueTable[PublicDataClass._SysParam.selindex] = data3.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
            }
            else
            {
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
            }
        }
    }
}
