using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FaTestSoft.CommonData;
using FaTestSoft.FUNCTIONCLASS;

namespace FaTestSoft
{
    public partial class ResetCmdViewForm : Form
    {
        public ResetCmdViewForm()
        {
            InitializeComponent();
        }
        private int ty = 0;
        static byte index = 0;
        private void ResetCmdViewForm_Load(object sender, EventArgs e)
        {
            radioreset.Enabled     = false;
            radioinitparam.Enabled = false;
            radiocleardata.Enabled = false;
            radioclearled.Enabled = false;

        }
        private void textBoxpassword_TextChanged(object sender, EventArgs e)
        {
            if (textBoxpassword.Text == "abcd")
            {
                radioreset.Enabled      = true;
                radioinitparam.Enabled  = true;
                radiocleardata.Enabled  = true;
                radioclearled.Enabled = true;
            }
        }

        private void buttonexe_Click(object sender, EventArgs e)
        {
            if (PublicDataClass.LinSPointName == "无信息")
            {
                MessageBox.Show("无测量点信息可操作", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            else
            {
                ty = PublicFunction.CheckPointOfCommunicationEntrace(PublicDataClass.LinSPointName);
                if (ty == 0)
                {
                    MessageBox.Show("无测量点信息可操作", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }

            }
            
            if (index == 1)
            {
                if (ty == 1)
                    PublicDataClass._ComTaskFlag.Reset_1 = true;

                if (ty == 2)
                    PublicDataClass._NetTaskFlag.Reset_1 = true;
                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.Reset_1 = true;
            }
            else if (index ==2)
            {
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                if (ty == 1)
                    PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;

                if (ty == 2)
                    PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;
                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.SET_PARAM_CON = true;
                PublicDataClass.seqflag = 0;
                PublicDataClass.seq = 1;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = 0x0b00;
            }
            else if (index == 3)
            {
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ =1;
                if (ty == 1)
                    PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;

                if (ty == 2)
                    PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;
                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.SET_PARAM_CON = true;
                PublicDataClass.seqflag = 0;
                PublicDataClass.seq = 1;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = 0x0c00;
            }
            else if (index == 4)
            {
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                if (ty == 1)
                    PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;

                if (ty == 2)
                    PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;
                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.SET_PARAM_CON = true;
                PublicDataClass.seqflag = 0;
                PublicDataClass.seq = 1;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = 0x0d00;
            }
            else
                return;
        }

        private void radioreset_CheckedChanged(object sender, EventArgs e)
        {
            index = 1;
        }

        private void radioinitparam_CheckedChanged(object sender, EventArgs e)
        {
            index = 2;
        }

        private void radiocleardata_CheckedChanged(object sender, EventArgs e)
        {
            index = 3;
        }

        private void radioclearled_CheckedChanged(object sender, EventArgs e)
        {
            index = 4;
        }

       
    }
}
