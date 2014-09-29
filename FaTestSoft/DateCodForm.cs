using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using FaTestSoft.CommonData;
using FaTestSoft.FUNCTIONCLASS;

namespace FaTestSoft
{
    public partial class DateCodForm : Form
    {
        public DateCodForm()
        {
            InitializeComponent();
        }
        OpenFileDialog OpenF = new OpenFileDialog();
           byte[] buffer = new byte[68];
            static byte DownloadType = 0;//下载类型
            int codeleng = 0;//key=68,sid=16
            static bool ShowTimeFlg = false;
          
        private void buttonselfile_Click(object sender, EventArgs e)
        {
            FileStream afilebin;
            BinaryReader sr;
           
            OpenF.Filter = "KEY文件(*.key)|*.key|所有文件(*.*)|*.*";
            OpenF.InitialDirectory = System.Environment.CurrentDirectory;
            if (OpenF.ShowDialog() == DialogResult.OK)
            {
                string filename = System.IO.Path.GetFileName(OpenF.FileName);
                int a = filename.IndexOf(".");
                string strResult = filename.Substring(filename.Length - 3);
                string keyname = filename.Substring(0, a);
                 if (strResult != "key")
                {
                    MessageBox.Show("文件名不合法！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                 if (keyname == "sig_id")
                 {
                     codeleng = 16;
                     DownloadType = 4;
                     filelabel.Text = keyname + "\r\n";
                     afilebin = new FileStream(OpenF.FileName, FileMode.Open, FileAccess.Read);
                     sr = new BinaryReader(afilebin);
                     try
                     {
                         buffer = sr.ReadBytes(codeleng);
                         //修改配置文件内容
                          for (int k = 0; k < PublicDataClass.XIEFILENAME.Length; k++)
                            {
                                if (PublicDataClass.XieTabCfg[k].PageName == "协处理器SID")
                                {
                                    for (int i = 0; i < codeleng; i++)
                                    {
                                        PublicDataClass.XieTabCfg[k].TabPageValue[1].ValueTable[i] = String.Format("{0:d}", buffer[i]);
                                      
                                    }
                                }
                        
                          }

                          for (int i = 0; i < codeleng; i++)
                         {
                             filelabel.Text += String.Format("{0:d}", buffer[i]) + " ";
                             //filelabel.Text += String.Format("{0:x}", buffer[i]) + " ";
                         }
                         filelabel.Text += "\r\n";
                     }
                     catch
                     { 
                     }
                 }
                 else //key
                 {
                     codeleng = 68;
                     if (filename.IndexOf("key1")>=0)
                         DownloadType = 0;
                     if (filename.IndexOf("key2") >= 0)
                         DownloadType = 1;
                     if (filename.IndexOf("key3") >= 0)
                         DownloadType = 2;
                     if (filename.IndexOf("key4") >= 0)
                         DownloadType = 3;

                     filelabel.Text = keyname + "\r\n";
                     afilebin = new FileStream(OpenF.FileName, FileMode.Open, FileAccess.Read);
                     sr = new BinaryReader(afilebin);
                     try
                     {
                         buffer = sr.ReadBytes(codeleng);

                         //修改配置文件内容
                         for (int k = 0; k < PublicDataClass.XIEFILENAME.Length; k++)
                         {
                            switch (DownloadType)
                            {
                                case 0:
                             if (PublicDataClass.XieTabCfg[k].PageName.IndexOf("key1")>=0)
                             {
                                 for (int i = 0; i < codeleng; i++)
                                 {
                                     PublicDataClass.XieTabCfg[k].TabPageValue[1].ValueTable[i] = String.Format("{0:d}", buffer[i]);

                                 }
                             }
                             break;
                                case 1:
                             if (PublicDataClass.XieTabCfg[k].PageName.IndexOf("key2") >= 0)
                             {
                                 for (int i = 0; i < codeleng; i++)
                                 {
                                     PublicDataClass.XieTabCfg[k].TabPageValue[1].ValueTable[i] = String.Format("{0:d}", buffer[i]);

                                 }
                             }
                             break;
                                case 2:
                             if (PublicDataClass.XieTabCfg[k].PageName.IndexOf("key3") >= 0)
                             {
                                 for (int i = 0; i < codeleng; i++)
                                 {
                                     PublicDataClass.XieTabCfg[k].TabPageValue[1].ValueTable[i] = String.Format("{0:d}", buffer[i]);

                                 }
                             }
                             break;
                                case 3:
                             if (PublicDataClass.XieTabCfg[k].PageName.IndexOf("key4") >= 0)
                             {
                                 for (int i = 0; i < codeleng; i++)
                                 {
                                     PublicDataClass.XieTabCfg[k].TabPageValue[1].ValueTable[i] = String.Format("{0:d}", buffer[i]);

                                 }
                             }
                             break;
                                default:
                             break;
                         }

                         }

                         for (int i = 0; i < 4; i++)
                         {
                           // filelabel.Text += String.Format("{0:d}", buffer[i]) + " ";
                              filelabel.Text += String.Format("{0:x2}", buffer[i]) + " ";
                         }
                         filelabel.Text += "\r\n";
                         for (int i = 0; i <16; i++)
                         {
                        //   filelabel.Text += String.Format("{0:d}", buffer[i+4]) + " ";
   
                               filelabel.Text += String.Format("{0:x2}", buffer[i + 4]) + " ";
                         }
                         filelabel.Text += "\r\n";
                         for (int i = 0; i < 16; i++)
                         {

                              filelabel.Text += String.Format("{0:x2}", buffer[i + 20]) + " ";
                            //filelabel.Text += String.Format("{0:d}", buffer[i + 20]) + " ";
                         }
                         filelabel.Text += "\r\n";
                         for (int i = 0; i < 16; i++)
                         {

                             filelabel.Text += String.Format("{0:x2}", buffer[i + 36]) + " ";
                            // filelabel.Text += String.Format("{0:d}", buffer[i + 36]) + " ";
                         }
                         filelabel.Text += "\r\n";
                         for (int i = 0; i < 16; i++)
                         {

                            filelabel.Text += String.Format("{0:x2}", buffer[i + 52]) + " ";
                             // filelabel.Text += String.Format("{0:d}", buffer[i + 52]) + " ";
                         }
                         filelabel.Text += "\r\n";

                         afilebin.Close();
                         sr.Close();
                     }
                     catch
                     {
                     }
                 }
            }
        }
        public static int EncodeFrame(byte dataty)
        {
            int Leng = 0;
            byte FCodebyte = 0;//功能码：0x55表示下载秘钥，0xAA表示查看参数
           // 秘钥：0-3表示key0-3,4表示sid；参数：0表示查看协处理器时间，1表示查看近一次秘钥下载时间
            if (dataty == 1)//下载密钥
            {
                FCodebyte = 0x55;
             
            }
            if (dataty == 2)//查看参数
            {
                FCodebyte = 0xAA;

            }
            PublicDataClass._NetStructData.NetTBuffer[0] = 0x16;
            PublicDataClass._NetStructData.NetTBuffer[3] = 0x16;
            PublicDataClass._NetStructData.NetTBuffer[4] = FCodebyte;
            PublicDataClass._NetStructData.NetTBuffer[5] = FCodebyte;
            PublicDataClass._NetStructData.NetTBuffer[6] = DownloadType;
            PublicDataClass._NetStructData.NetTBuffer[7] = 0x00;//备用
            PublicDataClass._NetStructData.NetTBuffer[8] = 0x00;//备用
            PublicDataClass._NetStructData.NetTBuffer[9] = 0x00;///备用
            Leng = 10;
            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];

            PublicDataClass._NetStructData.NetTBuffer[1] = (byte)((Leng - 4) & 0x00ff);
            //PublicDataClass._NetStructData.NetTBuffer[2] = (byte)(((Leng - 4) & 0xff00) >> 8);
            PublicDataClass._NetStructData.NetTBuffer[2] = 0x00;
            PublicDataClass._NetStructData.NetTBuffer[Leng] = GetSumCheck(PublicDataClass._NetStructData.NetTBuffer, 4, (Leng - 4));	//	校验和
            PublicDataClass._NetStructData.NetTBuffer[Leng + 1] = 0x16;
            Leng += 2;
            PublicDataClass.OutPutMessage.S_type = 0;
            return Leng;
            }
        public static byte DecodeFrame()
        { 
             
                int Len;
                byte AFN = 0;
                byte dataty = 0;
                if ((PublicDataClass._NetStructData.NetRBuffer[0] != 0x16) || (PublicDataClass._NetStructData.NetRBuffer[3] != 0x16) || (PublicDataClass._NetStructData.NetRBuffer[PublicDataClass._NetStructData.NetRLen - 1] != 0x16) || (PublicDataClass._NetStructData.NetRLen < 12))  //非法帧
                    dataty = 0;
                else if (GetSumCheck(PublicDataClass._NetStructData.NetRBuffer, 4, PublicDataClass._NetStructData.NetRLen - 6) != PublicDataClass._NetStructData.NetRBuffer[PublicDataClass._NetStructData.NetRLen - 2])
                    dataty = 0;
                else if(PublicDataClass._NetStructData.NetRBuffer[4]!=0xaa||PublicDataClass._NetStructData.NetRBuffer[5]!=0xaa)
                    dataty = 0;
                else if ((PublicDataClass._NetStructData.NetRLen - 6) != PublicDataClass._NetStructData.NetRBuffer[1])
                    dataty = 0;
                else
                {
                    //Len = (PublicDataClass._NetStructData.NetRBuffer[2] << 8) + PublicDataClass._NetStructData.NetRBuffer[1];
                    Len =PublicDataClass._NetStructData.NetRBuffer[1];
                    AFN = PublicDataClass._NetStructData.NetRBuffer[6];
                    switch (AFN)
                    {
                        case 0:                         //0表示查看协处理器时间
                            for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 10];
                            PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                            PublicDataClass._DataField.FieldVSQ = 1;
                            dataty = 120;    //器件状态回复
                            PublicDataClass.RevNetFrameMsg = "协处理器时间";
                            break;
                        case 1:   //1表示查看近一次秘钥下载时间
                            for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 10];
                            PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                            PublicDataClass._DataField.FieldVSQ = 5;
          
                            dataty = 121;    //
                            PublicDataClass.RevNetFrameMsg = "近一次秘钥下载时间";

                            break;
                    }
                   ShowTimeFlg = true ;
                }
               
