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
    public partial class JZfrm : Form
    {
        public JZfrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                float vaule1, vaule2;
                int checksum = 0;
                PublicDataClass._ComStructData.TxLen = 0;
                PublicDataClass._ComStructData.TXBuffer[0] = 0x68;  //
                PublicDataClass._ComStructData.TXBuffer[1] = 0x12;  //
                PublicDataClass._ComStructData.TXBuffer[2] = 0x00;  // 
                PublicDataClass._ComStructData.TXBuffer[3] = 0xFF;  //
                PublicDataClass._ComStructData.TXBuffer[4] = 0xFF;  //
                PublicDataClass._ComStructData.TXBuffer[5] = 0xD8;  // 
                PublicDataClass._ComStructData.TXBuffer[6] = 0x07;  // 
                PublicDataClass._ComStructData.TXBuffer[7] = 0x8E;  // 

                vaule1 = Convert.ToSingle(textBox1.Text);
                vaule2 = Convert.ToSingle(textBox4.Text);
                byte[] b1 = BitConverter.GetBytes(vaule1);
                byte[] b2 = BitConverter.GetBytes(vaule2);

                PublicDataClass._ComStructData.TXBuffer[8] = b1[0];
                PublicDataClass._ComStructData.TXBuffer[9] = b1[1];
                PublicDataClass._ComStructData.TXBuffer[10] = b1[2];
                PublicDataClass._ComStructData.TXBuffer[11] = b1[3];
                PublicDataClass._ComStructData.TXBuffer[12] = b2[0];
                PublicDataClass._ComStructData.TXBuffer[13] = b2[1];
                PublicDataClass._ComStructData.TXBuffer[14] = b2[2];
                PublicDataClass._ComStructData.TXBuffer[15] = b2[3];
                for (int i = 0; i < 16; i++)
                {
                    checksum += PublicDataClass._ComStructData.TXBuffer[i];
                }
                PublicDataClass._ComStructData.TXBuffer[16] = (byte)((checksum) & 0x00ff);
                PublicDataClass._ComStructData.TXBuffer[17] = (byte)(((checksum) & 0xff00) >> 8);

                PublicDataClass._ComStructData.TxLen = 18;
                PublicDataClass.ComFrameMsg = "直流量测试第三步";
                PublicDataClass._ComStructData.TX_TASK = true;
            }
            catch
            {
                MessageBox.Show("数值不能为空！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
            float vaule1, vaule2;
            int checksum=0;
            PublicDataClass._ComStructData.TxLen = 0;
            PublicDataClass._ComStructData.TXBuffer[0] = 0x68;  //
            PublicDataClass._ComStructData.TXBuffer[1] = 0x12;  //
            PublicDataClass._ComStructData.TXBuffer[2] = 0x00;  // 
            PublicDataClass._ComStructData.TXBuffer[3] = 0xFF;  //
            PublicDataClass._ComStructData.TXBuffer[4] = 0xFF;  //
            PublicDataClass._ComStructData.TXBuffer[5] = 0xD8;  // 
            PublicDataClass._ComStructData.TXBuffer[6] = 0x07;  // 
            PublicDataClass._ComStructData.TXBuffer[7] = 0x8F;  // 

            vaule1 = Convert.ToSingle(textBox2.Text);
            vaule2 = Convert.ToSingle(textBox3.Text);
            byte[] b1 = BitConverter.GetBytes(vaule1);
            byte[] b2 = BitConverter.GetBytes(vaule2);

            PublicDataClass._ComStructData.TXBuffer[8] = b1[0];
            PublicDataClass._ComStructData.TXBuffer[9] = b1[1];
            PublicDataClass._ComStructData.TXBuffer[10] = b1[2];
            PublicDataClass._ComStructData.TXBuffer[11] = b1[3];
            PublicDataClass._ComStructData.TXBuffer[12] = b2[0];
            PublicDataClass._ComStructData.TXBuffer[13] = b2[1];
            PublicDataClass._ComStructData.TXBuffer[14] = b2[2];
            PublicDataClass._ComStructData.TXBuffer[15] = b2[3];
            for (int i = 0; i < 16; i++)
            {
                checksum += PublicDataClass._ComStructData.TXBuffer[i];
            }
            PublicDataClass._ComStructData.TXBuffer[16] = (byte)((checksum) & 0x00ff);
            PublicDataClass._ComStructData.TXBuffer[17] = (byte)(((checksum) & 0xff00) >> 8);

            PublicDataClass._ComStructData.TxLen = 18;
            PublicDataClass.ComFrameMsg = "直流量测试第四步";
            PublicDataClass._ComStructData.TX_TASK = true;
            }
            catch
            {
                MessageBox.Show("数值不能为空！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void JZfrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            PublicDataClass.JZPARAM.vaule13 = textBox1.Text;
            PublicDataClass.JZPARAM.vaule14 = textBox2.Text;
            PublicDataClass.JZPARAM.vaule15 = textBox3.Text;
            PublicDataClass.JZPARAM.vaule16 = textBox4.Text;

            PublicDataClass.JZPARAM.vaule17 = textBox5.Text;
            PublicDataClass.JZPARAM.vaule18= textBox6.Text;
            PublicDataClass.JZPARAM.vaule19 = textBox7.Text;
            PublicDataClass.JZPARAM.vaule20 = textBox8.Text;
        }

        private void JZfrm_Load(object sender, EventArgs e)
        {
            textBox1.Text = PublicDataClass.JZPARAM.vaule13;
            textBox2.Text = PublicDataClass.JZPARAM.vaule14;
            textBox3.Text = PublicDataClass.JZPARAM.vaule15;
            textBox4.Text = PublicDataClass.JZPARAM.vaule16;

            textBox5.Text = PublicDataClass.JZPARAM.vaule17;
            textBox6.Text = PublicDataClass.JZPARAM.vaule18;
            textBox7.Text = PublicDataClass.JZPARAM.vaule19;
            textBox8.Text = PublicDataClass.JZPARAM.vaule20;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                float vaule1, vaule2;
                int checksum = 0;
                PublicDataClass._ComStructData.TxLen = 0;
                PublicDataClass._ComStructData.TXBuffer[0] = 0x68;  //
                PublicDataClass._ComStructData.TXBuffer[1] = 0x12;  //
                PublicDataClass._ComStructData.TXBuffer[2] = 0x00;  // 
                PublicDataClass._ComStructData.TXBuffer[3] = 0xFF;  //
                PublicDataClass._ComStructData.TXBuffer[4] = 0xFF;  //
                PublicDataClass._ComStructData.TXBuffer[5] = 0xD8;  // 
                PublicDataClass._ComStructData.TXBuffer[6] = 0x07;  // 
                PublicDataClass._ComStructData.TXBuffer[7] = 0x8C;  // 

                vaule1 = Convert.ToSingle(textBox6.Text);
                vaule2 = Convert.ToSingle(textBox5.Text);
                byte[] b1 = BitConverter.GetBytes(vaule1);
                byte[] b2 = BitConverter.GetBytes(vaule2);

                PublicDataClass._ComStructData.TXBuffer[8] = b1[0];
                PublicDataClass._ComStructData.TXBuffer[9] = b1[1];
                PublicDataClass._ComStructData.TXBuffer[10] = b1[2];
                PublicDataClass._ComStructData.TXBuffer[11] = b1[3];
                PublicDataClass._ComStructData.TXBuffer[12] = b2[0];
                PublicDataClass._ComStructData.TXBuffer[13] = b2[1];
                PublicDataClass._ComStructData.TXBuffer[14] = b2[2];
                PublicDataClass._ComStructData.TXBuffer[15] = b2[3];
                for (int i = 0; i < 16; i++)
                {
                    checksum += PublicDataClass._ComStructData.TXBuffer[i];
                }
                PublicDataClass._ComStructData.TXBuffer[16] = (byte)((checksum) & 0x00ff);
                PublicDataClass._ComStructData.TXBuffer[17] = (byte)(((checksum) & 0xff00) >> 8);

                PublicDataClass._ComStructData.TxLen = 18;
                PublicDataClass.ComFrameMsg = "直流量测试第一步";
                PublicDataClass._ComStructData.TX_TASK = true;
            }
            catch
            {
                MessageBox.Show("数值不能为空！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                float vaule1, vaule2;
                int checksum = 0;
                PublicDataClass._ComStructData.TxLen = 0;
                PublicDataClass._ComStructData.TXBuffer[0] = 0x68;  //
                PublicDataClass._ComStructData.TXBuffer[1] = 0x12;  //
                PublicDataClass._ComStructData.TXBuffer[2] = 0x00;  // 
                PublicDataClass._ComStructData.TXBuffer[3] = 0xFF;  //
                PublicDataClass._ComStructData.TXBuffer[4] = 0xFF;  //
                PublicDataClass._ComStructData.TXBuffer[5] = 0xD8;  // 
                PublicDataClass._ComStructData.TXBuffer[6] = 0x07;  // 
                PublicDataClass._ComStructData.TXBuffer[7] = 0x8D;  // 

                vaule1 = Convert.ToSingle(textBox8.Text);
                vaule2 = Convert.ToSingle(textBox7.Text);
                byte[] b1 = BitConverter.GetBytes(vaule1);
                byte[] b2 = BitConverter.GetBytes(vaule2);

                PublicDataClass._ComStructData.TXBuffer[8] = b1[0];
                PublicDataClass._ComStructData.TXBuffer[9] = b1[1];
                PublicDataClass._ComStructData.TXBuffer[10] = b1[2];
                PublicDataClass._ComStructData.TXBuffer[11] = b1[3];
                PublicDataClass._ComStructData.TXBuffer[12] = b2[0];
                PublicDataClass._ComStructData.TXBuffer[13] = b2[1];
                PublicDataClass._ComStructData.TXBuffer[14] = b2[2];
                PublicDataClass._ComStructData.TXBuffer[15] = b2[3];
                for (int i = 0; i < 16; i++)
                {
                    checksum += PublicDataClass._ComStructData.TXBuffer[i];
                }
                PublicDataClass._ComStructData.TXBuffer[16] = (byte)((checksum) & 0x00ff);
                PublicDataClass._ComStructData.TXBuffer[17] = (byte)(((checksum) & 0xff00) >> 8);

                PublicDataClass._ComStructData.TxLen = 18;
                PublicDataClass.ComFrameMsg = "直流量测试第二步";
                PublicDataClass._ComStructData.TX_TASK = true;
            }
            catch
            {
                MessageBox.Show("数值不能为空！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
