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
    public partial class OpenLinkForm : Form
    {
        public OpenLinkForm()
        {
            InitializeComponent();
        }

        

        private void OpenLinkForm_Load(object sender, EventArgs e)
        {
            for (byte i = 0; i < PublicDataClass.SaveText.channelnum; i++)
            {
                ListViewItem lv = new ListViewItem(PublicDataClass.SaveText.Channel[i].ChannelID);
                lv.SubItems.Add(PublicDataClass.SaveText.Channel[i].potocol);
                lv.SubItems.Add(PublicDataClass.SaveText.Channel[i].baud);
                lv.SubItems.Add(PublicDataClass.SaveText.Channel[i].jy);
                lv.SubItems.Add(PublicDataClass.SaveText.Channel[i].IP);
                lv.SubItems.Add(PublicDataClass.SaveText.Channel[i].port);
                listView1.Items.Add(lv);
            }

        }
        private void CheckAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem tempItem in listView1.Items)//循环遍历listView控件中的每一项
            {
                if (tempItem.Checked == false)//如果当前项处于未选中状态
                {
                    tempItem.Checked = true;//设置当前项为选中状态
                }
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem tempItem in listView1.Items)//循环遍历listView控件中的每一项
            {
                if (tempItem.Checked)//如果当前项处于选中状态
                {
                    tempItem.Checked = false;//设置当前项为未选中状态
                }
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {

            if (listView1.CheckedItems.Count == 0)
            {
                MessageBox.Show("需要打开的通道没有选取", "信息",
                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
            PublicDataClass._OpenLinkState.Linknum = listView1.CheckedItems.Count;
            PublicDataClass._OpenLinkState.LinkDevName = new string[PublicDataClass._OpenLinkState.Linknum];
            for (byte i = 0; i < PublicDataClass._OpenLinkState.Linknum; i++)
            {
                PublicDataClass._OpenLinkState.LinkDevName[i] = listView1.CheckedItems[i].Text;

            }
            this.DialogResult = DialogResult.OK;                                     //当文件夹不存在时 将按钮的DialogResult 的属性设置为OK
        }


    }
}
