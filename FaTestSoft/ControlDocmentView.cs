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

namespace FaTestSoft
{
    public partial class ControlDocmentView : DockContent
    {
        public ControlDocmentView()
        {
            InitializeComponent();
        }
        YkCmdViewForm YkFm       = new YkCmdViewForm();
        ResetCmdViewForm RetFm   = new ResetCmdViewForm();
        FuGuiCmdViewForm Fuguifm = new FuGuiCmdViewForm();

        private static byte NowMenuIndex = 1;                      //当前画面号
        private int num = 0;

        private void ControlDocmentView_Load(object sender, EventArgs e)
        {
            byte i = 0;
            if (PublicDataClass.SaveText.devicenum == 0)
            {
                comboBox1.Text = "无信息";

            }
            else
            {
                for (i = 0; i < PublicDataClass.SaveText.devicenum; i++)
                {
                    comboBox1.Items.Add(PublicDataClass.SaveText.Device[i].PointName);
                }
                comboBox1.Text = PublicDataClass.SaveText.Device[0].PointName;

            }
            num = PublicDataClass.SaveText.devicenum;
            PublicDataClass.LinSPointName = comboBox1.Text;

            NowMenuIndex  =1;

       
            YkFm.TopLevel = false;
            tabPage1.Controls.Add(YkFm);
            YkFm.StartPosition = FormStartPosition.Manual;  //修改窗体的起始位置 后才能修改top和left
            YkFm.Top    = 0;

            YkFm.Left   = 0;
            YkFm.Width = this.tabPage1.Width;
            YkFm.Height = this.tabPage1.Height;

            YkFm.Show();

        }

        private void ControlDocmentView_SizeChanged(object sender, EventArgs e)
        {
            if(NowMenuIndex  ==1)
            {
                tabPage1.Width = this.Width;
                tabPage1.Height = this.Height;
                YkFm.Width = tabPage1.Width;
                YkFm.Height = tabPage1.Height;
            }
            else if(NowMenuIndex  ==2)
            {
                tabPage2.Width = this.Width;
                tabPage2.Height = this.Height;
                RetFm.Width = this.tabPage2.Width;
                RetFm.Height = this.tabPage2.Height;

            }
            else if (NowMenuIndex == 3)
            {
                tabPage3.Width  = this.Width;
                tabPage3.Height = this.Height;
                Fuguifm.Width   = this.tabPage3.Width;
                Fuguifm.Height  = this.tabPage3.Height;

            }

        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            try
            {
                switch (e.TabPage.Name)
                {
                    case "tabPage1":                //
                        NowMenuIndex  =1;
                        YkFm.TopLevel = false;
                        tabPage1.Controls.Add(YkFm);

                        YkFm.StartPosition = FormStartPosition.Manual;  //修改窗体的起始位置 后才能修改top和left
                        YkFm.Top    = 0;
                        YkFm.Left   = 0;
                        tabPage1.Width = this.Width;
                        tabPage1.Height = this.Height;
                        YkFm.Width = this.tabPage1.Width;
                        YkFm.Height = this.tabPage1.Height;

                        YkFm.Show();

                        break;

                    case "tabPage2":                //
                        NowMenuIndex   =2;
                        RetFm.TopLevel = false;
                        tabPage2.Controls.Add(RetFm);

                        RetFm.StartPosition = FormStartPosition.Manual;  //修改窗体的起始位置 后才能修改top和left
                        RetFm.Top = 0;

                        RetFm.Left   = 0;

                        tabPage2.Width = this.Width;
                        tabPage2.Height = this.Height;
                        RetFm.Width = this.tabPage2.Width;
                        RetFm.Height = this.tabPage2.Height;

                        RetFm.Show();
                        break;

                    case "tabPage3":                //
                        NowMenuIndex = 3;
                        Fuguifm.TopLevel = false;
                        tabPage3.Controls.Add(Fuguifm);

                        Fuguifm.StartPosition = FormStartPosition.Manual;  //修改窗体的起始位置 后才能修改top和left
                        Fuguifm.Top = 0;

                        Fuguifm.Left = 0;

                        tabPage3.Width = this.Width;
                        tabPage3.Height = this.Height;
                        Fuguifm.Width = this.tabPage3.Width;
                        Fuguifm.Height = this.tabPage3.Height;

                        Fuguifm.Show();
                        break;

                    default:
                        break;

                }

            }
            catch
            {


            }
        }

        private void ControlDocmentView_Activated(object sender, EventArgs e)
        {
            if (num == PublicDataClass.SaveText.devicenum)
                return;
            comboBox1.Items.Clear();
            num = PublicDataClass.SaveText.devicenum;
            if (PublicDataClass.SaveText.devicenum == 0)
            {
                comboBox1.Text = "无信息";

            }
            else
            {
                for (byte i = 0; i < PublicDataClass.SaveText.devicenum; i++)
                {
                    comboBox1.Items.Add(PublicDataClass.SaveText.Device[i].PointName);
                }
                comboBox1.Text = PublicDataClass.SaveText.Device[0].PointName;

            }
        }

        private void ControlDocmentView_Deactivate(object sender, EventArgs e)
        {

        }
    }
}
