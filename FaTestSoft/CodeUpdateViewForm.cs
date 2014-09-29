using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;                       //读写文件所在的命名空间
using System.Windows.Forms;
using FaTestSoft.CommonData;
using FaTestSoft.FUNCTIONCLASS;

namespace FaTestSoft
{
    public partial class CodeUpdateViewForm : Form
    {
        public CodeUpdateViewForm()
        {
            InitializeComponent();
        }

        public struct COUNT_UPDATE_BYTE
        {
            public  long count_byte;    //长度
            public  byte order;   //序号

        }
        public struct UpdeCode
        {
            public static COUNT_UPDATE_BYTE[] Info = new COUNT_UPDATE_BYTE[16];

        }
        public struct UPCODE_SEG
        {
            public int PageAdd;
            public int AddInPage;
            public int Codelen;
	        public byte[] Up_Code ;
        }

        public struct SEG_REG
        {
	        public int  firstadd;               //起始地址
            public int fs_segnum;              //起始块
            public int ed_segnum;              //结束块
            public int fs_Page;                //起始页
            public int PageNum;                //存储页数
        }

        public struct UPDATE_CODE_REG
        {
	        public static  SEG_REG []Seg_Reg = new SEG_REG[16];
	        public static  UPCODE_SEG [] UpCode_Seg =new UPCODE_SEG[1000];
            public static byte DD_Num;        //任务个数
	        public static byte Checksum;      //校验和
            public static short  ARMChecksum;      //ARM校验和
            
            public static int  SegNum;        //块数
        }

        public struct Info
        {
            public int blockLen;  //块长度
            public int blockAddr; //块地址
            public int blockSumSeg;   //块包含多少段
            public int BeginSeg;
            public int EndSeg;         //结束段
            public int Count;
        }
        public struct BLOCKINFO
        {
            public static Info []BlockInfo =new Info[20];
        }
        public struct CODEBYTE
        {
            public byte[] FileBytes;
            public int Len;

        }
        public struct FILECODE
        {
            public static CODEBYTE[] CodeByte = new CODEBYTE[1024];  //512->1024

        }

        public struct FileDownInfo
        {  
            //long FileLenOfBytes;
            //WORD FileSegmentNum;
            //WORD LastSegmentBytes;
            public static int StartSeg;
            public static int DownSegNo;
            public static bool IsDownDoing;
            


        };


  
        OpenFileDialog OpenF = new OpenFileDialog();
      
        string strdata;
        static bool bFileIsOk = false;
        static int iDownTime = 0;
        static int segnum = 0;
        static int filetype = 0;//文件类型:      1: 430文件   2：2812文件   4:ARM32

        int timer2time = 0;
        static bool FirstDownDoing = false;    //开始下载标志 
        private int ty;
        byte count_codeseg    = 0;
        char[] code = new char[66];
        byte[] datacode = new byte[4];

        private void CodeUpdateViewForm_SizeChanged(object sender, EventArgs e)
        {
           /* groupBox1.Width  = splitContainer1.Width;
            groupBox1.Height = splitContainer1.Height /3;

            groupBox1.Top = splitContainer1.Panel1.Location.X;
            groupBox1.Left = 0;

            groupBox2.Width = splitContainer1.Width;
            groupBox2.Height = splitContainer1.Height - groupBox1.Height;

            groupBox2.Top = splitContainer1.Panel1.Location.X + groupBox1.Height;
            groupBox2.Left = 0;*/
        }

