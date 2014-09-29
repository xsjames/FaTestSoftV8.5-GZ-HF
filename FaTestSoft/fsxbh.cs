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
    public partial class fsxbh : Form
    {
        public fsxbh()
        {
            InitializeComponent();
        }

        private void fsxbh_Load(object sender, EventArgs e)
        {
            textIdata.Text = "5";
            radioButton4.Checked = true;

            PublicDataClass._FsxParam.Checkeddata = 0;
            PublicDataClass._FsxParam.IdataValue = textIdata.Text;

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            PublicDataClass._FsxParam.Checkeddata = 1;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            PublicDataClass._FsxParam.Checkeddata = 0;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            PublicDataClass._FsxParam.Checkeddata = 2;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            PublicDataClass._FsxParam.Checkeddata = 3;
        }

        private void textIdata_MouseLeave(object sender, EventArgs e)
        {
            PublicDataClass._FsxParam.IdataValue = textIdata.Text ;
        }
    }
}
