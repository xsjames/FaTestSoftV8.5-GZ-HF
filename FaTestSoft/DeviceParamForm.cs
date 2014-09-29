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
    public partial class DeviceParamForm : Form
    {
        public DeviceParamForm()
        {
            InitializeComponent();
        }
        TextBox tbox = new TextBox();
        private void DeviceParamForm_Load(object sender, EventArgs e)
        {
            string str = @"";
            for (byte j= 0; j < PublicDataClass._SysParam.num; j++)
            {
                str = String.Format("{0:d}", j);
                ListViewItem lv = new ListViewItem(str);
                lv.SubItems.Add(PublicDataClass._SysParam.NameTable[j]);
                lv.SubItems.Add(PublicDataClass._SysParam.ValueTable[j]);
                lv.SubItems.Add(PublicDataClass._SysParam.ByteTable[j]);
                listView1.Items.Add(lv);
            }
        }

        private void ListView_MousedoubleClick(object sender, MouseEventArgs e)
        {
            this.listView1.SelectedItems[0].ForeColor = Color.Red;//设置当前选择项为红色
            if (e.X > (listView1.Columns[0].Width + listView1.Columns[1].Width) && e.X < (listView1.Columns[0].Width + listView1.Columns[1].Width + listView1.Columns[2].Width))
            {
                tbox.Text = this.listView1.SelectedItems[0].SubItems[2].Text;
                tbox.BackColor = Color.White;
                tbox.Font = this.Font;
                tbox.Multiline = true;
                tbox.Leave += new System.EventHandler(tbox_Leave);
                listView1.AddEmbeddedControl(tbox, 2, this.listView1.SelectedItems[0].Index);


                //this.textBoxPort.Leave += new System.EventHandler(this.txtport_Leave);

            }
        }
        private void tbox_Leave(object sender, EventArgs e)
        {
            //listView1.RemoveEmbeddedControl(tbox);
            if (tbox.Focused == false)
            {

                this.listView1.SelectedItems[0].SubItems[2].Text = tbox.Text;
                listView1.RemoveEmbeddedControl(tbox);

            }
            else
                listView1.RemoveEmbeddedControl(tbox);
        }
    }
}
