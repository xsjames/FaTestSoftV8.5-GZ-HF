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
    public partial class CModifyViewForm3 : Form
    {
        public CModifyViewForm3()
        {
            InitializeComponent();
        }
        private Control[] Editors;

        private void CModifyViewForm3_Load(object sender, EventArgs e)
        {
            Editors = new Control[] {
	                                textBoxvalue,// 								                           
									};
            listView1.Controls.Add(textBoxvalue);
            textBoxvalue.Visible = false;
            ListViewItem lv = new ListViewItem("");         
            listView1.Items.Add(lv);
        }

        private void listView1_SubItemClicked(object sender, SubItemEventArgs e)
        {
            listView1.StartEditing(Editors[e.SubItem], e.Item, e.SubItem);
        }

        private void textBoxvalue_Leave(object sender, EventArgs e)
        {
            listView1.EndEditing(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PublicDataClass._Mystruct.linenum = Convert.ToInt16(textBox1.Text);
            PublicDataClass._Mystruct.paramnum = Convert.ToInt16(textBox2.Text);
            PublicDataClass._Mystruct.paramname = new string[PublicDataClass._Mystruct.paramnum];
            for (int j = 0; j < PublicDataClass._Mystruct.paramnum; j++)
            {
                PublicDataClass._Mystruct.paramname[j] = listView1.Items[j].SubItems[0].Text;    
            }
            this.DialogResult = DialogResult.OK;
        }
        
    }
}
