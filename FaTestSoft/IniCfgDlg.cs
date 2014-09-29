using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using FaTestSoft.CommonData;

namespace FaTestSoft
{
    public partial class IniCfgDlg : Form
    {
        public IniCfgDlg()
        {
            InitializeComponent();
        }
        string[] s;

        private void IniCfgDlg_Load(object sender, EventArgs e)
        {
         
            //string path = System.AppDomain.CurrentDomain.BaseDirectory;
            //path += "\\ini\\动态配置";
              string path = PublicDataClass.PrjPath + "\\ini\\动态配置";
           //  string path = PublicDataClass.PrjPath + "\\ini\\XML";
             s = Directory.GetFiles(path);
            string[] fname = new string[s.Length];
            int j= 0;
            foreach (string file in s)
            {  
                fname[j] = Path.GetFileName(file);
                j++;
            }
         
            listView1.Items.Clear();

            for (int m = 0; m < fname.Length; m++)
            {
                ListViewItem lv = new ListViewItem(fname[m]);

                listView1.Items.Add(lv);
                for (int i = 0; i < PublicDataClass.FILENAME.Length; i++)
                {
                    if (PublicDataClass.FILENAME[i].IndexOf (fname[m])>0)
                        lv.Checked = true;

                }

            }
                
    
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PublicDataClass.DynOptCfgFlag = 1;
            PublicDataClass.FILENAME = new string[listView1.CheckedItems.Count];
            int i = 0;  int j = 0;
             foreach (ListViewItem tempItem in listView1.Items)//循环遍历listView控件中的每一项
            {
                if (tempItem.Checked == true)
                {
                    PublicDataClass.FILENAME[j] = s[i];
                        j++;
                }
                    i++;
            }

             this.DialogResult = DialogResult.OK;
      
           
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
    }
    //public class items
    //{
    //    private string im;
    //    public string Im
    //    {
    //        get { return im; }
    //        set { im = value; }
    //    }
    //    public items(string im)
    //    {
    //        this.im = im;
    //    }
    //}
}
