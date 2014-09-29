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
    //public partial class ProductionVersionFun : Form
        public partial class ProductionVersionFun : DockContent
    {
        public ProductionVersionFun()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (PublicDataClass._ChangeFlag.ProductionVersionViewUpdate == true)
            {
                PublicDataClass._ChangeFlag.ProductionVersionViewUpdate = false;
               
                timer1.Enabled = false;
                label1.Text = PublicDataClass.PrjName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PublicDataClass._ChangeFlag.CommunicationTestUpdate = true ;
        }



    }
}
