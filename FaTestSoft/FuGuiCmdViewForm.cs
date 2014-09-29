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
    public partial class FuGuiCmdViewForm : Form
    {
        public FuGuiCmdViewForm()
        {
            InitializeComponent();
        }
        private int ty = 0;

        /// <summary>
        /// 执行--按钮的消息响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
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
            if (ty == 1)
                PublicDataClass._ComTaskFlag.FuGuiCmd = true;

            if (ty == 2)
                PublicDataClass._NetTaskFlag.FuGuiCmd = true;

        }
        /// <summary>
        /// 窗体初始化函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FuGuiCmdViewForm_Load(object sender, EventArgs e)
        {
            comboBoxsl.SelectedIndex = 0;
            comboBoxsl.Enabled = false;
        }

        private void textBoxpw_TextChanged(object sender, EventArgs e)
        {
            if (textBoxpw.Text == "abcd" || textBoxpw.Text == "ABCD")
            {
                comboBoxsl.Enabled = true;
            }
            else
                comboBoxsl.Enabled = false;
        }
        /// <summary>
        /// 选择的内容发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxsl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxsl.SelectedIndex)
            {
                case 0:
                    PublicDataClass.ParamInfoAddr = 0x1000; // G0__重合闸信号复归
                    break;

                case 1:
                    PublicDataClass.ParamInfoAddr = 0x2000;//G1__过流信号复归
                    break;
                case 2:
                    PublicDataClass.ParamInfoAddr = 0x3000;//G2__事故信号复归1
                    break;
                case 3:
                    PublicDataClass.ParamInfoAddr = 0x3001;//G3__事故信号复归2
                    break;
                case 4:
                    PublicDataClass.ParamInfoAddr = 0x3002;//G4__事故信号复归3
                    break;
                case 5:
                    PublicDataClass.ParamInfoAddr = 0x3003;//G5__事故信号复归4
                    break;
                case 6:
                    PublicDataClass.ParamInfoAddr = 0x3004;//G6__事故信号复归5
                    break;
                case 7:
                    PublicDataClass.ParamInfoAddr = 0x3005;//G7__事故信号复归6
                    break;
                case 8:
                    PublicDataClass.ParamInfoAddr = 0x3006;//G8__事故信号复归7
                    break;
                case 9:
                    PublicDataClass.ParamInfoAddr = 0x3007;//G9__事故信号复归8
                    break;
                case 10:
                    PublicDataClass.ParamInfoAddr = 0x3008;//G10__事故信号复归9
                    break;
                case 11:
                    PublicDataClass.ParamInfoAddr = 0x3009;//G11__事故信号复归10
                    break;
                case 12:
                    PublicDataClass.ParamInfoAddr = 0x300a;//G12__事故信号复归11
                    break;
                case 13:
                    PublicDataClass.ParamInfoAddr = 0x300b;//G13__事故信号复归12
                    break;
                case 14:
                    PublicDataClass.ParamInfoAddr = 0x300c;//G14__事故信号复归13
                    break;
                case 15:
                    PublicDataClass.ParamInfoAddr = 0x300d;//G15__事故信号复归14
                    break;
                case 16:
                    PublicDataClass.ParamInfoAddr = 0x300e;//G16__事故信号复归15
                    break;

                  




                default: break;
            }
        }
    }
}
