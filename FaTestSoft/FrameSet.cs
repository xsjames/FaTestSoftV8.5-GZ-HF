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
    public partial class FrameSet : Form
    {
        public FrameSet()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                //PublicDataClass.Framelen = 237;
                //PublicDataClass.CodeUpdatalen = 218;
                PublicDataClass.Framelen = 231;
                PublicDataClass.CodeUpdatalen = 212;
            }
            else if (radioButton2.Checked == true)
            {
                PublicDataClass.Framelen = 800;
                PublicDataClass.CodeUpdatalen = 512;
                //PublicDataClass.Framelen = 794;
                //PublicDataClass.CodeUpdatalen = 506;
            }
        }

        private void FrameSet_Load(object sender, EventArgs e)
        {
            //if(PublicDataClass.Framelen == 237)
            //{
            //    radioButton1.Checked = true;
            //}
             if (PublicDataClass.Framelen == 800)
            {
                radioButton2.Checked = true;
            }
          else  if (PublicDataClass.Framelen == 231)
            {
                radioButton1.Checked = true;
            }
            //else if (PublicDataClass.Framelen == 794)
            //{
            //    radioButton2.Checked = true;
            //}
        }
    }
}
