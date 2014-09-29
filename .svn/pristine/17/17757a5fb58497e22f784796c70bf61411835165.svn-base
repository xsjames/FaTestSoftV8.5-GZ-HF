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
    public partial class GprsParamForm : Form
    {
        public GprsParamForm()
        {
            InitializeComponent();
        }

        private void GprsParamForm_Load(object sender, EventArgs e)
        {
            userControl11.SetIpAddress(PublicDataClass._GprsParam.IP);
            userControl12.SetIpAddress(PublicDataClass._GprsParam.BIP);
            textBoxPort.Text = PublicDataClass._GprsParam.Port;
            textBoxBPort.Text = PublicDataClass._GprsParam.BPort;
            textBoxHeart.Text = PublicDataClass._GprsParam.Heart;
            textBoxApn.Text = PublicDataClass._GprsParam.APN;
        }

        private void userControl11_TextChanged(object sender, EventArgs e)
        {
            PublicDataClass._GprsParam.IP = userControl11.GetIpAddress();
        }

        private void userControl12_TextChanged(object sender, EventArgs e)
        {
            PublicDataClass._GprsParam.BIP = userControl12.GetIpAddress();
        }

        private void textBoxPort_TextChanged(object sender, EventArgs e)
        {
            PublicDataClass._GprsParam.Port=textBoxPort.Text ; 
        }

        private void textBoxBPort_TextChanged(object sender, EventArgs e)
        {
            PublicDataClass._GprsParam.BPort=textBoxBPort.Text  ;
        }

        private void textBoxHeart_TextChanged(object sender, EventArgs e)
        {
            PublicDataClass._GprsParam.Heart=textBoxHeart.Text  ;
        }

        private void textBoxApn_TextChanged(object sender, EventArgs e)
        {
            PublicDataClass._GprsParam.APN =textBoxApn.Text  ;
        }
    }
}
