using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KD.WinFormsUI.Docking;
using FaTestSoft.CommonData;
using FaTestSoft.FUNCTIONCLASS;

namespace FaTestSoft
{

          public partial class CommunicationTestView : DockContent
    {
        public CommunicationTestView()
        {
            InitializeComponent();
        }

        private void CommunicationTestView_Load(object sender, EventArgs e)
        {
            //ImageList image = new ImageList();
            //image.ImageSize = new Size(1, 20);
            //this.listView1.SmallImageList = image;
            listView1.Items.Clear();
            listView1.Columns.Clear();
            listView1.Columns.Add("序号", 60);
            listView1.Columns.Add("端口名称", 200);
            listView1.Columns.Add("测试结果", 200);
         
            string []temp =new  string[]{"网口1","网口2","串口1","串口2","串口3","串口4","串口5","串口6"};
           


            for (int j = 0; j < temp.Length; j++)
            {
                ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                lv.SubItems.Add(temp[j]);
                lv.SubItems.Add("未测");

                listView1.Items.Add(lv);
            }
            
        }
    }
}