                return dataty;
        
        }
        public static byte GetSumCheck(byte[] buf, int start, int len)
        {
            byte byTempSum = 0;

            for (int j = 0; j < len; j++)
            {
                byTempSum += buf[start + j];
            }


            return byTempSum;
        }
        private void button10_Click(object sender, EventArgs e)
        {
         
            for (int i = 0; i < codeleng; i++)
                {
                    PublicDataClass._DataField.Buffer[i] = buffer[i];  //代码赋值
                }
            PublicDataClass._DataField.FieldLen = codeleng;

            if(DownloadType==0)
            {
                PublicDataClass.ParamInfoAddr = 301;//key0
                 PublicDataClass._DataField.FieldVSQ=68;
            }
           else  if (DownloadType == 1)
            {
                PublicDataClass.ParamInfoAddr = 302;//key1
                    PublicDataClass._DataField.FieldVSQ=68;
           }
            else if (DownloadType == 2)
            {
                PublicDataClass.ParamInfoAddr = 303;//key1
                     PublicDataClass._DataField.FieldVSQ=68;
            }
            else if (DownloadType == 3)
            {
                PublicDataClass.ParamInfoAddr = 304;//key1
                PublicDataClass._DataField.FieldVSQ = 68;
            }
            else if (DownloadType == 4)
            {
                PublicDataClass.ParamInfoAddr = 305;//sid
                PublicDataClass._DataField.FieldVSQ = 16;
            }
            PublicDataClass.addselect = 3;//协处理器

  
            PublicDataClass._NetTaskFlag.ARMX_DOWN = true;
            //EncodeFrame(1);
            //PublicDataClass._NetStructData.TX_TASK = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DownloadType = 0;
            PublicDataClass._DataField.FieldLen = 0;
            PublicDataClass._NetStructData.NetTLen = EncodeFrame(2);
            PublicDataClass.SedNetFrameMsg = "查看协处理器时间";
            PublicDataClass._NetStructData.TX_TASK = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DownloadType = 1;
            PublicDataClass._DataField.FieldLen = 0;
            PublicDataClass._NetStructData.NetTLen = EncodeFrame(2);
            PublicDataClass.SedNetFrameMsg = "查看近一次秘钥下载时间";
            PublicDataClass._NetStructData.TX_TASK = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (PublicDataClass._WindowsVisable.DateCodFormVisable == true)  //窗体可见
            {
                if (ShowTimeFlg == true)
                {
                    ShowTimeFlg = false;
                    showtimeInfo();
                }
            }
        }
        private void showtimeInfo()
        {
            string str = @"";
            
            for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
            {
               if(PublicDataClass._DataField.FieldVSQ==1)
             {
              str +="协处理器时间： ";
              }
                else 
                {
                    if (j == PublicDataClass._DataField.FieldVSQ-1)
                        str += "sid： ";
                   else 
                    str +="key"+String.Format("{0:d}", j+1)+"： ";
                  }
                str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[j*12] + (PublicDataClass._DataField.Buffer[j*12+1] << 8));
                str += "年";
                str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[j*12+2] +( PublicDataClass._DataField.Buffer[j*12+3] << 8));
                str += "月";
                str += String.Format("{0:d}",PublicDataClass._DataField.Buffer[j*12+4] + (PublicDataClass._DataField.Buffer[j*12+5] << 8));  //日+星期
                str += "日";
                str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[j*12+6] + (PublicDataClass._DataField.Buffer[j*12+7] << 8));
                str += "时";
                str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[j*12+8] +( PublicDataClass._DataField.Buffer[j*12+9] << 8));
                str += "分";
                str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[j*12+10] +( PublicDataClass._DataField.Buffer[j*12+11] << 8));
                str += "秒";
                  str +="\r\n";
            }

            timelabel.Text =str;
        }

        private void DateCodForm_Activated(object sender, EventArgs e)
        {
            PublicDataClass._WindowsVisable.DateCodFormVisable = true;            //窗体可见
            //timer1.Enabled = true;
        }

        private void DateCodForm_Deactivate(object sender, EventArgs e)
        {
            //PublicDataClass._WindowsVisable.DateCodFormVisable = false;        //窗体不可见
            //timer1.Enabled = false;
        }
    }
}
