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
    public partial class TransmitDataForm : Form
    {
        public TransmitDataForm()
        {
            InitializeComponent();
        }
        string Sendmes = @"";

        private void button1_Click(object sender, EventArgs e)
        {

            Sendmes = richTextBox1.Text;
            try
            {
                if (radioButton4.Checked == true)
                {
                    PublicDataClass._DataField.Buffer = strToToHexByte(Sendmes);
                    PublicDataClass._DataField.FieldLen = PublicDataClass._DataField.Buffer.Length;
                }
                else if (radioButton5.Checked == true)
                {
                    PublicDataClass._DataField.Buffer = strToAsc(Sendmes);
                    PublicDataClass._DataField.FieldLen = PublicDataClass._DataField.Buffer.Length;
                }
            }
            catch 
            {
            }

            if (radioButton9.Checked == true)
                PublicDataClass.ParamInfoAddr=0x01;
            else if (radioButton10.Checked == true)
                PublicDataClass.ParamInfoAddr = 0x02;
            else if (radioButton11.Checked == true)
                PublicDataClass.ParamInfoAddr = 0x03;

            if (radioButton1.Checked == true)
                PublicDataClass._ComTaskFlag.Transmit = true;
            else if (radioButton2.Checked == true)
                PublicDataClass._NetTaskFlag.Transmit = true;
        }

        private void TransmitDataForm_Load(object sender, EventArgs e)
        {

        }

        private static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            hexString = hexString.Replace("\r", "");
            hexString = hexString.Replace("\n", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        private static byte[] strToAsc(string AscString)
        {
            AscString = AscString.Replace(" ", "");
            int count = AscString.Length;
            char[] char1 = AscString.ToCharArray(0, count);
            byte[] returnBytes = new byte[count];
            for (int i = 0; i < count; i++)
                returnBytes[i] = (byte)(char1[i]);
            return returnBytes;
        }
    }
}
