using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FaTestSoft.CommonData;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;


namespace FaTestSoft
{
    public partial class monitormessagefm : Form
    {
        public monitormessagefm()
        {
            InitializeComponent();
        }
        static string Msg = @"";
        private static Thread WriteTextThread;
        static bool writeflag;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (PublicDataClass._MnitorParam.TcpLinkState == true)
            {
                PublicDataClass._MnitorParam.TcpLinkState = false;

                Msg = @"";
                Msg += DateTime.Now.ToString() + " ";
                if (PublicDataClass._MnitorParam.TcpLinkType == 0)
                    Msg += "监视端口 [" + PublicDataClass._MnitorParam.MnitorIP + "：" + PublicDataClass._MnitorParam.MnitorPort + "]已断开";
                else if (PublicDataClass._MnitorParam.TcpLinkType == 1)
                    Msg += "监视端口 [" + PublicDataClass._MnitorParam.MnitorIP + "：" + PublicDataClass._MnitorParam.MnitorPort + "]连接中...";
                else if (PublicDataClass._MnitorParam.TcpLinkType == 2)
                    Msg += "监视端口 [" + PublicDataClass._MnitorParam.MnitorIP + "：" + PublicDataClass._MnitorParam.MnitorPort + "]已接通";
                Msg += "\r\n";
                //richTextBoxExtended1.LinkMessage(Msg);
                textBox1.Text += Msg;

            }
             if (PublicDataClass._MnitorParam.NetShowSendMonitorData == true)
            {
                
                Msg = @"";
                for (int i = 0; i < PublicDataClass._MnitorParam.NetTLen; i++)
                {
                    Msg += Convert.ToChar(PublicDataClass._MnitorParam.NetTBuffer[i]);
                }
                Msg += "\r\n";
                textBox1.Text += Msg;
                writeflag = true;
                //string savename;
                //FileStream afile;
                //StreamWriter sw;
                //try
                //{
                //    savename = System.AppDomain.CurrentDomain.BaseDirectory;
                //    savename += "\\报文记录.txt";
                //    afile = new FileStream(savename, FileMode.Append);


                //    sw = new StreamWriter(afile);
                //    sw.Write(Msg);
                //    //afile.Dispose;
                //    //sw.Dispose;
                //    sw.Close();
                //    afile.Close();
                //}
                //catch
                //{
                //    //MessageBox.Show(ex.Message, "提示,保存失败!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
                PublicDataClass._MnitorParam.NetShowSendMonitorData = false;
            }
            else if (PublicDataClass._MnitorParam.NetShowRecvMonitorData == true)
            {
                
                Msg = @"";

                for (int i = 0; i < PublicDataClass._MnitorParam.NetRLen; )
                {
                    if (PublicDataClass._MnitorParam.NetRBuffer[i] < 128)
                    {
                        Msg += Convert.ToChar(PublicDataClass._MnitorParam.NetRBuffer[i]);
                        i++;
                    }
                    else
                    {
                        byte[] bytes = new byte[2];
                        bytes[0] = PublicDataClass._MnitorParam.NetRBuffer[i];
                        bytes[1] = PublicDataClass._MnitorParam.NetRBuffer[i+1];
                        string s = Encoding.Default.GetString(bytes);
                        Msg += s;
                        i += 2;
                    }
  
                }

               
                Msg += "\r\n";
                textBox1.Text += Msg;
                writeflag = true;
                //string savename;
                //FileStream afile;
                //StreamWriter sw;
                //try
                //{
                //    savename = System.AppDomain.CurrentDomain.BaseDirectory;
                //    savename += "\\报文记录.txt";
                //    afile = new FileStream(savename, FileMode.Append);


                //    sw = new StreamWriter(afile);
                //    sw.Write(Msg);
                //    //afile.Dispose;
                //    //sw.Dispose;
                //    sw.Close();
                //    afile.Close();
                //}
                //catch
                //{
                //    //MessageBox.Show(ex.Message, "提示,保存失败!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}

                PublicDataClass._MnitorParam.NetShowRecvMonitorData = false;
            }
        }

        private void monitormessagefm_Activated(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void monitormessagefm_Deactivate(object sender, EventArgs e)
        {
            //timer1.Enabled = false;
        }

        private void monitormessagefm_FormClosing(object sender, FormClosingEventArgs e)
        {
            PublicDataClass._MnitorParam.monitotclose = true;
            if (WriteTextThread != null)
                WriteTextThread.Abort();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') //这是允许输入0-9数字
            {
                PublicDataClass._MnitorParam.stop = true;
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text.Length>0)
            {
                textBox1.SelectionStart = textBox1.Text.Length;
                textBox1.SelectionLength = 0;
                textBox1.ScrollToCaret();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.Text += "\r\n";
            textBox1.Text += "\r\n";
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string savesysfilepath = @"";
            string savename;
            byte cn = 0;
            bool saveret;
            FileStream afile;
            StreamWriter sw;
            try
            {

                SaveFileDialog savefile = new SaveFileDialog();
                savefile.Filter = "*.txt|*.*";
                saveret = savefile.ShowDialog() == DialogResult.OK;

                if (saveret)
                {
                    savename = savefile.FileName;
                    savename += ".txt";
                    //savesysfilepath = savefile.
                    afile = new FileStream(savename, FileMode.Create);


                    sw = new StreamWriter(afile);
                    sw.Write(textBox1.Text);

                    sw.Close();
                    afile.Close();
                    saveret = false;

                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "提示,保存失败!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void monitormessagefm_Load(object sender, EventArgs e)
        {
           
            textBox1.Text="";
            textBox1.Text += "\r\n";
            textBox1.Text += "\r\n";
            WriteTextThread = new Thread(new ThreadStart(WriteTextThreadProc));
            WriteTextThread.Start();
        }
        private void WriteTextThreadProc()   //写报文记录线程
        {
            while (true)
            {
                if (writeflag == true)
                {
                    writeflag = false;
                    string savename;
                    FileStream afile;
                    StreamWriter sw;
                    try
                    {
                        savename = System.AppDomain.CurrentDomain.BaseDirectory;
                        savename += "\\报文记录.txt";
                        afile = new FileStream(savename, FileMode.Append);


                        sw = new StreamWriter(afile);
                        sw.Write(Msg);
                        //afile.Dispose;
                        //sw.Dispose;
                        sw.Close();
                        afile.Close();
                    }
                    catch
                    {
                        //MessageBox.Show(ex.Message, "提示,保存失败!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                Thread.Sleep(1);
            }
        }


     
    }
}
