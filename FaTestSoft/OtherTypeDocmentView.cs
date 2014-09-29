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
    public partial class OtherTypeDocmentView : DockContent
    {
        public OtherTypeDocmentView()
        {
            InitializeComponent();
        }
        JSCmdViewForm jsfm = new JSCmdViewForm();
        //FileCmdViewForm Filefm = new FileCmdViewForm();
        DateCodForm Codefm = new DateCodForm();
        CodeUpdateViewForm codeUpfm = new CodeUpdateViewForm();
        monitor monitorfm = new monitor();
        private static byte NowMenuIndex = 1;                      //当前画面号
        private int num = 0;
        private void OtherTypeDocmentView_Load(object sender, EventArgs e)
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
            
            NowMenuIndex = 1;

            jsfm.TopLevel = false;
            tabPage1.Controls.Add(jsfm);

            jsfm.StartPosition = FormStartPosition.Manual;  //修改窗体的起始位置 后才能修改top和left
            jsfm.Top = 0;

            jsfm.Left = 0;
            jsfm.Width = this.Width;
            jsfm.Height = this.Height;

            jsfm.Show();
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            try
            {
                switch (e.TabPage.Name)
                {
                    case "tabPage1":                //
                        NowMenuIndex = 1;
                        jsfm.TopLevel = false;
                        tabPage1.Controls.Add(jsfm);

                        jsfm.StartPosition = FormStartPosition.Manual;  //修改窗体的起始位置 后才能修改top和left
                        jsfm.Top = 0;

                        jsfm.Left = 0;
                        jsfm.Width = this.Width;
                        jsfm.Height = this.Height;

                        jsfm.Show();
                        //PublicDataClass._WindowsVisable.DateCodFormVisable = false ; //
                        break;

                    case "tabPage2":                //
                        NowMenuIndex = 2;
                        Codefm.TopLevel = false;
                        //tabPage2.Controls.Add(Codefm);

                        //Codefm.StartPosition = FormStartPosition.Manual;  //修改窗体的起始位置 后才能修改top和left
                        //Codefm.Top = 0;

                        //Codefm.Left = 0;
                        //Codefm.Width = this.Width;
                        //Codefm.Height = this.Height;

                        //Codefm.Show();
                        //PublicDataClass._WindowsVisable.DateCodFormVisable = true; //窗体可见
                        break;

                    case "tabPage3":                //
                        NowMenuIndex = 3;
                        codeUpfm.TopLevel = false;
                        tabPage3.Controls.Add(codeUpfm);

                        codeUpfm.StartPosition = FormStartPosition.Manual;  //修改窗体的起始位置 后才能修改top和left
                        codeUpfm.Top = 0;

                        codeUpfm.Left = 0;
                        codeUpfm.Width = this.Width;
                        codeUpfm.Height = this.Height;

                        codeUpfm.Show();
                        //PublicDataClass._WindowsVisable.DateCodFormVisable = false; //
                        break;
                    case "tabPage4":                //
                        NowMenuIndex = 4;
                        monitorfm.TopLevel = false;
                        tabPage4.Controls.Add(monitorfm);

                        monitorfm.StartPosition = FormStartPosition.Manual;  //修改窗体的起始位置 后才能修改top和left
                        monitorfm.Top = 0;

                        monitorfm.Left = 0;
                        monitorfm.Width = this.Width;
                        monitorfm.Height = this.Height;

                        monitorfm.Show();
                        //PublicDataClass._WindowsVisable.DateCodFormVisable = false; //
                        break;

                    default:
                        break;

                }

            }
            catch
            {


            }
        }

        private void OtherTypeDocmentView_SizeChanged(object sender, EventArgs e)
        {
            if (NowMenuIndex == 1)
            {
                jsfm.Width = this.Width;
                jsfm.Height = this.Height;
            }
            else if (NowMenuIndex == 2)
            {
                Codefm.Width = this.Width;
                Codefm.Height = this.Height;

            }
            else if (NowMenuIndex == 3)
            {
                codeUpfm.Width = this.Width;
                codeUpfm.Height = this.Height;
            }
            else if (NowMenuIndex == 4)
            {
                monitorfm.Width = this.Width;
                monitorfm.Height = this.Height;
            }
        }

        private void OtherTypeDocmentView_Activated(object sender, EventArgs e)
        {
            //PublicDataClass._WindowsVisable.OtherTypeViewVisable = true;            //窗体可见
            //timer1.Enabled = true;
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

        private void OtherTypeDocmentView_Deactivate(object sender, EventArgs e)
        {
            //PublicDataClass._WindowsVisable.OtherTypeViewVisable = false;        //窗体不可见
            //timer1.Enabled = false;
            //PublicDataClass._WindowsVisable.DateCodFormVisable = false; //
        }
    }
}
