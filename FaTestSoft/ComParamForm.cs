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
    public partial class ComParamForm : Form
    {
        public ComParamForm()
        {
            InitializeComponent();
        }

        private void ComParamForm_Load(object sender, EventArgs e)
        {
            com1function.SelectedIndex = 0;
            com2function.SelectedIndex = 0;
            com3function.SelectedIndex = 0;
            com4function.SelectedIndex = 0;

            com1baud.SelectedIndex     = Convert.ToUInt16(PublicDataClass._ComParam.BaudRateTable[0]);
            com2baud.SelectedIndex     = Convert.ToUInt16(PublicDataClass._ComParam.BaudRateTable[1]);
            com3baud.SelectedIndex     = Convert.ToUInt16(PublicDataClass._ComParam.BaudRateTable[2]);
            com4baud.SelectedIndex     = Convert.ToUInt16(PublicDataClass._ComParam.BaudRateTable[3]);

            com1jy.SelectedIndex       = Convert.ToUInt16(PublicDataClass._ComParam.JyTable[0]);
            com2jy.SelectedIndex       = Convert.ToUInt16(PublicDataClass._ComParam.JyTable[1]);
            com3jy.SelectedIndex       = Convert.ToUInt16(PublicDataClass._ComParam.JyTable[2]);
            com4jy.SelectedIndex       = Convert.ToUInt16(PublicDataClass._ComParam.JyTable[3]);

            com1databit.SelectedIndex  = Convert.ToUInt16(PublicDataClass._ComParam.DataBitTable[0]);
            com2databit.SelectedIndex  = Convert.ToUInt16(PublicDataClass._ComParam.DataBitTable[1]);
            com3databit.SelectedIndex  = Convert.ToUInt16(PublicDataClass._ComParam.DataBitTable[2]);
            com4databit.SelectedIndex  = Convert.ToUInt16(PublicDataClass._ComParam.DataBitTable[3]);

            com1protol.SelectedIndex   = 2;
            com2protol.SelectedIndex   = 2;
            com3protol.SelectedIndex   = 2;
            com4protol.SelectedIndex   = 2;
        }
        /// <summary>
        /// 串口1--4波特率的设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void com1baud_SelectedIndexChanged(object sender, EventArgs e)
        {
            PublicDataClass._ComParam.BaudRateTable[0] = Convert.ToString(com1baud.SelectedIndex);
        }

        private void com2baud_SelectedIndexChanged(object sender, EventArgs e)
        {
            PublicDataClass._ComParam.BaudRateTable[1] = Convert.ToString(com2baud.SelectedIndex);
        }

        private void com3baud_SelectedIndexChanged(object sender, EventArgs e)
        {
            PublicDataClass._ComParam.BaudRateTable[2] = Convert.ToString(com3baud.SelectedIndex);
        }

        private void com4baud_SelectedIndexChanged(object sender, EventArgs e)
        {
            PublicDataClass._ComParam.BaudRateTable[3] = Convert.ToString(com4baud.SelectedIndex);
        }
        /// <summary>
        /// 串口1--4 校验的设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void com1jy_SelectedIndexChanged(object sender, EventArgs e)
        {
            PublicDataClass._ComParam.JyTable[0] = Convert.ToString(com1jy.SelectedIndex);
        }

        private void com2jy_SelectedIndexChanged(object sender, EventArgs e)
        {
            PublicDataClass._ComParam.JyTable[1] = Convert.ToString(com2jy.SelectedIndex);
        }

        private void com3jy_SelectedIndexChanged(object sender, EventArgs e)
        {
            PublicDataClass._ComParam.JyTable[2] = Convert.ToString(com3jy.SelectedIndex);
        }

        private void com4jy_SelectedIndexChanged(object sender, EventArgs e)
        {
            PublicDataClass._ComParam.JyTable[3] = Convert.ToString(com4jy.SelectedIndex);
        }
        /// <summary>
        /// 串口1--4 数据位的设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void com1databit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PublicDataClass._ComParam.DataBitTable[0] = Convert.ToString(com1databit.SelectedIndex);
        }

        private void com2databit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PublicDataClass._ComParam.DataBitTable[1] = Convert.ToString(com2databit.SelectedIndex);
        }

        private void com3databit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PublicDataClass._ComParam.DataBitTable[2] = Convert.ToString(com3databit.SelectedIndex);
        }

        private void com4databit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PublicDataClass._ComParam.DataBitTable[3] = Convert.ToString(com4databit.SelectedIndex);
        }
    }
}
