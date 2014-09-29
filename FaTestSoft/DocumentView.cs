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
    public partial class DocumentView : DockContent
    {
        private byte NowMenu = 0;
        ChannelClassViewForm ChannelVFm = new ChannelClassViewForm();
        DeviceInfoAddr Deinfaddrform = new DeviceInfoAddr();           //构造一个新的对象
        CePointClassView CPointfm = new CePointClassView();
        
        public DocumentView()
        {
            InitializeComponent();
        }

        private void DocumentView_Load(object sender, EventArgs e)
        {
            /*ChannelVFm.TopLevel = false;
            panel1.Controls.Add(ChannelVFm);

            ChannelVFm.StartPosition = FormStartPosition.Manual;  //修改窗体的起始位置 后才能修改top和left
            ChannelVFm.Top = 0;

            ChannelVFm.Left = 0;
            ChannelVFm.Width  = this.ParentForm.Width;
            ChannelVFm.Height = this.ParentForm.Height;

            ChannelVFm.Show();*/

        }

        private void Time_Tick(object sender, EventArgs e)
        {
            if (PublicDataClass._ChangeFlag.ChannelClassUpdata == true)
            {
                NowMenu = 1;
                panel1.Controls.Clear();
                PublicDataClass._ChangeFlag.ChannelClassUpdata = false;
                ChannelVFm.TopLevel = false;
                panel1.Controls.Add(ChannelVFm);

                ChannelVFm.StartPosition = FormStartPosition.Manual;  //修改窗体的起始位置 后才能修改top和left
                ChannelVFm.Top = 0;

                ChannelVFm.Left = 0;
                ChannelVFm.Width = panel1.Width;
                ChannelVFm.Height = panel1.Height;

                ChannelVFm.Show();


            }
            else if (PublicDataClass._ChangeFlag.DeviceCalssUpdate == true)
            {
                NowMenu = 2;
                PublicDataClass._ChangeFlag.DeviceCalssUpdate = false;
                panel1.Controls.Clear();

                Deinfaddrform.TopLevel = false;
                panel1.Controls.Add(Deinfaddrform);

                Deinfaddrform.StartPosition = FormStartPosition.Manual;  //修改窗体的起始位置 后才能修改top和left
                Deinfaddrform.Top = 0;

                Deinfaddrform.Left = 0;
                Deinfaddrform.Width = this.panel1.Width;
                Deinfaddrform.Height = this.panel1.Height;

                Deinfaddrform.Show();


            }
            else if (PublicDataClass._ChangeFlag.CePointClassUpdate == true)
            {
                NowMenu = 2;
                PublicDataClass._ChangeFlag.CePointClassUpdate = false;
                panel1.Controls.Clear();

                CPointfm.TopLevel = false;
                panel1.Controls.Add(CPointfm);

                CPointfm.StartPosition = FormStartPosition.Manual;  //修改窗体的起始位置 后才能修改top和left
                CPointfm.Top = 0;

                CPointfm.Left = 0;
                CPointfm.Width = this.panel1.Width;
                CPointfm.Height = this.panel1.Height;

                CPointfm.Show();
           

            }
          
        }

        private void DocumentView_SizeChanged(object sender, EventArgs e)
        {
            switch (NowMenu)
            {
                case 1:
                     ChannelVFm.Width = panel1.Width;
                     ChannelVFm.Height = panel1.Height;
                     break;
                case 2:
                     Deinfaddrform.Width = panel1.Width;
                     Deinfaddrform.Height = panel1.Height;
                     break;
                case 3:
                     CPointfm.Width = panel1.Width;
                     CPointfm.Height = panel1.Height;
                     break;
                default:break;
                     

            }
        }

    }
}
