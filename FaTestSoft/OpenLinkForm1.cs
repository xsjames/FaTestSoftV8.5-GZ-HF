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
    public partial class OpenLinkForm1 : Form
    {
        public OpenLinkForm1()
        {
            InitializeComponent();
        }

        private void OpenLinkForm1_Load(object sender, EventArgs e)
        {
            if (PublicDataClass.SaveText.devicenum > 0)
                textBox1.Text = PublicDataClass.SaveText.Device[0].PointName;
            if (PublicDataClass.SaveText.channelnum > 0)
            {
                //comboBox1.SelectedIndex = 0;
                comboBox1.Text = PublicDataClass.SaveText.Channel[0].ChannelID; 
                comboBox2 .Text= PublicDataClass.SaveText.Channel[0].baud;
                comboBox3.Text = PublicDataClass.SaveText.Channel[0].jy;
                textBox2.Text = PublicDataClass.SaveText.Channel[0].IP;
                textBox3.Text = PublicDataClass.SaveText.Channel[0].port;
                textBox4.Text = PublicDataClass.SaveText.Channel[0].RelayTime;
                textBox5.Text = Convert.ToString( PublicDataClass.cotlen);
                textBox6.Text = Convert.ToString(PublicDataClass.publen);
                textBox7.Text = Convert.ToString(PublicDataClass.inflen);
                if (PublicDataClass.SaveText.Channel[0].ChannelID == "NET")
                {
                    
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    textBox2.Enabled = true;
                    textBox3.Enabled = true;
                    textBox4.Enabled = true;
                    textBox5.Enabled = true;
                    textBox6.Enabled = true;
                    textBox7.Enabled = true;

                }

                else if (PublicDataClass.SaveText.Channel[0].ChannelID == "GPRS")
                {
                    
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    textBox2.Enabled = true;
                    textBox3.Enabled = true;
                    textBox4.Enabled = true;
                    textBox5.Enabled = false;
                    textBox6.Enabled = false;
                    textBox7.Enabled = false;
                }   
                else
                {
                    
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    comboBox3.Enabled = true;
                    textBox2.Enabled = false;
                    textBox3.Enabled = false;
                    textBox4.Enabled = true;
                    textBox5.Enabled = true;
                    textBox6.Enabled = true;
                    textBox7.Enabled = true;
                }

            }
            //else
            //{
            //    comboBox1.SelectedIndex = 0;
            //    comboBox2.SelectedIndex = 0;
            //    comboBox3.SelectedIndex = 2;
            //}
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            //通道
                PublicDataClass.SaveText.channelnum = 1;
                PublicDataClass.SaveText.Channel = new PublicDataClass.CHANNELINFO[PublicDataClass.SaveText.channelnum];
                
                PublicDataClass.SaveText.Channel[0].ChannelID = comboBox1.Text;
                PublicDataClass.SaveText.Channel[0].baud = comboBox2.Text;
                PublicDataClass.SaveText.Channel[0].jy = comboBox3.Text;
                PublicDataClass.SaveText.Channel[0].port = textBox3.Text;
                PublicDataClass.SaveText.Channel[0].IP = textBox2.Text;
                PublicDataClass.SaveText.Channel[0].RelayTime = textBox4.Text;
                PublicDataClass.cotlen = Convert.ToInt32(textBox5.Text);
                PublicDataClass.publen = Convert.ToInt32(textBox6.Text);
                PublicDataClass.linklen = Convert.ToInt32(textBox7.Text);
                PublicDataClass.inflen  = 2;
            
            PublicDataClass._OpenLinkState.Linknum = 1;
            PublicDataClass._OpenLinkState.LinkDevName = new string[PublicDataClass._OpenLinkState.Linknum];
            for (byte i = 0; i < PublicDataClass._OpenLinkState.Linknum; i++)
            {
                PublicDataClass._OpenLinkState.LinkDevName[i] = comboBox1.Text;

            }

            //设备
            PublicDataClass.SaveText.devicenum = 1;
            PublicDataClass.SaveText.Device = new PublicDataClass.DEVICEINFO[PublicDataClass.SaveText.devicenum];
            PublicDataClass.SaveText.cfgnum = PublicDataClass.SaveText.devicenum;  //配置了一个测量点


            PublicDataClass.SaveText.Device[0].PointName = textBox1.Text;

            PublicDataClass.SaveText.Device[0].ChannelID = comboBox1.Text;

            PublicDataClass.SaveText.Device[0].Addr = Convert.ToString(PublicDataClass.DevAddr);
       
            PublicDataClass.SaveText.Device[0].State = "是";


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((comboBox1.SelectedIndex == 0) || (comboBox1.SelectedIndex == 1))
            {
                comboBox1.Enabled = true;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox5.Enabled = true;
                textBox6.Enabled = true;
                textBox7.Enabled = true;

            }
            else
            {
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox5.Enabled = true;
                textBox6.Enabled = true;
                textBox7.Enabled = true;


            }
        }
    }
}
