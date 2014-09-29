using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using FaTestSoft.CommonData;  

namespace FaTestSoft
{
    public partial class monitor : Form
    {

        public monitor()
        {
            InitializeComponent();
        }

        private static Socket conn1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static Thread ComThread,ConnectThread1, NetSendThread1, NetRecvThread1;
        static string Msg = @"";
        static string sendstrold = @"";
        static string sendstrnew = @"";
        static int test = 0;
        bool NetLinkIsClose = false;
       
       

        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
            }
            else if(comboBox1.SelectedIndex == 1)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = false;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
            }
        }

        private void ConnThreadProc1()          //连接socket线程
        {

            while (true)
            {
                try
                {

                    conn1.Connect(IPAddress.Parse(PublicDataClass._MnitorParam.MnitorIP), Convert.ToInt16(PublicDataClass._MnitorParam.MnitorPort));
                    PublicDataClass._MnitorParam.TcpLinkType = 2;
                    PublicDataClass._MnitorParam.TcpLinkState = true;
                    break;

                }
                catch
                {

                    PublicDataClass._MnitorParam.TcpLinkType = 1;
                    PublicDataClass._MnitorParam.TcpLinkState = true;
                }
                Thread.Sleep(20000);   //20s
            }

        }
        //=============================================================================================       
             private void NetThreadSendProc1()   //网络发送数据线程
                {
                    
                    while (true)    //处理事物
                    {
                        PtlNetFrameProc1(); // 是否有发送任务处理
                        if (PublicDataClass._MnitorParam.TX_TASK)
                        {
                            PublicDataClass._MnitorParam.TX_TASK = false;

                            try
                            {
                                conn1.Send(PublicDataClass._MnitorParam.NetTBuffer, 0, PublicDataClass._MnitorParam.NetTLen, 0);
                                PublicDataClass._MnitorParam.NetShowSendMonitorData = true;
                            }
                            catch
                            {
                                if (PublicDataClass._MnitorParam.TcpLinkType != 0)
                                {
                                    PublicDataClass._MnitorParam.TcpLinkType = 0;
                                    PublicDataClass._MnitorParam.TcpLinkState = true;
                                    NetLinkIsClose = true;
                                }

                            }

                            
                        }
                        Thread.Sleep(1);


                    }


                }
    //=====================================================================================

    public static void PtlNetFrameProc1()  //---------------------------------------------网络组包
    {
        //int count;
        //char[] char1;
        //byte[] down;
       
        if (PublicDataClass._MnitorParam.start == true)
        {
            PublicDataClass._MnitorParam.start = false;
            int count = sendstrnew.Length;
             //char1 = new char[count];
             char[] char1 = sendstrnew.ToCharArray(0, count);
             byte[] down = new byte[count];
             if (count <= 9)
                 return;

            for (int i = 0; i < count; i++)
            {

                down[i] = (byte)(char1[i]);
                PublicDataClass._MnitorParam.NetTBuffer[i] = down[i];
                
            }
            PublicDataClass._MnitorParam.NetTLen = count;
            PublicDataClass._MnitorParam.TX_TASK = true;
        }
         if (PublicDataClass._MnitorParam.stop == true)
        {
            PublicDataClass._MnitorParam.stop = false;

            sendstrnew = @"";
            sendstrnew += "\r\n";

            int count = sendstrnew.Length;
            //char1 = new char[count];
            char[] char1 = sendstrnew.ToCharArray(0, count);
            byte[] down = new byte[count];
            for (int i = 0; i < count; i++)
            {

                down[i] = (byte)(char1[i]);
                PublicDataClass._MnitorParam.NetTBuffer[i] = down[i];

            }
            PublicDataClass._MnitorParam.NetTLen = count;
            PublicDataClass._MnitorParam.TX_TASK = true;
        }
         if (PublicDataClass._MnitorParam.test == true)
         {
             PublicDataClass._MnitorParam.test= false;

             sendstrnew = @"";
             sendstrnew += "test";

             int count = sendstrnew.Length;
             //char1 = new char[count];
             char[] char1 = sendstrnew.ToCharArray(0, count);
             byte[] down = new byte[count];
             for (int i = 0; i < count; i++)
             {

                 down[i] = (byte)(char1[i]);
                 PublicDataClass._MnitorParam.NetTBuffer[i] = down[i];

             }
             PublicDataClass._MnitorParam.NetTLen = count;
             PublicDataClass._MnitorParam.TX_TASK = true;
         }
           
    }
           
    


    //=====================================================================================

        
        
        private void ReceiveNetMsg1()   //---------------------------------------------网络解包线程
        {
            while (true)
            {
                try
                {
                    if (PublicDataClass._MnitorParam.NetShowRecvMonitorData == false)
                    {
                        PublicDataClass._MnitorParam.NetRLen = conn1.Receive(PublicDataClass._MnitorParam.NetRBuffer);
                        if (PublicDataClass._MnitorParam.NetRLen > 0)
                        {
                            PublicDataClass._MnitorParam.NetShowRecvMonitorData = true;

                        }
                        else
                        {
                            if (PublicDataClass._MnitorParam.TcpLinkType != 0)
                            {
                                PublicDataClass._MnitorParam.TcpLinkType = 0;
                                PublicDataClass._MnitorParam.TcpLinkState = true;
                                NetLinkIsClose = true;
                            }

                        }
                    }
                  

                }
                catch
                {
                    if (PublicDataClass._MnitorParam.TcpLinkType != 0)
                    {
                        PublicDataClass._MnitorParam.TcpLinkType = 0;
                        PublicDataClass._MnitorParam.TcpLinkState = true;
                        
                    }

                }
                Thread.Sleep(1);

            }

        }
  

        private void monitor_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "NET")
            {

                if (textBox1.Text == "")
                    MessageBox.Show("请输入监视通道端口号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    PublicDataClass._MnitorParam.MnitorPort = textBox1.Text;


                if (textBox2.Text == "")
                    MessageBox.Show("请输入监视通道IP！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    PublicDataClass._MnitorParam.MnitorIP = textBox2.Text;

                conn1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
               
               
                    ConnectThread1 = new Thread(new ThreadStart(ConnThreadProc1));
                    ConnectThread1.Start();
               

                
                    NetSendThread1 = new Thread(new ThreadStart(NetThreadSendProc1));
                    NetSendThread1.Start();
                
                    NetRecvThread1 = new Thread(new ThreadStart(ReceiveNetMsg1));
                    NetRecvThread1.Start();
                

                if (PublicDataClass._MnitorParam.monitotclose == true)
                {
                    monitormessagefm moniFm = new monitormessagefm();
                    moniFm.Show();
                    PublicDataClass._MnitorParam.monitotclose = false;
                    timerclose.Enabled = true;
                    button1.Enabled = false;
                }

            }
            timer1.Enabled = true;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            sendstrnew = @"monitor";
            if (checkBox3.Checked == true)
                sendstrnew += " " + "com 1";
            if (checkBox4.Checked == true)
                sendstrnew += " " + "com 2";
            if (checkBox5.Checked == true)
                sendstrnew += " " + "com 3";
            if (checkBox6.Checked == true)
                sendstrnew += " " + "com 4";
            if (checkBox7.Checked == true)
                sendstrnew += " " + "com 5";
            if (checkBox8.Checked == true)
                sendstrnew += " " + "com 6";
            if (checkBox9.Checked == true)
                sendstrnew += " " + "com 7";
            if (checkBox10.Checked == true)
                sendstrnew += " " + "com 8";

            if (checkBox11.Checked == true)
                sendstrnew += " " + "net 1 sock 0";
            if (checkBox12.Checked == true)
                sendstrnew += " " + "net 1 sock 1";
            if (checkBox13.Checked == true)
                sendstrnew += " " + "net 1 sock 2";
            if (checkBox14.Checked == true)
                sendstrnew += " " + "net 1 sock 3";
            if (checkBox15.Checked == true)
                sendstrnew += " " + "net 2 sock 0";
            if (checkBox16.Checked == true)
                sendstrnew += " " + "net 2 sock 1";
            if (checkBox17.Checked == true)
                sendstrnew += " " + "net 2 sock 2";
            if (checkBox18.Checked == true)
                sendstrnew += " " + "net 2 sock 3";
            sendstrnew  += "\r\n";
            PublicDataClass._MnitorParam.start = true;
            
        }

    

        private void timerclose_Tick(object sender, EventArgs e)
        {
            //if ((PublicDataClass._MnitorParam.TcpLinkType == 0)&&(button1.Enabled = false))
            //    button1.Enabled = true;
            if (PublicDataClass._MnitorParam.monitotclose == true)
            {
                if (NetSendThread1 != null)
                    NetSendThread1.Abort();
                if (NetRecvThread1 != null)
                    NetRecvThread1.Abort();
                if (ConnectThread1 != null)
                    ConnectThread1.Abort();
                conn1.Close();
                PublicDataClass._MnitorParam.TcpLinkType = 0;
                PublicDataClass._MnitorParam.TcpLinkState = true;
                timerclose.Enabled = false;
                button1.Enabled = true;
            }
            if (NetLinkIsClose == true)
            {
                NetLinkIsClose = false;
                if (ConnectThread1 != null)
                    ConnectThread1.Abort();
                //conn1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ConnectThread1 = new Thread(new ThreadStart(ConnThreadProc1));
                ConnectThread1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            test++;
            if (test >= 60)
            {
                test=0;
                PublicDataClass._MnitorParam.test = true;
            }
        }

  

      

       
    }
}
