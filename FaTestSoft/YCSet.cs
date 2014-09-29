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
    public partial class YCSet : Form
    {
        public YCSet()
        {
            InitializeComponent();
        }

        private void YCSet_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PublicDataClass._DataField.FieldLen = 0;
            PublicDataClass._DataField.FieldVSQ = 0;
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(textBox1.Text)) & 0x00ff);
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(textBox1.Text)) & 0xff00) >> 8);
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = 0x00;
            PublicDataClass._DataField.FieldLen += 3;
            if (radioButton1.Checked == true)//归一化
            {
                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(textBox2.Text)) & 0x00ff);
                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(textBox2.Text)) & 0xff00) >> 8);
                PublicDataClass._DataField.FieldLen += 2;
                PublicDataClass._DataField.FieldVSQ++;
                PublicDataClass._NetTaskFlag.CEL_YC_1 = true;
            }
            if (radioButton2.Checked == true)//标度化
            {
                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(textBox2.Text)) & 0x00ff);
                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(textBox2.Text)) & 0xff00) >> 8);
                PublicDataClass._DataField.FieldLen += 2;
                PublicDataClass._DataField.FieldVSQ++;
                PublicDataClass._NetTaskFlag.CEL_YC_2 = true;

            }
            if (radioButton3.Checked == true)//短浮点
            {
                byte[] b = BitConverter.GetBytes(float.Parse(textBox2.Text));

                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];

                PublicDataClass._DataField.FieldLen += 4;
                PublicDataClass._DataField.FieldVSQ++;
                PublicDataClass._NetTaskFlag.CEL_YC_3 = true;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            PublicDataClass._DataField.FieldLen = 0;
            PublicDataClass._DataField.FieldVSQ=0;
              PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(textBox1.Text )) & 0x00ff);
              PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(textBox1.Text ))& 0xff00) >> 8);
              PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = 0x00;
              PublicDataClass._DataField.FieldLen += 3;
              if (radioButton1.Checked == true)//归一化
              {
                  PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(textBox2.Text)) & 0x00ff);
                  PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(textBox2.Text)) & 0xff00) >> 8);
                  PublicDataClass._DataField.FieldLen += 2;
                  PublicDataClass._DataField.FieldVSQ++;
                  PublicDataClass._NetTaskFlag.SET_YC_1 = true;
              }
              if (radioButton2.Checked == true)//标度化
              {
                  PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(textBox2.Text)) & 0x00ff);
                  PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(textBox2.Text)) & 0xff00) >> 8);
                  PublicDataClass._DataField.FieldLen += 2;
                  PublicDataClass._DataField.FieldVSQ++;
              PublicDataClass._NetTaskFlag.SET_YC_2 = true;

              }
              if (radioButton3.Checked == true)//短浮点
              {
                  byte[] b = BitConverter.GetBytes(float.Parse(textBox2.Text));

                  PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
                  PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
                  PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
                  PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];

                  PublicDataClass._DataField.FieldLen += 4;
                  PublicDataClass._DataField.FieldVSQ++;
                  PublicDataClass._NetTaskFlag.SET_YC_3 = true;

              }

                  
                    
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Text = "0";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Text = "0";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Text = "0.0";
        }
    }
}
