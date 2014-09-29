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
    public partial class YkqualityConfig : Form
    {
        public YkqualityConfig()
        {
            InitializeComponent();
        }

        private void YkqualityConfig_Load(object sender, EventArgs e)
        {
            int tempdata;
            comboBox1.Items.Clear();
            comboBox1.Items.Add("NULL");
            for(int i=0;i<18;i++)
            {
                comboBox1.Items.Add("线路"+(i+1));
            }
            tempdata = Convert.ToInt32(PublicDataClass._YkConfigParam.AddrTable[PublicDataClass._YkConfigParam.selindex]);
            if(((tempdata& 0xff00) >> 8)<=comboBox1.Items.Count)
            {
                comboBox1.SelectedIndex = ((tempdata & 0xff00) >> 8);
            }
            if ((tempdata & 0x0001) == 0x01)
                checkBox1.Checked = true;
            if ((tempdata & 0x0002) == 0x02)
                checkBox2.Checked = true;
            if ((tempdata & 0x0004) == 0x04)
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
           data3=((data1<<8)+data2);
           PublicDataClass._YkConfigParam.AddrTable[PublicDataClass._YkConfigParam.selindex] = data3.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex==0)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked =false;
                checkBox1.Enabled=false;
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
