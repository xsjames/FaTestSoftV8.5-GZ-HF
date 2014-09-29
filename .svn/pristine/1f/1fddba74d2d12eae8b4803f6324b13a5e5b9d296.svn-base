using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FaTestSoft.CommonData;

namespace FaTestSoft
{
    public partial class CallTypeViewForm : Form
    {
        public CallTypeViewForm()
        {
            InitializeComponent();
        }
        static byte RadioIndex = 0;
        int rowIndex;
        string[] str = new string[3];
        private void CallTypeViewForm_Load(object sender, EventArgs e)
        {
            if (RadioIndex == 1)
            {
                radioButtonAlone.Checked = false;
                radioButtonRe.Checked = false;
                radioButtonXunH.Checked = true;
                textBoxtime.Text = "5";
                textBoxtime.Enabled = false;

            }
            else if (RadioIndex == 2)
            {
                radioButtonAlone.Checked = false;
                radioButtonRe.Checked = true;
                radioButtonXunH.Checked = false;
                textBoxtime.Text = "5";
                textBoxtime.Enabled = true;

            }
            else 
            {
                radioButtonAlone.Checked = true;
                radioButtonRe.Checked = false;
                radioButtonXunH.Checked = false;
                textBoxtime.Text = "5";
                textBoxtime.Enabled = false;
            }
            dataGridView1.AllowUserToResizeColumns = false;                                              //不允许改变列宽
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;  //行头不允许改变大小
            for (byte i = 0; i < PublicDataClass.SaveText.channelnum; i++)
            {
                str[0] = String.Format("通道{0:d}", i);
                str[1] = PublicDataClass.SaveText.Channel[i].ChannelID;
                if(RadioIndex ==1)
                    str[2] ="循环";
                else if(RadioIndex ==2)
                    str[2] ="重发";
                else 
                    str[2] ="单召";
                this.dataGridView1.Rows.Add(str);
            }
            this.dataGridView1.AllowUserToAddRows = false;//设置用户不能手动给 DataGridView1 添加新行
            
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            for (byte i = 0; i < this.dataGridView1.RowCount ; i++)
            {
                if(PublicDataClass.SaveText.Channel[i].ChannelID ==Convert.ToString(dataGridView1.Rows[i].Cells[1].Value))
                {
                    PublicDataClass.SaveText.Channel[i].calltype = Convert.ToString(dataGridView1.Rows[i].Cells[2].Value);
                    if (RadioIndex == 2)
                    {
                        if (PublicDataClass.SaveText.Channel[i].ChannelID.Contains("NET"))
                        {
                            PublicDataClass._Time.NetReTime = Convert.ToInt16(textBoxtime.Text);
                        }
                        if (PublicDataClass.SaveText.Channel[i].ChannelID.Contains("COM"))
                        {
                            PublicDataClass._Time.ComReTime = Convert.ToInt16(textBoxtime.Text);
                        }

                    }
                }

            }
            
            this.DialogResult = DialogResult.OK;                                     //当文件夹不存在时 将按钮的DialogResult 的属性设置为OK
        }

        private void radioButtonXunH_CheckedChanged(object sender, EventArgs e)
        {
            if (PublicDataClass.SaveText.channelnum == 0)
                return;
            RadioIndex = 1;
            dataGridView1.Rows[rowIndex].Cells[2].Value = "循环"; //获取当前行xh字段的值 
            textBoxtime.Enabled = false;
        }

        private void radioButtonRe_CheckedChanged(object sender, EventArgs e)
        {
            if (PublicDataClass.SaveText.channelnum == 0)
                return;
            RadioIndex = 2;
            dataGridView1.Rows[rowIndex].Cells[2].Value = "重发"; //获取当前行xh字段的值 
            textBoxtime.Enabled = true;
        }

        private void radioButtonAlone_CheckedChanged(object sender, EventArgs e)
        {
            if (PublicDataClass.SaveText.channelnum == 0)
                return;
            RadioIndex = 3;
            dataGridView1.Rows[rowIndex].Cells[2].Value = "单召"; //获取当前行xh字段的值 
            textBoxtime.Enabled = false;
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            rowIndex = e.RowIndex;//获取当前行   
            

        }

      
    }
}