        private void CodeUpdateViewForm_Load(object sender, EventArgs e)
        {
            
            //comboBoxaddr.Items.Clear();
            //comboBoxaddr.Items.Add("主 板");
            //comboBoxaddr.Items.Add("从板1");
            //comboBoxaddr.Items.Add("从板2");
            //comboBoxaddr.Items.Add("从板3");
            //comboBoxaddr.Items.Add("广 播");
         
            //comboBoxaddr.SelectedIndex = 0;
  
            //checkBox1.Checked = true;
            if (PublicDataClass.LinSPointName == "无信息")
            {
                MessageBox.Show("无测量点信息可操作", "信息",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            this.progressBar1.Value = 0;          //清空进度条
            textBoxloadstate.Enabled = false;
            
            
        }
        /********************************************************************************************
        *  函数名：    buttonselfile_Click                                                          *
        *  功能  ：    打开文件事件处理函数                                                         *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2010-12-07                                                                   *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void buttonselfile_Click(object sender, EventArgs e)
        {
            OpenF.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            OpenF.InitialDirectory = System.Environment.CurrentDirectory;
            filename.Text         = "";
            textBoxloadstate.Text = "";
            PublicDataClass._FileHeadInfo.FileLen = 0;
            textBoxloadstate.Text                 = "";
            iDownTime                             = 0;

            FileStream afile;
            FileStream afilebin;
            FileStream afiletxt;
            StreamReader sr;
          
            
            byte filety = 0;       //文件类型
            if (OpenF.ShowDialog() == DialogResult.OK)
            {
                filename.Text = OpenF.FileName;           //找到文件名
               
                string strResult = OpenF.FileName.Substring(OpenF.FileName.Length - 3);

                if (strResult=="bin")
                {
                  
                  //  string directory = System.Environment.CurrentDirectory;
                  //  string bintotxtname = directory + "\\bintotxt.txt";
                  //  afilebin = new FileStream(OpenF.FileName, FileMode.Open, FileAccess.Read);
                  //  if (afilebin == null)
                  //  {
                  //      MessageBox.Show("文件不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  //      return;
                  //  }
                  ////  sr = new StreamReader(afilebin);
                  //  afiletxt = new FileStream(bintotxtname, FileMode.Create, FileAccess.Write);//创建写入文件
                  ////  sw = new StreamReader(afiletxt);
                  //  BintoTxt(afilebin, afiletxt);
                  //  OpenF.FileName = bintotxtname;
                    filety = 4;
                 }
         

                //CFile file(sFileName,CFile::modeRead);
                    afile = new FileStream(OpenF.FileName, FileMode.Open, FileAccess.Read);
                
                    if (afile == null)
                   {
                    MessageBox.Show("文件不存在！", "提示",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;  
                   }
               
                sr = new StreamReader(afile,Encoding.ASCII);

                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                if (filety!=4)
                filety = CheckSelFileType(sr);
                if (filety == 1)                    //430文件
                {
                    filetype = 1;//文件类型430文件
                    bFileIsOk = true;
                    
                    afile = new FileStream(OpenF.FileName, FileMode.Open, FileAccess.Read);
                 
                    sr = new StreamReader(afile, Encoding.ASCII);

                    sr.BaseStream.Seek(0, SeekOrigin.Begin);
                    counter_update_txt(sr);

                    afile = new FileStream(OpenF.FileName, FileMode.Open, FileAccess.Read);
                    for (int i = 0; i < 360; i++)
                    {
                        UPDATE_CODE_REG.UpCode_Seg[i].Up_Code = new byte[512];


                    }
                    Txt430_Code(afile);
                    filename.Text += "\r\n";
                    filename.Text += "\r\n" + String.Format("校验和    :{0:x}", UPDATE_CODE_REG.Checksum);
                    filename.Text += "\r\n";
                    filename.Text += "\r\n" + String.Format("总块数    :{0:d}", UPDATE_CODE_REG.DD_Num);
                    filename.Text += "\r\n";
                    filename.Text += "\r\n" + String.Format("下载总段数:{0:d}", UPDATE_CODE_REG.SegNum);
                    textBoxendseg.Text = Convert.ToString(UPDATE_CODE_REG.SegNum);
                   
                }
                else if (filety == 2)                //2812的文件
                {
                     filetype = 2;//文件类型2812的文件
                     bFileIsOk = true;

                     afile = new FileStream(OpenF.FileName, FileMode.Open, FileAccess.Read);
                     for (int i = 0; i < 1024; i++)   //512->1024
                     {
                         FILECODE.CodeByte[i].FileBytes = new byte[PublicDataClass.CodeUpdatalen];                       //分配内存
                         FILECODE.CodeByte[i].Len       = 0;
                     }

                     DspTxt_Code(afile);
                     filename.Text += "\r\n";
                     filename.Text += "\r\n" + String.Format("校验和    :{0:x}", PublicDataClass._FileHeadInfo.CheckSum);
                     filename.Text += "\r\n";
                     filename.Text += "\r\n" + String.Format("总块数    :{0:d}", PublicDataClass._FileHeadInfo.DataBlockNum);
                     filename.Text += "\r\n";
                     filename.Text += "\r\n" + String.Format("下载总长度:{0:d}", PublicDataClass._FileHeadInfo.FileLen) + "-----"+
                                               String.Format("下载段数:{0:d}", PublicDataClass._FileHeadInfo.SegNum);
                     textBoxendseg.Text = Convert.ToString(PublicDataClass._FileHeadInfo.SegNum);
                }

                else if (filety == 4)                    //AMRstm32文件
                {
                    filetype = 4;//文件类型AMRstm32文件
                    bFileIsOk = true;

                    afile = new FileStream(OpenF.FileName, FileMode.Open, FileAccess.Read);

                    sr = new StreamReader(afile, Encoding.ASCII);

                    sr.BaseStream.Seek(0, SeekOrigin.Begin);
                    //counter_updateARM_txt(sr);

                    afile = new FileStream(OpenF.FileName, FileMode.Open, FileAccess.Read);
                    for (int i = 0; i < 1000; i++)
                    {
                        UPDATE_CODE_REG.UpCode_Seg[i].Up_Code = new byte[512];
                    }
                   
                    //ArmTxt_Code(afile);
                    ReadArm_Code(afile);
                    filename.Text += "\r\n";
                    filename.Text += "\r\n" + String.Format("校验和    :{0:x}", UPDATE_CODE_REG.ARMChecksum);
                    filename.Text += "\r\n";
                    //filename.Text += "\r\n" + String.Format("总块数    :{0:d}", UPDATE_CODE_REG.DD_Num);
                    //filename.Text += "\r\n";
                    filename.Text += "\r\n" + String.Format("下载总段数:{0:d}", UPDATE_CODE_REG.SegNum);
                    textBoxendseg.Text = Convert.ToString(UPDATE_CODE_REG.SegNum);

                }
                else
                {
                    bFileIsOk = false;
                    filename.Text += "\r\n";
                    filename.Text += "\r\n" + "选择的文件：无效";
                }
                sr.Close();
                textBoxstartseg.Text = "1";
                buttonstart.Enabled = true;
                buttonstop.Enabled  = false;
                this.progressBar1.Value = 0;
            }
        }
        /********************************************************************************************
        *  函数名：    CheckSelFileType                                                             *
        *  功能  ：    文件类型的校验                                                               *
        *  参数  ：    str                                                                          *
        *  返回值：    1-430文件  2--DSP文件  3--无效文件   4--AMRstm32文件                                           *
        *  修改日期：  2012-2-07                                                                   *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private byte CheckSelFileType(StreamReader str)
        {
            byte tmp = 0, Data_H = 0, Data_L = 0; ;
            strdata = @"";
            strdata = str.ReadLine();
            if (strdata.Contains("@5C00"))
            {
                filename.Text += "\r\n";
                filename.Text += "\r\n" + "选择的文件：MSP430文件";
                tmp = 1;
            }
            else if (strdata.Contains("@5000"))
            {
                filename.Text += "\r\n";
                filename.Text += "\r\n" + "选择的文件：AMRstm32文件";
                tmp = 4;
            }
            else
            {
                str.Read(code, 0, 66);
                str.Read(code, 0, 2);
                Data_L= Convert.ToByte(CharToIntOrbyte(code, 2));            //获取字节
                str.Read(code, 0, 1);
                str.Read(code, 0, 2);
                Data_H = Convert.ToByte(CharToIntOrbyte(code, 2));            //获取字节
                if (((Data_H << 8) + Data_L) != 0)
                {
                    filename.Text += "\r\n";
                    filename.Text += "\r\n" + "选择的文件：DSP文件";
                    tmp = 2;
                }
                else
                {
                    tmp = 3;

                }

            }
            str.Close();
            return tmp;
        }


        private void counter_update_txt(StreamReader str)
        {
             strdata = @"";
            do
            {
                strdata = str.ReadLine();
                if (strdata[0] == 'q') break;
                if (strdata[0] == 0) break;
                if (strdata[0] == '@')
                {
                     count_codeseg++;
                    UpdeCode.Info[count_codeseg].order = count_codeseg;
                  
                }
                else
                {
                    if (strdata != null)
                        UpdeCode.Info[count_codeseg].count_byte += ((strdata.Length + 1) / 3);
                }
            } while (strdata[0] != 'q');
            str.Close();
            
        }


        private void counter_updateARM_txt(StreamReader str)
        {
            strdata = @"";
           strdata = str.ReadLine();
            do
            {
                
              
                if (strdata[0] == 0) break;
                if (strdata[0] == '@')
                {
                    count_codeseg++;
                    UpdeCode.Info[count_codeseg].order = count_codeseg;

                }
                else
                {
                    if (strdata != null)
                        UpdeCode.Info[count_codeseg].count_byte += ((strdata.Length + 1) / 3);
                }

                strdata = str.ReadLine();
            } while (strdata != null);
            str.Close();
         //   

        }
        /********************************************************************************************
        *  函数名：    BintoTxt                                                             *
        *  功能  ：    bin转换成txt                                                              *
                                               *
        *  修改日期：  2012-2-15                                                                   *
                                                             *
        * ******************************************************************************************/
        private void BintoTxt(FileStream str_in, FileStream str_out)
        {

           byte [] buffer=new byte[16];       
            StreamReader sr = new StreamReader(str_in);
            StreamWriter sw = new StreamWriter(str_out);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            

            string strLine = sr.ReadLine();
            sw.WriteLine("@5000");
            while (strLine != null)
            {

                   str_in.Read(buffer, 0, 16);//读取FileStream对象所指的文件到字节数组里
               /**/ for (int i = 0; i < strLine.Length; i++)
                {                 
                  byte[] b= BitConverter.GetBytes(str_in.ReadByte());
              //    sw.Write(Convert.ToString(b[0],16));
                //  sw.Write(Convert.ToByte(b[0]));
                  sw.Write(Convert.ToString(b[0])); 
                  sw.Write(' ');
                  if ((i > 15 )&&( i % 16 == 0))
                  {
                      sw.Write("\r");
                          sw.Write("\n");
                  }
                }
                strLine = sr.ReadLine();

            }
              /*;
               Convert.ToByte(CharToIntOrbyte(code, 2));            //获取字节
               if (Convert.ToChar(a) == ' ')//扔掉' '  空格
                   for (byte k = 0; k < 2; k++)
                       code[k] = Convert.ToChar(str.ReadByte());

               if (Convert.ToChar(a) == '@')
                   {
                       for (byte k = 0; k < 4; k++)
                       {
                           code[k] = Convert.ToChar(str.ReadByte());

                       }
                       startdata = CharToIntOrbyte(code, 4);          //获取首信息
          
                        for (byte k = 0; k < 2; k++)
                                 code[k] = Convert.ToChar(str.ReadByte());
                             a = str.ReadByte();
                             if (Convert.ToChar(a) == ' ')//扔掉' '  空格
                             { }

                             if (Convert.ToChar(a) == '\r')
                                 a = str.ReadByte();    //扔掉'\n '
                             tin = Convert.ToByte(CharToIntOrbyte(code, 2));            //获取字节*/
      

            sr.Close();
            sw.Close();
            str_in.Close();
            str_out.Close();
          

        
        
        }
        /********************************************************************************************
       *  函数名：    Txt430_Code                                                                  *
       *  功能  ：    读取430编译后的txt文件                                                       *
       *  参数  ：    str                                                                          *
       *  返回值：    无                                                                           *
       *  修改日期：  2010-12-07                                                                   *
       *  作者    ：  cuibj                                                                        *
       * ******************************************************************************************/
        private void Txt430_Code(FileStream str)
        {
            int a =0,startdata=0;
            byte nDD = 0,tin=0;
            int Pagenum = 0, npos = 0;
            segnum = 0;
            UPDATE_CODE_REG.Checksum = 0;
            for (byte i = 1; i <= count_codeseg; i++)
            {
                
                a =str.ReadByte();
                if (Convert.ToChar(a) == '@')
                {
                    for (byte k = 0; k < 4; k++)
                    {
                       code[k] =Convert.ToChar( str.ReadByte());

                    }
                    startdata = CharToIntOrbyte(code, 4);          //获取首信息
                    if (startdata != 0x5c00)
                    {
                        UPDATE_CODE_REG.Seg_Reg[nDD].firstadd = startdata;
                        UPDATE_CODE_REG.Seg_Reg[nDD].fs_segnum = segnum;
                        UPDATE_CODE_REG.Seg_Reg[nDD].PageNum = 1;
                        UPDATE_CODE_REG.Seg_Reg[nDD].fs_Page = Pagenum;
                        Pagenum += 1;
                        npos = 0;

                        UPDATE_CODE_REG.UpCode_Seg[segnum].PageAdd = Pagenum - 1;
                        UPDATE_CODE_REG.UpCode_Seg[segnum].AddInPage = 0;
                    }
                    for(byte k=0;k<2;k++)
                        a = str.ReadByte();   //扔掉\r\n

                    for (int j = 0; j < UpdeCode.Info[i].count_byte; j++)
                    {
                            for (byte k = 0; k < 2; k++)
                                code[k] = Convert.ToChar(str.ReadByte());
                            a =str.ReadByte();  
                            if(Convert.ToChar(a) ==' ')//扔掉' '  空格
                            {}
                            
                            if(Convert.ToChar(a) =='\r')
                                a = str.ReadByte();    //扔掉'\n '
                            tin = Convert.ToByte(CharToIntOrbyte(code, 2));            //获取字节

                            if (startdata != 0x5c00)
                            {
                                //if (j != 0 && (j & 0x1ff) == 0) //0x1ff=511
                                if ((j != 0) && (j & (PublicDataClass.CodeUpdatalen-1)) == 0)   //修改一帧长度
                                {
                                    
                                    UPDATE_CODE_REG.Seg_Reg[nDD].PageNum += 1;
                                    Pagenum += 1;

                                    UPDATE_CODE_REG.UpCode_Seg[segnum].Codelen = npos;
                                    segnum++;
                                    UPDATE_CODE_REG.UpCode_Seg[segnum].PageAdd = Pagenum - 1;
                                    UPDATE_CODE_REG.UpCode_Seg[segnum].AddInPage = j - (Pagenum - 1 - UPDATE_CODE_REG.Seg_Reg[nDD].fs_Page) * 512;
                                    npos = 0;
                                }
                                UPDATE_CODE_REG.UpCode_Seg[segnum].Up_Code[npos] = tin;
                                UPDATE_CODE_REG.Checksum += tin;
                                npos++;
                            }

                    }
                    for (byte k = 0; k < 2; k++)
                        a = str.ReadByte();   //扔掉\r\n

                    if (startdata != 0x5c00)
                    {
                        UPDATE_CODE_REG.UpCode_Seg[segnum].Codelen = npos;
                        UPDATE_CODE_REG.Seg_Reg[nDD].ed_segnum = segnum;
                        segnum++;
                        nDD++;
                    }
                }
            }
            UPDATE_CODE_REG.SegNum = segnum;
            UPDATE_CODE_REG.DD_Num = nDD;

            PublicDataClass._FileHeadInfo.CheckSum = UPDATE_CODE_REG.Checksum;
            PublicDataClass._FileHeadInfo.DataBlockNum = nDD;
            PublicDataClass._FileHeadInfo.SegNum = segnum;
            str.Close();
            //为第二次打开文件时清除上一次参数
            for (byte i = 1; i <= count_codeseg; i++)
            {
                UpdeCode.Info[i].count_byte = 0;
            }
            count_codeseg = 0;
        }

        /********************************************************************************************
      *  函数名：    ArmTxt_Code                                                             *
      *  功能  ：    读取AMR编译后的txt文件                                                       *
      *  参数  ：    str                                                                          *
      *  返回值：    无                                                                           *
      *  修改日期：  2012-2-07                                                                   *
      *  作者    ：  liuhch                                                                        *
      * ******************************************************************************************/
        private void ArmTxt_Code(FileStream str)
        {
            int a = 0, startdata = 0;
            byte nDD = 0, tin = 0;
            int Pagenum = 0, npos = 0;
            segnum = 0;

            UPDATE_CODE_REG.Checksum = 0;


            for (byte i = 1; i <= count_codeseg; i++)
            {

                a = str.ReadByte();
                if (Convert.ToChar(a) == '@')
                {
                    for (byte k = 0; k < 4; k++)
                    {
                        code[k] = Convert.ToChar(str.ReadByte());

                    }
                    startdata = CharToIntOrbyte(code, 4);          //获取首信息
                      if (startdata != 0x5c00)
                      {
                          UPDATE_CODE_REG.Seg_Reg[nDD].firstadd = startdata;
                          UPDATE_CODE_REG.Seg_Reg[nDD].fs_segnum = segnum;
                          UPDATE_CODE_REG.Seg_Reg[nDD].PageNum = 1;
                          UPDATE_CODE_REG.Seg_Reg[nDD].fs_Page = Pagenum;
                          Pagenum += 1;
                          npos = 0;

                          UPDATE_CODE_REG.UpCode_Seg[segnum].PageAdd = Pagenum - 1;
                          UPDATE_CODE_REG.UpCode_Seg[segnum].AddInPage = 0;
                      }
                      for (byte k = 0; k < 2; k++)
                          a = str.ReadByte();   //扔掉\r\n

                      for (int j = 0; j < UpdeCode.Info[i].count_byte; j++)
                      {
                          for (byte k = 0; k < 2; k++)
                              code[k] = Convert.ToChar(str.ReadByte());
                          a = str.ReadByte();
                          if (Convert.ToChar(a) == ' ')//扔掉' '  空格
                          { }

                          if (Convert.ToChar(a) == '\r')
                              a = str.ReadByte();    //扔掉'\n '
                          tin = Convert.ToByte(CharToIntOrbyte(code, 2));            //获取字节

                          if (startdata != 0x5c00)
                          {
                              //if (j != 0 && (j & 0x1ff) == 0)  //0x1ff=511
                              if (j != 0 && (j & (PublicDataClass.CodeUpdatalen-1)) == 0)  //修改一帧长度
                              {

                                  UPDATE_CODE_REG.Seg_Reg[nDD].PageNum += 1;
                                  Pagenum += 1;

                                  UPDATE_CODE_REG.UpCode_Seg[segnum].Codelen = npos;
                                  segnum++;
                                  UPDATE_CODE_REG.UpCode_Seg[segnum].PageAdd = Pagenum - 1;
                                  UPDATE_CODE_REG.UpCode_Seg[segnum].AddInPage = j - (Pagenum - 1 - UPDATE_CODE_REG.Seg_Reg[nDD].fs_Page) * 512;
                                  npos = 0;
                              }
                              UPDATE_CODE_REG.UpCode_Seg[segnum].Up_Code[npos] = tin;
                              UPDATE_CODE_REG.Checksum += tin;
                              npos++;
                          }

                      }
                      for (byte k = 0; k < 2; k++)
                          a = str.ReadByte();   //扔掉\r\n

                      if (startdata != 0x5c00)
                      {
                          UPDATE_CODE_REG.UpCode_Seg[segnum].Codelen = npos;
                          UPDATE_CODE_REG.Seg_Reg[nDD].ed_segnum = segnum;
                          segnum++;
                          nDD++;
                      }
                  }
                }
                UPDATE_CODE_REG.SegNum = segnum;
                UPDATE_CODE_REG.DD_Num = nDD;

                PublicDataClass._FileHeadInfo.CheckSum = UPDATE_CODE_REG.Checksum;
                PublicDataClass._FileHeadInfo.DataBlockNum = nDD;
                PublicDataClass._FileHeadInfo.SegNum = segnum;
                str.Close();

                //为第二次打开文件时清除上一次参数
            
                UpdeCode.Info[count_codeseg].count_byte = 0;
                count_codeseg = 0;
            
        }
        /********************************************************************************************
*  函数名：    ReadArm_Code                                                             *
*  功能  ：    读取AMR编译后的bin文件                                                       *
*  参数  ：    str                                                                          *
*  返回值：    无                                                                           *
*  修改日期：  2013-7-25                                                                   *
*  作者    ：  liuhch                                                                        *
* ******************************************************************************************/
        private void ReadArm_Code(FileStream str_in)
        {
         
            int j = 0, startdata = 0;
            byte[] buffer = new byte[PublicDataClass.CodeUpdatalen];      
         
            int Pagenum = 0, npos = 0;
            segnum = 0;
             byte tin;
             BinaryReader br = new BinaryReader(str_in);
             UpdeCode.Info[0].count_byte = 0;
             UPDATE_CODE_REG.ARMChecksum = 0;

             try
             {
                 while (true)
                 {
                     tin=br.ReadByte();
                     //buffer = br.ReadBytes(PublicDataClass.CodeUpdatalen);
                     UpdeCode.Info[0].count_byte++;
                     if (j==PublicDataClass.CodeUpdatalen )  //一帧长度
                         //if (j != 0 && (j & (PublicDataClass.CodeUpdatalen - 1)) == 0)  //修改一帧长度
                     {
                         //UPDATE_CODE_REG.Seg_Reg[0].PageNum += 1;
                         //Pagenum += 1;
                         UPDATE_CODE_REG.UpCode_Seg[segnum].Codelen = npos;
                         segnum++;
                         npos = 0;
                         j = 0;
                     }
                     UPDATE_CODE_REG.UpCode_Seg[segnum].Up_Code[npos] = tin;
                     UPDATE_CODE_REG.ARMChecksum += tin;
                     npos++;
                     j++;
     
                 }
             }
             catch (EndOfStreamException e)
             {
                 UPDATE_CODE_REG.UpCode_Seg[segnum].Codelen = npos;
                 segnum++;
                 UPDATE_CODE_REG.SegNum = segnum;
                 PublicDataClass._FileHeadInfo.ARMCheckSum = UPDATE_CODE_REG.ARMChecksum;
                 PublicDataClass._FileHeadInfo.SegNum = segnum;
                 br.Close();
                 str_in.Close();

                 //为第二次打开文件时清除上一次参数

            
                 //Console.WriteLine("已经读到末尾");
             }



            // StreamReader sr = new StreamReader(str_in);
            // sr.BaseStream.Seek(0, SeekOrigin.Begin);

            // string strLine = sr.ReadLine();
            //while (strLine != null)
            //{

            //    str_in.Read(buffer, 0, PublicDataClass.CodeUpdatalen);//读取FileStream对象所指的文件到字节数组里
            //   /**/ for (int i = 0; i < strLine.Length; i++)
            //    {                 
            //      byte[] b= BitConverter.GetBytes(str_in.ReadByte());
            //       tin=b[0];
            //         UpdeCode.Info[0].count_byte++;
            //             if (j != 0 && (j & (PublicDataClass.CodeUpdatalen - 1)) == 0)  //修改一帧长度
            //                {
            //                    UPDATE_CODE_REG.Seg_Reg[0].PageNum += 1;
            //                    Pagenum += 1;
            //                    UPDATE_CODE_REG.UpCode_Seg[segnum].Codelen = npos;
            //                    segnum++;                              
            //                    npos = 0;
            //                }
            //                UPDATE_CODE_REG.UpCode_Seg[segnum].Up_Code[npos] = tin;
            //                UPDATE_CODE_REG.Checksum += tin;
            //                npos++;
        
            //    }
  
            //    strLine = sr.ReadLine();
            //}

            //UPDATE_CODE_REG.SegNum = segnum;
            //PublicDataClass._FileHeadInfo.CheckSum = UPDATE_CODE_REG.Checksum;
            //PublicDataClass._FileHeadInfo.SegNum = segnum;

            //sr.Close();

            ////为第二次打开文件时清除上一次参数

            //UpdeCode.Info[0].count_byte = 0;
      
    

  


            //for (byte i = 1; i <= count_codeseg; i++)
            //{

            //    a = str.ReadByte();
            //    if (Convert.ToChar(a) == '@')
            //    {
            //        for (byte k = 0; k < 4; k++)
            //        {
            //            code[k] = Convert.ToChar(str.ReadByte());

            //        }
            //        startdata = CharToIntOrbyte(code, 4);          //获取首信息
            //        if (startdata != 0x5c00)
            //        {
            //            UPDATE_CODE_REG.Seg_Reg[nDD].firstadd = startdata;
            //            UPDATE_CODE_REG.Seg_Reg[nDD].fs_segnum = segnum;
            //            UPDATE_CODE_REG.Seg_Reg[nDD].PageNum = 1;
            //            UPDATE_CODE_REG.Seg_Reg[nDD].fs_Page = Pagenum;
            //            Pagenum += 1;
            //            npos = 0;

            //            UPDATE_CODE_REG.UpCode_Seg[segnum].PageAdd = Pagenum - 1;
            //            UPDATE_CODE_REG.UpCode_Seg[segnum].AddInPage = 0;
            //        }
            //        for (byte k = 0; k < 2; k++)
            //            a = str.ReadByte();   //扔掉\r\n

            //        for (int j = 0; j < UpdeCode.Info[i].count_byte; j++)
            //        {
            //            for (byte k = 0; k < 2; k++)
            //                code[k] = Convert.ToChar(str.ReadByte());
            //            a = str.ReadByte();
            //            if (Convert.ToChar(a) == ' ')//扔掉' '  空格
            //            { }

            //            if (Convert.ToChar(a) == '\r')
            //                a = str.ReadByte();    //扔掉'\n '
            //            tin = Convert.ToByte(CharToIntOrbyte(code, 2));            //获取字节

            //            if (startdata != 0x5c00)
            //            {
            //                //if (j != 0 && (j & 0x1ff) == 0)  //0x1ff=511
            //                if (j != 0 && (j & (PublicDataClass.CodeUpdatalen - 1)) == 0)  //修改一帧长度
            //                {

            //                    UPDATE_CODE_REG.Seg_Reg[nDD].PageNum += 1;
            //                    Pagenum += 1;

            //                    UPDATE_CODE_REG.UpCode_Seg[segnum].Codelen = npos;
            //                    segnum++;
            //                    UPDATE_CODE_REG.UpCode_Seg[segnum].PageAdd = Pagenum - 1;
            //                    UPDATE_CODE_REG.UpCode_Seg[segnum].AddInPage = j - (Pagenum - 1 - UPDATE_CODE_REG.Seg_Reg[nDD].fs_Page) * 512;
            //                    npos = 0;
            //                }
            //                UPDATE_CODE_REG.UpCode_Seg[segnum].Up_Code[npos] = tin;
            //                UPDATE_CODE_REG.Checksum += tin;
            //                npos++;
            //            }

            //        }
            //        for (byte k = 0; k < 2; k++)
            //            a = str.ReadByte();   //扔掉\r\n

            //        if (startdata != 0x5c00)
            //        {
            //            UPDATE_CODE_REG.UpCode_Seg[segnum].Codelen = npos;
            //            UPDATE_CODE_REG.Seg_Reg[nDD].ed_segnum = segnum;
            //            segnum++;
            //            nDD++;
            //        }
            //    }
            //}
            //UPDATE_CODE_REG.SegNum = segnum;
            //UPDATE_CODE_REG.DD_Num = nDD;

            //PublicDataClass._FileHeadInfo.CheckSum = UPDATE_CODE_REG.Checksum;
            //PublicDataClass._FileHeadInfo.DataBlockNum = nDD;
            //PublicDataClass._FileHeadInfo.SegNum = segnum;
            //str.Close();

            ////为第二次打开文件时清除上一次参数

            //UpdeCode.Info[count_codeseg].count_byte = 0;
            //count_codeseg = 0;

        }
        /********************************************************************************************
       *  函数名：    DspTxt_Code                                                                  *
       *  功能  ：    读取DSP编译后的txt文件                                                       *
       *  参数  ：    str                                                                          *
       *  返回值：    1-430文件  2--DSP文件  3--无效文件                                           *
       *  修改日期：  2010-12-07                                                                   *
       *  作者    ：  cuibj                                                                        *
       * ******************************************************************************************/
        private void DspTxt_Code(FileStream str)
        {
            int a = 0;
            byte Len_H = 0, Len_L = 0, tin;
            int length = 0, LinSLen = 0;
            byte checksum = 0;
            int Address = 0;
            int blockSumSeg = 0;
            int pos = 0;

            byte blocknum = 0;
            byte[] add = new byte[4];
            segnum = 0;

            a = str.ReadByte();  //抛弃首字符
            for (byte k = 0; k < 2; k++)
                a = str.ReadByte();   //扔掉\r\n
            for (byte k = 0; k < 22; k++)     //扔掉22个无效的字节
            {
                a = str.ReadByte();
                a = str.ReadByte();
                a = str.ReadByte();
            }
            while (a != -1)     //如果不是文件的结尾
            {
                for (byte k = 0; k < 2; k++)
                    code[k] = Convert.ToChar(str.ReadByte());
                Len_L = Convert.ToByte(CharToIntOrbyte(code, 2));            //获取本段长度的高字节
                a = str.ReadByte();
                if (Convert.ToChar(a) == ' ')//扔掉' '  空格
                    for (byte k = 0; k < 2; k++)
                        code[k] = Convert.ToChar(str.ReadByte());
                Len_H = Convert.ToByte(CharToIntOrbyte(code, 2));            //获取字节长度的低字节

                length = (Len_H << 8) + Len_L;
                if (length <= 0)
                    break;
                checksum += Len_L;
                checksum += Len_H;


                a = str.ReadByte();
                if (blocknum == 0)
                {
                    if (Convert.ToChar(a) == ' ')//扔掉' '  空格
                    {
                        a = str.ReadByte();    //扔掉'\r '
                        a = str.ReadByte();    //扔掉'\n '
                    }
                    if (Convert.ToChar(a) == '\r')
                        a = str.ReadByte();    //扔掉'\n '
                }
                //读地址
                for (byte j = 0; j < 4; j++)
                {
                    for (byte k = 0; k < 2; k++)
                        code[k] = Convert.ToChar(str.ReadByte());
                    add[j] = Convert.ToByte(CharToIntOrbyte(code, 2));
                    a = str.ReadByte();  //扔掉' '  空格
                    //checksum += add[j];
                }

                //a = str.ReadByte();  //扔掉' '  空格

                Address = (add[1] << 8) + add[0];
                Address = Address << 16;
                Address += ((add[3] << 8) + add[2]);

                //获取代码
                length = length * 2;
                if (length > PublicDataClass.CodeUpdatalen)       //长度>PublicDataClass.CodeUpdatalen
                {
                    LinSLen = 0;
                    while ((length - LinSLen) > PublicDataClass.CodeUpdatalen)
                    {
                        pos = 0;
                        while (pos < PublicDataClass.CodeUpdatalen)
                        {
                            for (byte k = 0; k < 2; k++)
                                code[k] = Convert.ToChar(str.ReadByte());
                            if (code[0] != '\r' && code[1] != '\n')
                            {
                                a = str.ReadByte();  //扔掉' '  空格
                                tin = Convert.ToByte(CharToIntOrbyte(code, 2));            //获取字节
                                FILECODE.CodeByte[segnum].FileBytes[pos] = tin;
                                FILECODE.CodeByte[segnum].Len++;
                                checksum += tin;
                                pos++;
                            }
                        }
                        LinSLen += PublicDataClass.CodeUpdatalen;
                        segnum++;
                    }
                    LinSLen = length - LinSLen;    //剩余的字节数
                    pos = 0;
                    while (pos < LinSLen)             //直接LinSLen   
                    {
                        for (byte k = 0; k < 2; k++)
                            code[k] = Convert.ToChar(str.ReadByte());
                        if (code[0] != '\r' && code[1] != '\n')
                        {
                            a = str.ReadByte();  //扔掉' '  空格
                            tin = Convert.ToByte(CharToIntOrbyte(code, 2));            //获取字节
                            FILECODE.CodeByte[segnum].FileBytes[pos] = tin;
                            FILECODE.CodeByte[segnum].Len++;
                            checksum += tin;
                            pos++;
                        }

                    }
                    segnum++;

                }
                else
                {
                    pos = 0;
                    while (pos < length)             //直接LinSLen   
                    {
                        for (byte k = 0; k < 2; k++)
                            code[k] = Convert.ToChar(str.ReadByte());
                        if (code[0] != '\r' && code[1] != '\n')
                        {
                            a = str.ReadByte();  //扔掉' '  空格
                            tin = Convert.ToByte(CharToIntOrbyte(code, 2));            //获取字节
                            FILECODE.CodeByte[segnum].FileBytes[pos] = tin;
                            FILECODE.CodeByte[segnum].Len++;
                            checksum += tin;
                            pos++;
                        }

                    }
                    segnum++;
                }
                for (byte k = 0; k < 2; k++)
                    a = str.ReadByte();             //扔掉'\r ''\n'
                BLOCKINFO.BlockInfo[blocknum].blockAddr = Address;
                BLOCKINFO.BlockInfo[blocknum].blockLen = length;
                if (BLOCKINFO.BlockInfo[blocknum].blockLen <= PublicDataClass.CodeUpdatalen)
                    blockSumSeg = 1;
                else
                {
                    if ((BLOCKINFO.BlockInfo[blocknum].blockLen % PublicDataClass.CodeUpdatalen) == 0)  //PublicDataClass.CodeUpdatalen的偶数
                        blockSumSeg = BLOCKINFO.BlockInfo[blocknum].blockLen / PublicDataClass.CodeUpdatalen;    //只有一段
                    else
                        blockSumSeg = BLOCKINFO.BlockInfo[blocknum].blockLen / PublicDataClass.CodeUpdatalen + 1;
                }

                if (blocknum == 0)
                {

                    BLOCKINFO.BlockInfo[blocknum].BeginSeg = 1;
                    BLOCKINFO.BlockInfo[blocknum].blockSumSeg = blockSumSeg;
                    BLOCKINFO.BlockInfo[blocknum].EndSeg = blockSumSeg;

                }
                else
                {
                    BLOCKINFO.BlockInfo[blocknum].BeginSeg = BLOCKINFO.BlockInfo[blocknum - 1].EndSeg + 1;  //开始段
                    BLOCKINFO.BlockInfo[blocknum].blockSumSeg = blockSumSeg;                                   //总段数
                    if (blockSumSeg == 1)
                        BLOCKINFO.BlockInfo[blocknum].EndSeg = BLOCKINFO.BlockInfo[blocknum].BeginSeg;         //结束段
                    else
                        BLOCKINFO.BlockInfo[blocknum].EndSeg = blockSumSeg + BLOCKINFO.BlockInfo[blocknum].BeginSeg - 1;

                }
                blocknum++;
                PublicDataClass._FileHeadInfo.FileLen += length;

            }
            PublicDataClass._FileHeadInfo.CheckSum = checksum;
            PublicDataClass._FileHeadInfo.DataBlockNum = blocknum;
            PublicDataClass._FileHeadInfo.SegNum = segnum;

            str.Close();   //关闭文件读入流

        }
        /// <summary>
        /// 文件校验---按钮的处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonjy_Click(object sender, EventArgs e)  //文件校验
        {
            byte[] data = new byte[2];
            if (bFileIsOk == false)
            {
                MessageBox.Show("文件选择有误", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (filetype == 4)//ARM32文件
            {

                  PublicDataClass._DataField.FieldLen = 0;
                  PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)( PublicDataClass._FileHeadInfo.ARMCheckSum & 0x00ff);
                  PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)((PublicDataClass._FileHeadInfo.ARMCheckSum & 0xff00) >> 8);
                  PublicDataClass._DataField.FieldLen += 2;

                datacode = ConVertToByte((int)(UpdeCode.Info[0].count_byte), 4);
                for (byte i = 0; i < 4; i++)
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + i] = datacode[i];
                PublicDataClass._DataField.FieldLen += 4;
                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = 1000;
                
                //byte[] pd = new byte[2];
                //datacode = ConVertToByte(1000, 2);
                //for (byte i = 0; i < 2; i++)
                //    pd[i] = datacode[i];                                                //段号

                //for (byte i = 0; i < 2; i++)
                //    PublicDataClass._DataField.Buffer[i] = pd[i];
                //PublicDataClass._DataField.FieldLen += 2;

                if (ty == 1)
                    PublicDataClass._ComTaskFlag.UpdateCode_ARMJY_1 = true;
                if (ty == 2)
                    PublicDataClass._NetTaskFlag.UpdateCode_ARMJY_1 = true;
                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.UpdateCode_ARMJY_1 = true; 

            }
            else  if (filetype == 2 )//2812文件//可能错了
            {
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.Buffer[0] = PublicDataClass._FileHeadInfo.CheckSum;
                PublicDataClass._DataField.FieldLen++;
                datacode = ConVertToByte(PublicDataClass._FileHeadInfo.FileLen, 4);
                for (byte i = 0; i < 4; i++)
                    PublicDataClass._DataField.Buffer[1 + i] = datacode[i];
                PublicDataClass._DataField.FieldLen += 4;
                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = 1000;
                if (ty == 1)
                    PublicDataClass._ComTaskFlag.UpdateCode_JY_1 = true;
                if (ty == 2)
                    PublicDataClass._NetTaskFlag.UpdateCode_JY_1 = true;
                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.UpdateCode_JY_1 = true;
            }
            else if ( filetype == 1)//430
            {
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.Buffer[0] = PublicDataClass._FileHeadInfo.CheckSum;
                PublicDataClass._DataField.FieldLen++;

                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen++] = PublicDataClass._FileHeadInfo.DataBlockNum;  //任务数
                for (byte i = 0; i < PublicDataClass._FileHeadInfo.DataBlockNum; i++)
                {
                    datacode = ConVertToByte(UPDATE_CODE_REG.Seg_Reg[i].firstadd, 4);
                    for (byte j = 0; j < 4; j++)
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + j] = datacode[j];
                    PublicDataClass._DataField.FieldLen += 4;

                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen++] = (byte)(UPDATE_CODE_REG.Seg_Reg[i].fs_Page & 0xff);  //存储器首页地址
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen++] = (byte)(UPDATE_CODE_REG.Seg_Reg[i].fs_Page >> 8);
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen++] = (byte)(UPDATE_CODE_REG.Seg_Reg[i].PageNum & 0xff);  //存储器存储页数
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen++] = (byte)(UPDATE_CODE_REG.Seg_Reg[i].PageNum >> 8);

                }

                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = 1000;
                if (ty == 1)
                    PublicDataClass._ComTaskFlag.UpdateCode_JY_1 = true;
                if (ty == 2)
                    PublicDataClass._NetTaskFlag.UpdateCode_JY_1 = true;
                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.UpdateCode_JY_1 = true;
            }

            //PublicDataClass.seq = 1;
            //PublicDataClass.seqflag = 0;
            //PublicDataClass.SQflag = 0;
            //PublicDataClass.ParamInfoAddr = 1000;
            //if (ty == 1)
            //    PublicDataClass._ComTaskFlag.UpdateCode_JY_1 = true;
            //if (ty == 2)
            //    PublicDataClass._NetTaskFlag.UpdateCode_JY_1 = true;
            
                timer1.Enabled = true;
        }
        /// <summary>
        /// 升级---按钮的处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonupdata_Click(object sender, EventArgs e)
        {
            PublicDataClass._ChangeFlag.CodeUpdateFlag = false;
            /*if (PublicDataClass.LinSPointName == "无信息")
            {
                MessageBox.Show("无测量点信息可操作", "信息",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            }*/
            if (bFileIsOk == false)
            {
                MessageBox.Show("文件选择有误", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (filetype == 2 || filetype == 1)//2812文件
            {
                PublicDataClass._DataField.FieldLen = 0;

                //byte[] pd = new byte[2];
                //datacode = ConVertToByte(1001, 2);
                //for (byte i = 0; i < 2; i++)
                //    pd[i] = datacode[i];                                                //段号

                //for (byte i = 0; i < 2; i++)
                //    PublicDataClass._DataField.Buffer[i] = pd[i];
                //PublicDataClass._DataField.FieldLen += 2;

                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = 1001;
                if (ty == 1)
                    PublicDataClass._ComTaskFlag.UpdateCode_OK_1 = true;
                if (ty == 2)
                    PublicDataClass._NetTaskFlag.UpdateCode_OK_1 = true;
                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.UpdateCode_OK_1 = true;
            }
            else  if (filetype == 4)//ARM32文件
            {
                PublicDataClass._DataField.FieldLen = 0;

                //byte[] pd = new byte[2];
                //datacode = ConVertToByte(1001, 2);
                //for (byte i = 0; i < 2; i++)
                //    pd[i] = datacode[i];                                                //段号

                //for (byte i = 0; i < 2; i++)
                //    PublicDataClass._DataField.Buffer[i] = pd[i];
                //PublicDataClass._DataField.FieldLen += 2;

                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = 1001;
                if (ty == 1)
                    PublicDataClass._ComTaskFlag.UpdateCode_ARMOK_1 = true;
                if (ty == 2)
                    PublicDataClass._NetTaskFlag.UpdateCode_ARMOK_1 = true;
                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.UpdateCode_ARMOK_1 = true;
            }

            //PublicDataClass.seq = 1;
            //PublicDataClass.seqflag = 0;
            //PublicDataClass.SQflag = 0;
            //PublicDataClass.ParamInfoAddr = 1001;
            //if (ty == 1)
            //    PublicDataClass._ComTaskFlag.UpdateCode_OK_1 = true;
            //if (ty == 2)
            //    PublicDataClass._NetTaskFlag.UpdateCode_OK_1 = true;
            

        }
        /// <summary>
        /// 开始下载---按钮的处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonstart_Click(object sender, EventArgs e)
        {
            timer3.Enabled = true;
            PublicDataClass._ChangeFlag.CodeUpdateFlag = true;
            /*if (PublicDataClass.LinSPointName == "无信息")
            {
                MessageBox.Show("无测量点信息可操作", "信息",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            }*/
            
            
            if (bFileIsOk == false)
            {
                MessageBox.Show("文件选择有误", "信息",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((Convert.ToInt16(textBoxstartseg.Text) > segnum) || (Convert.ToInt16(textBoxstartseg.Text) < 1))
            {
                MessageBox.Show("开始段选择有误", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            if ((Convert.ToInt16(textBoxendseg.Text) > segnum) || (Convert.ToInt16(textBoxendseg.Text) < 1))
            {
                MessageBox.Show("结束段选择有误", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            if (Convert.ToInt16(textBoxendseg.Text) < Convert.ToInt16(textBoxstartseg.Text))
            {
                MessageBox.Show("开始段小于结束段有误", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            FileDownInfo.DownSegNo = Convert.ToInt16(textBoxstartseg.Text);
            FileDownInfo.StartSeg = FileDownInfo.DownSegNo;
            PublicDataClass._DataField.FieldLen = 0;


            FirstDownDoing = true;    //开始下载标志
            DownFileSegment();

            PublicDataClass.seq = 1;
            PublicDataClass.seqflag = 1;
            PublicDataClass.SQflag = 0;
            PublicDataClass.ParamInfoAddr = FileDownInfo.DownSegNo;

            //if (ty == 1)
            //    PublicDataClass._ComTaskFlag.UpdateCode_Start_1 = true;
            //if (ty == 2)
            //    PublicDataClass._NetTaskFlag.UpdateCode_Start_1 = true;

            if (filetype == 2||filetype == 1) //2812文件
            {

                if (ty == 1)
                    PublicDataClass._ComTaskFlag.UpdateCode_Start_1 = true;
                if (ty == 2)
                    PublicDataClass._NetTaskFlag.UpdateCode_Start_1 = true;
                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.UpdateCode_Start_1 = true;
            }
            else if (filetype == 4) //ARM文件
            {
                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 1;
                PublicDataClass.SQflag = 0;
                if (ty == 1)
                    PublicDataClass._ComTaskFlag.UpdateCode_ARMStart_1 = true;
                if (ty == 2)
                    PublicDataClass._NetTaskFlag.UpdateCode_ARMStart_1 = true;
                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.UpdateCode_ARMStart_1 = true;
            }
            timer1.Enabled = true;    //打开定时器
            progressBar1.Minimum = 0;               //设定ProgressBar控件的最小值为0
            progressBar1.Maximum = PublicDataClass._FileHeadInfo.SegNum;              //设定ProgressBar控件的最大值为10

            buttonstart.Enabled = false;
            buttonstop.Enabled  = true;
        }
        /// <summary>
        /// 中断按钮----的处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonstop_Click(object sender, EventArgs e)
        {
            //PublicDataClass._NetTaskFlag.UpdateCode_Stop_1 = true;
            PublicDataClass._Message.CodeUpdateDoing = false;
            timer1.Enabled = false;
            buttonstart.Enabled = true;
            buttonstop.Enabled  = false;
        }

        private int CharToIntOrbyte(char []data,byte num)
        {
            int tmp = 0;
            byte[] value = new byte[num];
            for (byte j = 0; j < num; j++)
            {
                if ((data[j] >= '0') && (data[j] <= '9'))
                    value[j] = Convert.ToByte(data[j] - '0');
                if ((data[j] >= 'A') && (data[j] <= 'F'))
                    value[j] = Convert.ToByte(data[j] - 'A' + 10);
                if ((data[j] >= 'a') && (data[j] <= 'f'))
                    value[j] = Convert.ToByte(data[j] - 'a' + 10);
            }
            if (num == 4)
            {
                tmp = (value[0] << 4) + value[1];
                tmp = tmp << 8;
                tmp = tmp + ((value[2] << 4) + value[3]);
            }
            else if (num == 2)
            {
                tmp = (value[0] << 4) + value[1];

            }
            return tmp;
        }

        private void textBoxstartseg_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字
                {

                    MessageBox.Show("只能输入数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Handled = true;
                }
            }
        }

        private void textBoxendseg_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字
                {

                    MessageBox.Show("只能输入数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Handled = true;
                }
            }
        }

        private void DownFileSegment()
        {


            if (filetype == 4) //ARM文件 
            {


                datacode = ConVertToByte(UPDATE_CODE_REG.SegNum, 2);
                for (byte i = 0; i < 2; i++)
                    PublicDataClass._DataField.Buffer[i] = datacode[i];                                                //总段数
                datacode = ConVertToByte(FileDownInfo.DownSegNo , 2);
                for (byte i = 0; i < 2; i++)
                    PublicDataClass._DataField.Buffer[2 + i] = datacode[i];                                              //当前段号
                datacode = ConVertToByte(UPDATE_CODE_REG.UpCode_Seg[FileDownInfo.DownSegNo - 1].Codelen, 2);
                for (byte i = 0; i < 2; i++)
                    PublicDataClass._DataField.Buffer[4 + i] = datacode[i];                                             //当前段数据长度

                PublicDataClass._DataField.FieldLen = 6;
                for (int i = 0; i < UPDATE_CODE_REG.UpCode_Seg[FileDownInfo.DownSegNo - 1].Codelen; i++)
                {
                    PublicDataClass._DataField.Buffer[6 + i] = UPDATE_CODE_REG.UpCode_Seg[FileDownInfo.DownSegNo - 1].Up_Code[i];  //代码赋值
                }
                PublicDataClass._DataField.FieldLen += UPDATE_CODE_REG.UpCode_Seg[FileDownInfo.DownSegNo - 1].Codelen;

            }

                 //if (FirstDownDoing == true    )//开始下载标志从第一段发
                 //  {
                 //      FirstDownDoing = false;   
                     
                 //       byte[] pda4= new byte[2];  
                 //       datacode = ConVertToByte(FileDownInfo.DownSegNo-1, 2);
                 //      for (byte i = 0; i <2; i++)
                 //      pda4[ i] = datacode[i];                                                //段号
                 
                 //       for (byte i = 0; i <2; i++)
                 //      PublicDataClass._DataField.Buffer[i] = pda4[i];
                 //       PublicDataClass._DataField.FieldLen += 2;
                 //       FileDownInfo.DownSegNo = 0;

                 //  }


                 //   else
                 //        {
                
                 //       byte[] pda430= new byte[10];      
                
                 //        datacode = ConVertToByte(FileDownInfo.DownSegNo, 2);
                 //       for (byte i = 0; i <2; i++)
                 //         pda430[ i] = datacode[i];                                                //段号
                         
                 //          pda430[2] = 0;   
                 //          pda430[ 3] = 0;   
                
                 //       datacode = ConVertToByte(UPDATE_CODE_REG.UpCode_Seg[FileDownInfo.DownSegNo -1].PageAdd ,2);
                 //       for (byte i = 0; i < 2; i++)
                 //           pda430[4 + i] = datacode[i];                                              //页地址
                 //       datacode = ConVertToByte(UPDATE_CODE_REG.UpCode_Seg[FileDownInfo.DownSegNo -1].AddInPage, 2);
                 //       for (byte i = 0; i < 2; i++)
                 //           pda430[6 + i] = datacode[i];                                             //页内地址
               

                 //       datacode = ConVertToByte(UPDATE_CODE_REG.UpCode_Seg[FileDownInfo.DownSegNo -1].Codelen , 2);
                 //       for (byte i = 0; i < 2; i++)
                 //           pda430[8 + i] = datacode[i];                                             //当前段的长度

                 //        for (byte i = 0; i < 10; i++)
                 //           PublicDataClass._DataField.Buffer[i] = pda430[i];
                 //       PublicDataClass._DataField.FieldLen += 10;
                 //       for (int i = 0; i < UPDATE_CODE_REG.UpCode_Seg[FileDownInfo.DownSegNo -1].Codelen; i++)
                 //       {
                 //           PublicDataClass._DataField.Buffer[10 + i] = UPDATE_CODE_REG.UpCode_Seg[FileDownInfo.DownSegNo - 1].Up_Code[i];  //代码赋值
                 //       }
                 //       PublicDataClass._DataField.FieldLen +=UPDATE_CODE_REG.UpCode_Seg[FileDownInfo.DownSegNo -1].Codelen;                                              
                         
                 //       }

            else if (filetype == 1)//430文件
            {
                byte[] pda = new byte[18];

                PublicDataClass._DataField.Buffer[0] = 0x00;
                PublicDataClass._DataField.Buffer[1] = 0x00;
                datacode = ConVertToByte(FileDownInfo.DownSegNo, 2);
                for (byte i = 0; i < 2; i++)
                    PublicDataClass._DataField.Buffer[2 + i] = datacode[i];                                              //当前段号
                PublicDataClass._DataField.Buffer[4] = 0x00;
                PublicDataClass._DataField.Buffer[5] = 0x00;
                //datacode = ConVertToByte(UPDATE_CODE_REG.SegNum, 2);
                //for (byte i = 0; i < 2; i++)
                //    PublicDataClass._DataField.Buffer[i] = datacode[i];                                                //总段数
              
                datacode = ConVertToByte(UPDATE_CODE_REG.UpCode_Seg[FileDownInfo.DownSegNo - 1].Codelen, 2);
                for (byte i = 0; i < 2; i++)
                    PublicDataClass._DataField.Buffer[6 + i] = datacode[i];                                             //当前段数据长度

                PublicDataClass._DataField.FieldLen = 8;
                for (int i = 0; i < UPDATE_CODE_REG.UpCode_Seg[FileDownInfo.DownSegNo - 1].Codelen; i++)
                {
                    PublicDataClass._DataField.Buffer[8 + i] = UPDATE_CODE_REG.UpCode_Seg[FileDownInfo.DownSegNo - 1].Up_Code[i];  //代码赋值
                }
                PublicDataClass._DataField.FieldLen += UPDATE_CODE_REG.UpCode_Seg[FileDownInfo.DownSegNo - 1].Codelen;

            }//end of  if (filetype = 1)//430文件
            else if (filetype == 2)//2812文件
            {
                    byte[] pda = new byte[18];

                    pda[0] = PublicDataClass._FileHeadInfo.DataBlockNum;
                    if ((FileDownInfo.DownSegNo >= 1) && (FileDownInfo.DownSegNo <= BLOCKINFO.BlockInfo[0].EndSeg))
                    {
                        pda[1] = 1;         //快号

                    }
                    if ((FileDownInfo.DownSegNo > BLOCKINFO.BlockInfo[0].EndSeg) && (FileDownInfo.DownSegNo <= BLOCKINFO.BlockInfo[1].EndSeg))
                        pda[1] = 2;
                    else if ((FileDownInfo.DownSegNo > BLOCKINFO.BlockInfo[1].EndSeg) && (FileDownInfo.DownSegNo <= BLOCKINFO.BlockInfo[2].EndSeg))
                        pda[1] = 3;
                    else if ((FileDownInfo.DownSegNo > BLOCKINFO.BlockInfo[2].EndSeg) && (FileDownInfo.DownSegNo <= BLOCKINFO.BlockInfo[3].EndSeg))
                        pda[1] = 4;
                    else if ((FileDownInfo.DownSegNo > BLOCKINFO.BlockInfo[3].EndSeg) && (FileDownInfo.DownSegNo <= BLOCKINFO.BlockInfo[4].EndSeg))
                        pda[1] = 5;
                    else if ((FileDownInfo.DownSegNo > BLOCKINFO.BlockInfo[4].EndSeg) && (FileDownInfo.DownSegNo <= BLOCKINFO.BlockInfo[5].EndSeg))
                        pda[1] = 6;
                    else if ((FileDownInfo.DownSegNo > BLOCKINFO.BlockInfo[5].EndSeg) && (FileDownInfo.DownSegNo <= BLOCKINFO.BlockInfo[6].EndSeg))
                        pda[1] = 7;
                    else if ((FileDownInfo.DownSegNo > BLOCKINFO.BlockInfo[6].EndSeg) && (FileDownInfo.DownSegNo <= BLOCKINFO.BlockInfo[7].EndSeg))
                        pda[1] = 8;
                    else if ((FileDownInfo.DownSegNo > BLOCKINFO.BlockInfo[7].EndSeg) && (FileDownInfo.DownSegNo <= BLOCKINFO.BlockInfo[8].EndSeg))
                        pda[1] = 9;
                    else if ((FileDownInfo.DownSegNo > BLOCKINFO.BlockInfo[8].EndSeg) && (FileDownInfo.DownSegNo <= BLOCKINFO.BlockInfo[9].EndSeg))
                        pda[1] = 10;
                    else if ((FileDownInfo.DownSegNo > BLOCKINFO.BlockInfo[9].EndSeg) && (FileDownInfo.DownSegNo <= BLOCKINFO.BlockInfo[10].EndSeg))
                        pda[1] = 11;

                    // pda[2] = (BLOCKINFO.BlockInfo[pda[1] - 1].blockAddr & 0xff000000) >> 24;

                    datacode = ConVertToByte(BLOCKINFO.BlockInfo[pda[1] - 1].blockAddr, 4);
                    for (byte i = 0; i < 4; i++)
                        pda[2 + i] = datacode[i];                                                //块地址
                    datacode = ConVertToByte(BLOCKINFO.BlockInfo[pda[1] - 1].blockLen, 4);
                    for (byte i = 0; i < 4; i++)
                        pda[6 + i] = datacode[i];                                              //块长度
                    datacode = ConVertToByte(PublicDataClass._FileHeadInfo.SegNum, 2);
                    for (byte i = 0; i < 2; i++)
                        pda[10 + i] = datacode[i];                                             //总段数
                    datacode = ConVertToByte(FileDownInfo.DownSegNo, 2);
                    for (byte i = 0; i < 2; i++)
                        pda[12 + i] = datacode[i];                                             //当前段

                    datacode = ConVertToByte(FILECODE.CodeByte[FileDownInfo.DownSegNo - 1].Len, 2);
                    for (byte i = 0; i < 2; i++)
                        pda[14 + i] = datacode[i];                                             //当前段的长度
                    for (byte i = 0; i < 16; i++)
                        PublicDataClass._DataField.Buffer[i] = pda[i];
                    PublicDataClass._DataField.FieldLen += 16;
                    for (int i = 0; i < FILECODE.CodeByte[FileDownInfo.DownSegNo - 1].Len; i++)
                    {
                        PublicDataClass._DataField.Buffer[16 + i] = FILECODE.CodeByte[FileDownInfo.DownSegNo - 1].FileBytes[i];  //代码赋值
                    }
                    PublicDataClass._DataField.FieldLen += FILECODE.CodeByte[FileDownInfo.DownSegNo - 1].Len;



            }//end of  if (filetype = 2)//2812文件



        }

        private byte[] ConVertToByte(int soure, byte pos)
        {
            byte[] data = new byte[pos]; 
            if (pos == 2)   //int型
            {
                data[0] = Convert.ToByte(soure & 0x00ff);
                data[1] = Convert.ToByte((soure & 0xff00) >> 8);

            }
            if (pos == 4)
            {
                data[3] =Convert.ToByte((soure &0xff000000)>>24);
                data[2] = Convert.ToByte((soure & 0x00ff0000) >> 16);
                data[1] = Convert.ToByte((soure & 0x0000ff00) >> 8);
                data[0] = Convert.ToByte((soure & 0x000000ff));
            }
            return data;

        }
        /// <summary>
        /// 定时器---的处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
    
            if (PublicDataClass._Message.CodeUpdateDoing == true)
            {

             /*    System.Threading.Thread.Sleep(1000); */
                /*   if (ty == 2)
              {
                 timer2time=1;
                 timer2.Enabled =true;
               }
           for (int time = 0; time < 400000000; time++)
            { 
                    
           }*/


                    PublicDataClass._Message.CodeUpdateDoing = false;
               
                textBoxloadstate.Text = String.Format("{0:d}", PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] >> 8));

    
                //如果升级确认帧的段号不对，重发收到的确认帧段号textBoxloadstate
                if (textBoxstartseg.Text == FileDownInfo.DownSegNo.ToString())
                {
                    FileDownInfo.DownSegNo++;
                    iDownTime++;
                    progressBar1.Increment(1);
                }
                

                if (FileDownInfo.DownSegNo > PublicDataClass._FileHeadInfo.SegNum)
                {
                    timer1.Enabled = false;
                    return;
                }
                //progressBar1.MarqueeAnimationSpeed = 100; //设定ProgressBar控件进度块在进度栏中移动的时间段
                
                //m_prgUpdate.SetPos(FileDownMsg.DownSegNo);
        


                    labelmsg.Text = "下载进度：" + String.Format("{0:d}", (FileDownInfo.DownSegNo ) * 100 / PublicDataClass._FileHeadInfo.SegNum)
            + "%  用时：" + String.Format("{0:d}秒", iDownTime);

                    textBoxstartseg.Text = String.Format("{0:d}", FileDownInfo.DownSegNo);
               PublicDataClass._DataField.FieldLen = 0;               
                DownFileSegment();

                PublicDataClass.seq ++;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = Convert.ToInt16(textBoxloadstate.Text);
                if (FileDownInfo.DownSegNo < PublicDataClass._FileHeadInfo.SegNum)
                {
                    PublicDataClass.seqflag = 1;
                }
                else
                {
                    PublicDataClass.seqflag = 0;
                }



                if (filetype == 2 || filetype == 1) //2812文件
                {
                    if (ty == 1)
                        PublicDataClass._ComTaskFlag.UpdateCode_Start_1 = true;
                    if (ty == 2)
                        PublicDataClass._NetTaskFlag.UpdateCode_Start_1 = true;
                    if (ty == 3)
                        PublicDataClass._GprsTaskFlag.UpdateCode_Start_1 = true;
                }
                else if (filetype == 4) //ARM文件
                {
                    if (ty == 1)
                      PublicDataClass._ComTaskFlag.UpdateCode_ARMStart_1 = true;
                        
                    if (ty == 2)
                        PublicDataClass._NetTaskFlag.UpdateCode_ARMStart_1 = true;
                    if (ty == 3)
                        PublicDataClass._GprsTaskFlag.UpdateCode_ARMStart_1 = true;
                }
                
               /*
                if (progressBar1.Value == progressBar1.Maximum)
                {
                    timer1.Stop();
                    MessageBox.Show("Server has been connected");
                    this.Close();
                    timer1.Stop();
                }*/

            }
            if (PublicDataClass._Message.CodeUpdateJY == true)
            {
                PublicDataClass._Message.CodeUpdateJY = false;
                if (PublicDataClass._DataField.Buffer[0] == 0x55)
                    MessageBox.Show("校验正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (PublicDataClass._DataField.Buffer[0] == 0xaa)
                    MessageBox.Show("校验错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                timer1.Enabled = false;
            }
            if (PublicDataClass._Message.CodeUpdateARMJY == true) //ARM校验
            {
                PublicDataClass._Message.CodeUpdateARMJY = false;

                if (PublicDataClass._FileHeadInfo.CheckSum == PublicDataClass._DataField.Buffer[2])
                {
                    MessageBox.Show("校验正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    buttonupdata.Enabled = true;
                }
                else
                {
                    MessageBox.Show("校验错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    buttonupdata.Enabled = false;
                }
                timer1.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                comboBoxaddr.Enabled = true;
            else
                comboBoxaddr.Enabled = false;
            PublicDataClass.addselect = 0;   //是否应加上大括号？
        }

        private void comboBoxaddr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxaddr.SelectedIndex == 0)
                PublicDataClass.addselect = 5;
            else if (comboBoxaddr.SelectedIndex == 1)
                PublicDataClass.addselect = 1;
            else if (comboBoxaddr.SelectedIndex == 2)
                PublicDataClass.addselect = 2;
            else if (comboBoxaddr.SelectedIndex == 3)
                PublicDataClass.addselect = 3;
            else if (comboBoxaddr.SelectedIndex == 4)
                PublicDataClass.addselect = 4;
            
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2time--;
            if (timer2time == 0)
                timer2.Enabled = false;
        }

        private void CodeUpdateViewForm_Deactivate(object sender, EventArgs e)
        {
            //timer1.Enabled = false;
            //buttonstart.Enabled = true;
            //buttonstop.Enabled = false;
       //     timer3.Enabled = false;
        }

        private void CodeUpdateViewForm_VisibleChanged(object sender, EventArgs e)
        {
            //timer1.Enabled = false;
            //buttonstart.Enabled = true;
            //buttonstop.Enabled = false;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            timer3.Enabled = false;
            if (PublicDataClass._Message.LinkUpdata == true)//通道更新
            {
                PublicDataClass._Message.LinkUpdata = false;

            if (PublicDataClass.LinSPointName == "无信息")
            {
                MessageBox.Show("无测量点信息可操作", "信息",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            
            }
        }

        private void CodeUpdateViewForm_Activated(object sender, EventArgs e)
        {
      //      timer3.Enabled = true;
        }

    }
}
