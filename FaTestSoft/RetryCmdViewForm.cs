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
    public partial class RetryCmdViewForm : Form
    {
        public RetryCmdViewForm()
        {
            InitializeComponent();
        }
        static byte chanelty = 0;

        private void RetryCmdViewForm_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            textBox1.Text = @"5";
            if (comboBox1.SelectedIndex == 0)
                textBox1.Enabled = false;

            rbcom.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
                textBox1.Enabled = false;
            else
                textBox1.Enabled = true;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字
                {

                    MessageBox.Show("只能输入数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Handled = true;
                }
            }
        }

        private void rbcom_CheckedChanged(object sender, EventArgs e)
        {
            if (rbcom.Checked == true)
                chanelty = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
                chanelty = 2;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
                chanelty = 3;
        }

        private void buttonok_Click(object sender, EventArgs e)
        {
       
            if (chanelty == 1)
            {
                if (radioButton4.Checked == true)//循环
                {
                    if (comboBox1.SelectedIndex == 0)
                        return;
                    else if (comboBox1.SelectedIndex == 1)
                    {
                        PublicDataClass._ComTaskFlag.C_RQ_NA_LINKREQ_F= true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 1;
                    }
                    else if (comboBox1.SelectedIndex == 2)
                    {
                        //PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_OK = true;
                        //PublicDataClass._ThreadIndex.RetryNetThreadID = 2;
                    }
                    else if (comboBox1.SelectedIndex == 3)
                    {
                        //PublicDataClass._NetTaskFlag.STOP_LINKREQ = true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 3;
                    }
                    else if (comboBox1.SelectedIndex == 4)
                    {
                        //PublicDataClass._NetTaskFlag.STOP_LINKREQ_OK = true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 4;
                    }
                    else if (comboBox1.SelectedIndex == 5)
                    {
                        //PublicDataClass._NetTaskFlag.Do_TESTACT = true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 5;
                    }
                    else if (comboBox1.SelectedIndex == 6)
                    {
                        //PublicDataClass._NetTaskFlag.Do_TESTACT_OK = true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 6;
                    }
                    else if (comboBox1.SelectedIndex == 7)
                    {
                        //PublicDataClass._NetTaskFlag.Do_OKTACT = true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 7;
                    }
                    else if (comboBox1.SelectedIndex == 8)
                    {
                        //PublicDataClass._NetTaskFlag.C_CS_NA_1 = true; //对时
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 8;
                    }
                    else if (comboBox1.SelectedIndex == 9)
                    {
                        //PublicDataClass._NetTaskFlag.C_IC_NA_1 = true; //总召唤
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 9;
                        PublicDataClass._FrameTime.LoopTime = 15;
                    }
                    else if (comboBox1.SelectedIndex == 10)
                    {
                        //PublicDataClass._NetTaskFlag.Reset_1 = true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 10;
                    }
                    else if (comboBox1.SelectedIndex == 11)
                    {
                        //PublicDataClass._NetTaskFlag.AloneCallYc_1 = true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 11;
                    }
                    else if (comboBox1.SelectedIndex == 12)
                    {
                        //PublicDataClass._NetTaskFlag.AloneCallYx_1 = true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 12;
                    }



                    PublicDataClass._Time.NetReTime = Convert.ToInt16(textBox1.Text);
                    PublicDataClass._CallType.NetTy = 1;
                }
                else//单次
                {
                    if (comboBox1.SelectedIndex == 0)
                        return;
                    else if (comboBox1.SelectedIndex == 1)
                    {
                        PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F = true;
                       
                    }
                    else if (comboBox1.SelectedIndex == 2)
                    {
                        PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_OK = true;
                        
                    }
                    else if (comboBox1.SelectedIndex == 3)
                    {
                        PublicDataClass._NetTaskFlag.STOP_LINKREQ = true;
                       
                    }
                    else if (comboBox1.SelectedIndex == 4)
                    {
                        PublicDataClass._NetTaskFlag.STOP_LINKREQ_OK = true;
                       
                    }
                    else if (comboBox1.SelectedIndex == 5)
                    {
                        PublicDataClass._NetTaskFlag.Do_TESTACT = true;
                       
                    }
                    else if (comboBox1.SelectedIndex == 6)
                    {
                        PublicDataClass._NetTaskFlag.Do_TESTACT_OK = true;
                       
                    }
                    else if (comboBox1.SelectedIndex == 7)
                    {
                        PublicDataClass._NetTaskFlag.Do_OKTACT = true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 7;
                    }
                    else if (comboBox1.SelectedIndex == 8)
                    {
                        PublicDataClass._NetTaskFlag.C_CS_NA_1 = true; //对时
                       
                    }
                    else if (comboBox1.SelectedIndex == 9)
                    {
                        PublicDataClass._NetTaskFlag.C_IC_NA_1 = true; //总召唤
                      
                    }
                    else if (comboBox1.SelectedIndex == 10)
                    {
                        PublicDataClass._NetTaskFlag.Reset_1 = true;
                       
                    }
                    else if (comboBox1.SelectedIndex == 11)
                    {
                        PublicDataClass._NetTaskFlag.AloneCallYc_1 = true;
                        
                    }
                    else if (comboBox1.SelectedIndex == 12)
                    {
                        PublicDataClass._NetTaskFlag.AloneCallYx_1 = true;
                       
                    }
                    PublicDataClass._CallType.NetTy = 2;
                }
               
            }
        else    if (chanelty == 2)
            {
                if (radioButton4.Checked == true)//循环
                {
                    if (comboBox1.SelectedIndex == 0)
                        return;
                    else if (comboBox1.SelectedIndex == 1)
                    {
                        PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F = true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 1;
                    }
                    else if (comboBox1.SelectedIndex == 2)
                    {
                        PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_OK = true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 2;
                    }
                    else if (comboBox1.SelectedIndex == 3)
                    {
                        PublicDataClass._NetTaskFlag.STOP_LINKREQ = true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 3;
                    }
                    else if (comboBox1.SelectedIndex == 4)
                    {
                        PublicDataClass._NetTaskFlag.STOP_LINKREQ_OK = true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 4;
                    }
                    else if (comboBox1.SelectedIndex == 5)
                    {
                        PublicDataClass._NetTaskFlag.Do_TESTACT = true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 5;
                    }
                    else if (comboBox1.SelectedIndex == 6)
                    {
                        PublicDataClass._NetTaskFlag.Do_TESTACT_OK = true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 6;
                    }
                    else if (comboBox1.SelectedIndex == 7)
                    {
                        PublicDataClass._NetTaskFlag.Do_OKTACT = true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 7;
                    }
                    else if (comboBox1.SelectedIndex == 8)
                    {
                        PublicDataClass._NetTaskFlag.C_CS_NA_1 = true; //对时
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 8;
                    }
                    else if (comboBox1.SelectedIndex == 9)
                    {
                        PublicDataClass._NetTaskFlag.C_IC_NA_1 = true; //总召唤
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 9;
                        PublicDataClass._FrameTime.LoopTime = 15;
                    }
                    else if (comboBox1.SelectedIndex == 10)
                    {
                        PublicDataClass._NetTaskFlag.Reset_1 = true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 10;
                    }
                    else if (comboBox1.SelectedIndex == 11)
                    {
                        PublicDataClass._NetTaskFlag.AloneCallYc_1 = true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 11;
                    }
                    else if (comboBox1.SelectedIndex == 12)
                    {
                        PublicDataClass._NetTaskFlag.AloneCallYx_1 = true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 12;
                    }



                    PublicDataClass._Time.NetReTime = Convert.ToInt16(textBox1.Text);
                    PublicDataClass._CallType.NetTy = 1;
                }
                else//单次
                {
                    if (comboBox1.SelectedIndex == 0)
                        return;
                    else if (comboBox1.SelectedIndex == 1)
                    {
                        PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F = true;

                    }
                    else if (comboBox1.SelectedIndex == 2)
                    {
                        PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_OK = true;

                    }
                    else if (comboBox1.SelectedIndex == 3)
                    {
                        PublicDataClass._NetTaskFlag.STOP_LINKREQ = true;

                    }
                    else if (comboBox1.SelectedIndex == 4)
                    {
                        PublicDataClass._NetTaskFlag.STOP_LINKREQ_OK = true;

                    }
                    else if (comboBox1.SelectedIndex == 5)
                    {
                        PublicDataClass._NetTaskFlag.Do_TESTACT = true;

                    }
                    else if (comboBox1.SelectedIndex == 6)
                    {
                        PublicDataClass._NetTaskFlag.Do_TESTACT_OK = true;

                    }
                    else if (comboBox1.SelectedIndex == 7)
                    {
                        PublicDataClass._NetTaskFlag.Do_OKTACT = true;
                        PublicDataClass._ThreadIndex.RetryNetThreadID = 7;
                    }
                    else if (comboBox1.SelectedIndex == 8)
                    {
                        PublicDataClass._NetTaskFlag.C_CS_NA_1 = true; //对时

                    }
                    else if (comboBox1.SelectedIndex == 9)
                    {
                        PublicDataClass._NetTaskFlag.C_IC_NA_1 = true; //总召唤

                    }
                    else if (comboBox1.SelectedIndex == 10)
                    {
                        PublicDataClass._NetTaskFlag.Reset_1 = true;

                    }
                    else if (comboBox1.SelectedIndex == 11)
                    {
                        PublicDataClass._NetTaskFlag.AloneCallYc_1 = true;

                    }
                    else if (comboBox1.SelectedIndex == 12)
                    {
                        PublicDataClass._NetTaskFlag.AloneCallYx_1 = true;

                    }
                    PublicDataClass._CallType.NetTy = 2;
                }

            }
            this.DialogResult = DialogResult.OK;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                textBox1.Enabled = true;
            }
            else
            {
                textBox1.Enabled = false;
            }
        }

     
    }
}
