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
    public partial class ACJZfrm : Form
    {
        public ACJZfrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                float vaule1, vaule2, vaule3, vaule4, vaule5, vaule6, vaule7, vaule8, vaule9, vaule10, vaule11, vaule12;
                int checksum = 0;
                PublicDataClass._ComStructData.TxLen = 0;
                PublicDataClass._ComStructData.TXBuffer[0] = 0x68;  //
                PublicDataClass._ComStructData.TXBuffer[1] = 0x3A;  //
                PublicDataClass._ComStructData.TXBuffer[2] = 0x00;  // 
                PublicDataClass._ComStructData.TXBuffer[3] = 0xFF;  //
                PublicDataClass._ComStructData.TXBuffer[4] = 0xFF;  //
                PublicDataClass._ComStructData.TXBuffer[5] = 0xD8;  // 
                PublicDataClass._ComStructData.TXBuffer[6] = 0x07;  // 
                PublicDataClass._ComStructData.TXBuffer[7] = 0x8B;  // 

                vaule1 = Convert.ToSingle(textBox1.Text);
                vaule2 = Convert.ToSingle(textBox2.Text);
                vaule3 = Convert.ToSingle(textBox3.Text);
                vaule4 = Convert.ToSingle(textBox4.Text);
                vaule5 = Convert.ToSingle(textBox5.Text);
                vaule6 = Convert.ToSingle(textBox6.Text);
                vaule7 = Convert.ToSingle(textBox7.Text);
                vaule8 = Convert.ToSingle(textBox8.Text);
                vaule9 = Convert.ToSingle(textBox9.Text);
                vaule10 = Convert.ToSingle(textBox10.Text);
                vaule11 = Convert.ToSingle(textBox11.Text);
                vaule12 = Convert.ToSingle(textBox12.Text);


                byte[] b1 = BitConverter.GetBytes(vaule1);
                byte[] b2 = BitConverter.GetBytes(vaule2);
                byte[] b3 = BitConverter.GetBytes(vaule3);
                byte[] b4 = BitConverter.GetBytes(vaule4);
                byte[] b5 = BitConverter.GetBytes(vaule5);
                byte[] b6 = BitConverter.GetBytes(vaule6);
                byte[] b7 = BitConverter.GetBytes(vaule7);
                byte[] b8 = BitConverter.GetBytes(vaule8);
                byte[] b9 = BitConverter.GetBytes(vaule9);
                byte[] b10 = BitConverter.GetBytes(vaule10);
                byte[] b11 = BitConverter.GetBytes(vaule11);
                byte[] b12 = BitConverter.GetBytes(vaule12);
                

                PublicDataClass._ComStructData.TXBuffer[8] = b1[0];
                PublicDataClass._ComStructData.TXBuffer[9] = b1[1];
                PublicDataClass._ComStructData.TXBuffer[10] = b1[2];
                PublicDataClass._ComStructData.TXBuffer[11] = b1[3];

                PublicDataClass._ComStructData.TXBuffer[12] = b2[0];
                PublicDataClass._ComStructData.TXBuffer[13] = b2[1];
                PublicDataClass._ComStructData.TXBuffer[14] = b2[2];
                PublicDataClass._ComStructData.TXBuffer[15] = b2[3];

                PublicDataClass._ComStructData.TXBuffer[16] = b3[0];
                PublicDataClass._ComStructData.TXBuffer[17] = b3[1];
                PublicDataClass._ComStructData.TXBuffer[18] = b3[2];
                PublicDataClass._ComStructData.TXBuffer[19] = b3[3];

                PublicDataClass._ComStructData.TXBuffer[20] = b4[0];
                PublicDataClass._ComStructData.TXBuffer[21] = b4[1];
                PublicDataClass._ComStructData.TXBuffer[22] = b4[2];
                PublicDataClass._ComStructData.TXBuffer[23] = b4[3];

                PublicDataClass._ComStructData.TXBuffer[24] = b5[0];
                PublicDataClass._ComStructData.TXBuffer[25] = b5[1];
                PublicDataClass._ComStructData.TXBuffer[26] = b5[2];
                PublicDataClass._ComStructData.TXBuffer[27] = b5[3];

                PublicDataClass._ComStructData.TXBuffer[28] = b6[0];
                PublicDataClass._ComStructData.TXBuffer[29] = b6[1];
                PublicDataClass._ComStructData.TXBuffer[30] = b6[2];
                PublicDataClass._ComStructData.TXBuffer[31] = b6[3];

                PublicDataClass._ComStructData.TXBuffer[32] = b7[0];
                PublicDataClass._ComStructData.TXBuffer[33] = b7[1];
                PublicDataClass._ComStructData.TXBuffer[34] = b7[2];
                PublicDataClass._ComStructData.TXBuffer[35] = b7[3];

                PublicDataClass._ComStructData.TXBuffer[36] = b8[0];
                PublicDataClass._ComStructData.TXBuffer[37] = b8[1];
                PublicDataClass._ComStructData.TXBuffer[38] = b8[2];
                PublicDataClass._ComStructData.TXBuffer[39] = b8[3];

                PublicDataClass._ComStructData.TXBuffer[40] = b9[0];
                PublicDataClass._ComStructData.TXBuffer[41] = b9[1];
                PublicDataClass._ComStructData.TXBuffer[42] = b9[2];
                PublicDataClass._ComStructData.TXBuffer[43] = b9[3];

                PublicDataClass._ComStructData.TXBuffer[44] = b10[0];
                PublicDataClass._ComStructData.TXBuffer[45] = b10[1];
                PublicDataClass._ComStructData.TXBuffer[46] = b10[2];
                PublicDataClass._ComStructData.TXBuffer[47] = b10[3];

                PublicDataClass._ComStructData.TXBuffer[48] = b11[0];
                PublicDataClass._ComStructData.TXBuffer[49] = b11[1];
                PublicDataClass._ComStructData.TXBuffer[50] = b11[2];
                PublicDataClass._ComStructData.TXBuffer[51] = b11[3];

                PublicDataClass._ComStructData.TXBuffer[52] = b12[0];
                PublicDataClass._ComStructData.TXBuffer[53] = b12[1];
                PublicDataClass._ComStructData.TXBuffer[54] = b12[2];
                PublicDataClass._ComStructData.TXBuffer[55] = b12[3];
                for (int i = 0; i < 56; i++)
                {
                    checksum += PublicDataClass._ComStructData.TXBuffer[i];
                }
                PublicDataClass._ComStructData.TXBuffer[56] = (byte)((checksum) & 0x00ff);
                PublicDataClass._ComStructData.TXBuffer[57] = (byte)(((checksum) & 0xff00) >> 8);

                PublicDataClass._ComStructData.TxLen = 58;
                PublicDataClass.ComFrameMsg = "交流量校准";
                PublicDataClass._ComStructData.TX_TASK = true;
            }
            catch
            {
                MessageBox.Show("数值不能为空！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ACJZfrm_Load(object sender, EventArgs e)
        {
            textBox1.Text=PublicDataClass.JZPARAM.vaule1;
            textBox2.Text=PublicDataClass.JZPARAM.vaule2 ;
            textBox3.Text=PublicDataClass.JZPARAM.vaule3;
            textBox4.Text=PublicDataClass.JZPARAM.vaule4;
            textBox5.Text= PublicDataClass.JZPARAM.vaule5;
            textBox6.Text= PublicDataClass.JZPARAM.vaule6;
            textBox7.Text= PublicDataClass.JZPARAM.vaule7;
            textBox8.Text=PublicDataClass.JZPARAM.vaule8;
            textBox9.Text= PublicDataClass.JZPARAM.vaule9;
            textBox10.Text=PublicDataClass.JZPARAM.vaule10;
            textBox11.Text=PublicDataClass.JZPARAM.vaule11;
            textBox12.Text = PublicDataClass.JZPARAM.vaule12;
        }

        private void ACJZfrm_FormClosing(object sender, FormClosingEventArgs e)
        {

            PublicDataClass.JZPARAM.vaule1 = textBox1.Text;
            PublicDataClass.JZPARAM.vaule2 = textBox2.Text;
            PublicDataClass.JZPARAM.vaule3 = textBox3.Text;
            PublicDataClass.JZPARAM.vaule4 = textBox4.Text;
            PublicDataClass.JZPARAM.vaule5 = textBox5.Text;
            PublicDataClass.JZPARAM.vaule6 = textBox6.Text;
            PublicDataClass.JZPARAM.vaule7 = textBox7.Text;
            PublicDataClass.JZPARAM.vaule8 = textBox8.Text;
            PublicDataClass.JZPARAM.vaule9 = textBox9.Text;
            PublicDataClass.JZPARAM.vaule10 = textBox10.Text;
            PublicDataClass.JZPARAM.vaule11 = textBox11.Text;
            PublicDataClass.JZPARAM.vaule12 = textBox12.Text;
        }

        
    }
}
