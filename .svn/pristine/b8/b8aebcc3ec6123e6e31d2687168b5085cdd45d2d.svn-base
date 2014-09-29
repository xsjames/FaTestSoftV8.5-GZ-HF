using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KD.WinFormsUI.Docking;

namespace FaTestSoft
{
    public partial class MainDocmentView :  DockContent
    {
        public MainDocmentView()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Left -= 2;
            if (label1.Left < -340)
            {
                label1.Left = this.Width;
                label1.Left -= 50;
            }
        }
    }
}
