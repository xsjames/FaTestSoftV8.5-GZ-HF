using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

using System.Windows.Forms;
using KD.WinFormsUI.Docking;
using FaTestSoft.CommonData;

namespace FaTestSoft
{
    public partial class OutputWindows : DockContent
    {



        public OutputWindows()
        {
            InitializeComponent();


        }
        int mymin, initmin;
        static int Type;
        static string Msg = @"";
        static string DataInfo = @"";
        static int index = 17;
        int Pos = 0;
        int data = 0;
        float fdata = 0.0F;
        byte[] bytes = new byte[4];
        private StringBuilder strB;

        /// <summary>
        ///  定时器的处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            int thei = 0;
            if (PublicDataClass._Message.ComShowSendTextData == true)
            {
                strB = new StringBuilder();
                strB.Append(DateTime.Now.ToString() + " ");
                strB.Append("[" + PublicDataClass.COMID + "]" + "Send：" + "(" + PublicDataClass.ComFrameMsg + ")");

                if (PublicDataClass._ProtocoltyFlag.ComProFlag == 2)//串口走104规约显示具体信息
                {
                    if (PublicDataClass._NetStructData.NetTBuffer[0] == 0x69)
                        PublicDataClass.OutPutMessage.S_type = 0;
                    if (PublicDataClass.OutPutMessage.S_type == 1)
                        strB.Append("(U帧) ");
                    if (PublicDataClass.OutPutMessage.S_type == 2)
                    {
                        strB.Append("(I帧) " + " 发送序号:" + PublicDataClass.OutPutMessage.S_txseq + " 接收序号:" + PublicDataClass.OutPutMessage.S_rxseq + " TypeID:" + PublicDataClass.OutPutMessage.S_TypeID + " COT:" + PublicDataClass.OutPutMessage.S_COT + " VSQ:" + PublicDataClass.OutPutMessage.S_VSQ);
                        strB.Append("\r\n");
                    }
                    if (PublicDataClass.OutPutMessage.S_type == 3)
                    {
                        strB.Append("(S帧) " + " 接收序号:" + PublicDataClass.OutPutMessage.S_rxseq);
                        strB.Append("\r\n");

                    }

                }
                for (int i = 0; i < PublicDataClass._ComStructData.TxLen; i++)
                {
                    strB.Append(PublicDataClass._ComStructData.TXBuffer[i].ToString("X2"));
                    strB.Append(" ");
                }
                strB.Append("\r\n");
                Msg = String.Empty;
                Msg = strB.ToString();
                richTextBoxExtended1.LogMessage(Msg);

                PublicDataClass._Message.ComShowSendTextData = false;
            }
            else if (PublicDataClass._Message.ComShowRecvTextData == true)                               //串口收
            {

                strB = new StringBuilder();
                strB.Append(DateTime.Now.ToString() + " ");
                strB.Append("[" + PublicDataClass.COMID + "]" + "Recv：" + "(" + PublicDataClass.ComFrameMsg + ")");

                if (PublicDataClass._ProtocoltyFlag.ComProFlag == 2)//串口走104规约显示具体信息
                {
                    if (PublicDataClass._ComStructData.RXBuffer[0] == 0x69)
                        PublicDataClass.OutPutMessage.type = 0;
                    if (PublicDataClass.OutPutMessage.type == 1)
                        strB.Append("(U帧) ");
                    if (PublicDataClass.OutPutMessage.type == 2)
                    {
                        strB.Append("(I帧) " + "数据单元长度:" + PublicDataClass.OutPutMessage.length + " 发送序号:" + PublicDataClass.OutPutMessage.txseq + " 接收序号:" + PublicDataClass.OutPutMessage.rxseq + " TypeID:" + PublicDataClass.OutPutMessage.TypeID + " COT:" + PublicDataClass.OutPutMessage.COT + " VSQ:" + PublicDataClass.OutPutMessage.VSQ);
                        strB.Append("\r\n");

                    }
                    if (PublicDataClass.OutPutMessage.type == 3)
                    {
                        strB.Append("(S帧) " + " 接收序号:" + PublicDataClass.OutPutMessage.rxseq);
                        strB.Append("\r\n");
                    }

                    if ((PublicDataClass.DataTy == 35) || (PublicDataClass.DataTy == 40))
                        index = 19;
                    if (PublicDataClass.DataTy == 36)
                        index = 17;
                    if (PublicDataClass.DataTy == 38)
                        index = 17;
                    if (PublicDataClass.DataTy == 42)
                        index = 16;
                    if ((PublicDataClass.DataTy == 50) || (PublicDataClass.DataTy == 51) || (PublicDataClass.DataTy == 52)
                                                       || (PublicDataClass.DataTy == 53) || (PublicDataClass.DataTy == 54))
                        index = 15;
                    if ((PublicDataClass.DataTy == 56) || (PublicDataClass.DataTy == 58))
                        index = 22;
                    if (PublicDataClass.DataTy == 41)
                        index = 19;
                    if (PublicDataClass.DataTy == 37)
                        index = 17;
                    if (PublicDataClass.DataTy == 39)
                        index = 17;
                    if (PublicDataClass.DataTy == 43)
                        index = 16;
                    for (int i = 0; i < PublicDataClass._ComStructData.RxLen; i++)
                    {
                        //    Msg += String.Format("{0:x2}", PublicDataClass._ComStructData.RXBuffer[i]);
                        strB.Append(PublicDataClass._ComStructData.RXBuffer[i].ToString("X2"));

                        if ((PublicDataClass.ComFrameMsg == "遥信") && (i == index))
                        {
                            if (i == 15)
                            {

                                Pos = PublicDataClass._DataField.Buffer[2];
                                Pos = Pos << 16;
                                Pos += PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);

                                if (PublicDataClass.DataTy == 53)
                                {
                                    if (PublicDataClass._ComStructData.RXBuffer[i] == 0x01)
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：分>" + " ");
                                    else
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：合>" + " ");
                                }
                                else
                                {
                                    if (PublicDataClass._ComStructData.RXBuffer[i] == 0x00)
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：分>" + " ");
                                    else
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：合>" + " ");
                                }
                            }
                            else
                            {
                                if (PublicDataClass.DataTy == 53)
                                {
                                    if (PublicDataClass._ComStructData.RXBuffer[i] == 0x01)
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：分>" + " ");
                                    else
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：合>" + " ");
                                }
                                else
                                {
                                    if (PublicDataClass._ComStructData.RXBuffer[i] == 0x00)
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：分>" + " ");
                                    else
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：合>" + " ");
                                }

                            }
                            index += 1;
                            Pos++;
                        }
                        else if ((PublicDataClass.ComFrameMsg == "遥测") && (i == index))
                        {

                            //if (i == 19)
                            //{
                            if ((PublicDataClass.DataTy == 35) || (PublicDataClass.DataTy == 40))  //浮点型
                            {

                                Pos = PublicDataClass._DataField.Buffer[2];
                                Pos = Pos << 16;
                                Pos += PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);

                                //data = PublicDataClass._DataField.Buffer[index - 12];
                                //data = data << 16;
                                //data = PublicDataClass._DataField.Buffer[index - 14] + (PublicDataClass._DataField.Buffer[index - 13] << 8);
                                //if (data > 0x8000)
                                //    data = data - 65536; 
                                //Msg += "<" + String.Format("{0:d}", Pos - 16385) + "：" + String.Format("{0:d}", data) + ">" + " ";
                                bytes[0] = PublicDataClass._DataField.Buffer[index - 16];
                                bytes[1] = PublicDataClass._DataField.Buffer[index - 15];
                                bytes[2] = PublicDataClass._DataField.Buffer[index - 14];
                                bytes[3] = PublicDataClass._DataField.Buffer[index - 13];

                                fdata = BitConverter.ToSingle(bytes, 0);
                                if ((Pos - 16385 + thei < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 + thei >= 0))
                                    PublicDataClass._Curve.ycdata[Pos - 16385 + thei] = Convert.ToDouble(fdata);
                                strB.Append("<" + String.Format("{0:d}", Pos - 16385 + thei) + "：" + String.Format("{0:f4}", fdata) + ">" + " ");
                                index += 5;
                                Pos++;
                                thei++;

                            }
                            if ((PublicDataClass.DataTy == 36) || (PublicDataClass.DataTy == 38))     //带品质描述归一化值
                            {


                                Pos = PublicDataClass._DataField.Buffer[2];
                                Pos = Pos << 16;
                                Pos += PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);


                                data = PublicDataClass._DataField.Buffer[index - 14] + (PublicDataClass._DataField.Buffer[index - 13] << 8);
                                if ((Pos - 16385 + thei < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 + thei >= 0))
                                    PublicDataClass._Curve.ycdata[Pos - 16385 + thei] = Convert.ToDouble(data);

                                strB.Append("<" + String.Format("{0:d}", Pos - 16385 + thei) + "：" + String.Format("{0:d}", data) + ">" + " ");

                                index += 3;
                                Pos++;
                                thei++;
                            }
                            if (PublicDataClass.DataTy == 42)     //不带品质描述归一化值
                            {


                                Pos = PublicDataClass._DataField.Buffer[2];
                                Pos = Pos << 16;
                                Pos += PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);


                                data = PublicDataClass._DataField.Buffer[index - 13] + (PublicDataClass._DataField.Buffer[index - 12] << 8);
                                if ((Pos - 16385 + thei < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 + thei >= 0))
                                    PublicDataClass._Curve.ycdata[Pos - 16385 + thei] = Convert.ToDouble(data);
                                strB.Append("<" + String.Format("{0:d}", Pos - 16385 + thei) + "：" + String.Format("{0:d}", data) + ">" + " ");

                                index += 2;
                                Pos++;
                                thei++;
                            }

                            //else
                            //{
                            //    data = PublicDataClass._DataField.Buffer[index - 12];
                            //    data = data << 16;
                            //    data =PublicDataClass._DataField.Buffer[index - 14] + (PublicDataClass._DataField.Buffer[index - 13] << 8);
                            //    if (data > 0x8000)
                            //        data = data - 65536;
                            //    /*bytes[0] = PublicDataClass._DataField.Buffer[index - 16];
                            //    bytes[1] = PublicDataClass._DataField.Buffer[index - 15];
                            //    bytes[2] = PublicDataClass._DataField.Buffer[index - 14];
                            //    bytes[3] = PublicDataClass._DataField.Buffer[index - 13];

                            //    fdata = BitConverter.ToSingle(bytes, 0);*/
                            //    Msg += "<" + String.Format("{0:d}", Pos - 16385) + "：" + String.Format("{0:d}", data) + ">" + " ";


                            //}

                        }
                        else if ((PublicDataClass.ComFrameMsg == "扰动事件") && (i == index))
                        {

                            //if (i == 19)
                            //{
                            if ((PublicDataClass.DataTy == 41))  //浮点型扰动
                            {

                                Pos = PublicDataClass._DataField.Buffer[index - 17];
                                Pos = Pos << 16;
                                Pos += PublicDataClass._DataField.Buffer[index - 19] + (PublicDataClass._DataField.Buffer[index - 18] << 8);
                                bytes[0] = PublicDataClass._DataField.Buffer[index - 16];
                                bytes[1] = PublicDataClass._DataField.Buffer[index - 15];
                                bytes[2] = PublicDataClass._DataField.Buffer[index - 14];
                                bytes[3] = PublicDataClass._DataField.Buffer[index - 13];

                                fdata = BitConverter.ToSingle(bytes, 0);
                                if ((Pos - 16385 < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 >= 0))
                                    PublicDataClass._Curve.ycdata[Pos - 16385] = Convert.ToDouble(fdata);
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：" + String.Format("{0:f4}", fdata) + ">" + " ");
                                index += 8;

                            }
                            if ((PublicDataClass.DataTy == 37) || (PublicDataClass.DataTy == 39))    //带品质描述归一化值扰动
                            {


                                Pos = PublicDataClass._DataField.Buffer[index - 15];
                                Pos = Pos << 16;
                                Pos += PublicDataClass._DataField.Buffer[index - 17] + (PublicDataClass._DataField.Buffer[index - 16] << 8);


                                data = PublicDataClass._DataField.Buffer[index - 14] + (PublicDataClass._DataField.Buffer[index - 13] << 8);
                                if ((Pos - 16385 < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 >= 0))
                                    PublicDataClass._Curve.ycdata[Pos - 16385] = Convert.ToDouble(data);
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：" + String.Format("{0:d}", data) + ">" + " ");

                                index += 6;

                            }
                            if (PublicDataClass.DataTy == 43)     //带品质描述归一化值扰动
                            {


                                Pos = PublicDataClass._DataField.Buffer[index - 14];
                                Pos = Pos << 16;
                                Pos += PublicDataClass._DataField.Buffer[index - 16] + (PublicDataClass._DataField.Buffer[index - 15] << 8);


                                data = PublicDataClass._DataField.Buffer[index - 13] + (PublicDataClass._DataField.Buffer[index - 12] << 8);
                                if (Pos - 16385 < PublicDataClass.SaveText.Cfg[0].YccfgNum)
                                    if ((Pos - 16385 < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 >= 0))
                                        strB.Append("<" + String.Format("{0:d}", Pos) + "：" + String.Format("{0:d}", data) + ">" + " ");

                                index += 5;

                            }



                        }
                        else if ((PublicDataClass.DataTy == 52) && i == index)  //不带时标遥信和遥信变位
                        {

                            Pos = PublicDataClass._DataField.Buffer[index - 13];
                            Pos = Pos << 16;
                            Pos += PublicDataClass._DataField.Buffer[index - 15] + (PublicDataClass._DataField.Buffer[index - 14] << 8);

                            if (PublicDataClass._DataField.Buffer[index - 12] == 0)
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：分>" + " ");
                            else
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：合>" + " ");
                            index += 4;

                        }
                        else if ((PublicDataClass.DataTy == 54) && i == index)  //不带时标遥信和遥信变位
                        {

                            Pos = PublicDataClass._DataField.Buffer[index - 13];
                            Pos = Pos << 16;
                            Pos += PublicDataClass._DataField.Buffer[index - 15] + (PublicDataClass._DataField.Buffer[index - 14] << 8);

                            if (PublicDataClass._DataField.Buffer[index - 12] == 1)
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：分>" + " ");
                            else
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：合>" + " ");
                            index += 4;

                        }
                        else if (PublicDataClass.DataTy == 56 && i == index) //带时标的遥信变位
                        {
                            Pos = PublicDataClass._DataField.Buffer[index - 20];
                            Pos = Pos << 16;
                            Pos += PublicDataClass._DataField.Buffer[index - 22] + (PublicDataClass._DataField.Buffer[index - 21] << 8);

                            if (PublicDataClass._DataField.Buffer[index - 19] == 0)
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：分 ");
                            else
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：合 ");

                            DataInfo = "";
                            DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - 12]);   //年
                            DataInfo += "年";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - 13]);  //月
                            DataInfo += "月";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - 14] & 0x1f);  //日+星期
                            DataInfo += "日";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - 15]);  //时
                            DataInfo += "时";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - 16]);  //分
                            DataInfo += "分";
                            int ms = (PublicDataClass._DataField.Buffer[index - 17] << 8) + PublicDataClass._DataField.Buffer[index - 18];

                            DataInfo += String.Format("{0:d}", ms);
                            DataInfo += "毫秒" + ">";
                            strB.Append(DataInfo);

                            index += 11;


                        }
                        else if (PublicDataClass.DataTy == 58 && i == index) //带时标的双点遥信变位
                        {
                            Pos = PublicDataClass._DataField.Buffer[index - 20];
                            Pos = Pos << 16;
                            Pos += PublicDataClass._DataField.Buffer[index - 22] + (PublicDataClass._DataField.Buffer[index - 21] << 8);

                            if (PublicDataClass._DataField.Buffer[index - 19] == 1)
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：分 ");
                            else
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：合 ");

                            DataInfo = "";
                            DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - 12]);   //年
                            DataInfo += "年";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - 13]);  //月
                            DataInfo += "月";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - 14] & 0x1f);  //日+星期
                            DataInfo += "日";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - 15]);  //时
                            DataInfo += "时";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - 16]);  //分
                            DataInfo += "分";
                            int ms = (PublicDataClass._DataField.Buffer[index - 17] << 8) + PublicDataClass._DataField.Buffer[index - 18];

                            DataInfo += String.Format("{0:d}", ms);
                            DataInfo += "毫秒" + ">";
                            strB.Append(DataInfo);

                            index += 11;


                        }
                        else
                            strB.Append(" ");
                    }
                }//end of  if (PublicDataClass._ProtocoltyFlag.ComProFlag == 2)

                if (PublicDataClass._ProtocoltyFlag.ComProFlag == 1)
                {
                    if (PublicDataClass.DataTy == 36)
                        index = 9 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen;
                    if (PublicDataClass.DataTy == 38)
                        index = 9 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen;
                    if ((PublicDataClass.DataTy == 40) || (PublicDataClass.DataTy == 35))
                        index = 11 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen;
                    if (PublicDataClass.DataTy == 42)
                        index = 8 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen;

                    if (PublicDataClass.DataTy == 37)
                        index = 9 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen;
                    if (PublicDataClass.DataTy == 39)
                        index = 9 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen;
                    if (PublicDataClass.DataTy == 41)
                        index = 11 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen;
                    if (PublicDataClass.DataTy == 43)
                        index = 8 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen;

                    if ((PublicDataClass.DataTy == 50) || (PublicDataClass.DataTy == 51) || (PublicDataClass.DataTy == 52)  //单点遥信
                                                     || (PublicDataClass.DataTy == 53) || (PublicDataClass.DataTy == 54)) //双点遥信
                        index = 7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen;
                    if ((PublicDataClass.DataTy == 56) || (PublicDataClass.DataTy == 58))
                        index = 14 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen;

                    for (int i = 0; i < PublicDataClass._ComStructData.RxLen; i++)
                    {

                        strB.Append(PublicDataClass._ComStructData.RXBuffer[i].ToString("X2"));
                        if ((PublicDataClass.ComFrameMsg == "遥信") && (i == index) && (i < PublicDataClass._ComStructData.RxLen - 2))
                        {
                            if ((PublicDataClass.inflen == 2) && (i == 7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen))
                            {
                                Pos = PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);

                                if (PublicDataClass.DataTy == 53)
                                {
                                    if (PublicDataClass._ComStructData.RXBuffer[i] == 0x01)
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：分>" + " ");
                                    else
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：合>" + " ");
                                }
                                else
                                {
                                    if (PublicDataClass._ComStructData.RXBuffer[i] == 0x00)
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：分>" + " ");
                                    else
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：合>" + " ");
                                }
                            }
                            else if ((PublicDataClass.inflen == 3) && (i == 7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen))
                            {
                                Pos = PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);

                                if (PublicDataClass.DataTy == 53)
                                {
                                    if (PublicDataClass._ComStructData.RXBuffer[i] == 0x01)
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：分>" + " ");
                                    else
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：合>" + " ");
                                }
                                else
                                {
                                    if (PublicDataClass._ComStructData.RXBuffer[i] == 0x00)
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：分>" + " ");
                                    else
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：合>" + " ");
                                }
                            }
                            else
                            {
                                if (PublicDataClass.DataTy == 53)
                                {
                                    if (PublicDataClass._ComStructData.RXBuffer[i] == 0x01)
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：分>" + " ");
                                    else
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：合>" + " ");
                                }
                                else
                                {
                                    if (PublicDataClass._ComStructData.RXBuffer[i] == 0x00)
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：分>" + " ");
                                    else
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：合>" + " ");
                                }

                            }
                            index += 1;
                            Pos++;
                        }
                        else if ((PublicDataClass.ComFrameMsg == "遥测") && (i == index) && (i < PublicDataClass._ComStructData.RxLen - 2))
                        {
                            if ((PublicDataClass.DataTy == 35) || (PublicDataClass.DataTy == 40))  //浮点型
                            {
                                Pos = PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);
                                bytes[0] = PublicDataClass._DataField.Buffer[index - (9 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)];
                                bytes[1] = PublicDataClass._DataField.Buffer[index - (8 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)];
                                bytes[2] = PublicDataClass._DataField.Buffer[index - (7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)];
                                bytes[3] = PublicDataClass._DataField.Buffer[index - (6 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)];
                                fdata = BitConverter.ToSingle(bytes, 0);
                                if ((Pos - 16385 + thei < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 + thei >= 0))
                                    PublicDataClass._Curve.ycdata[Pos - 16385 + thei] = Convert.ToDouble(fdata);
                                strB.Append("<" + String.Format("{0:d}", Pos - 16385 + thei) + "：" + String.Format("{0:f4}", fdata) + ">" + " ");
                                index += 5;
                                Pos++;
                                thei++;

                            }
                            if ((PublicDataClass.DataTy == 36) || (PublicDataClass.DataTy == 38))     //带品质描述归一化值
                            {
                                Pos = PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);
                                data = PublicDataClass._DataField.Buffer[index - (7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]
                                     + (PublicDataClass._DataField.Buffer[index - (6 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);
                                if ((Pos - 16385 + thei < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 + thei >= 0))
                                    PublicDataClass._Curve.ycdata[Pos - 16385 + thei] = Convert.ToDouble(data);

                                strB.Append("<" + String.Format("{0:d}", Pos - 16385 + thei) + "：" + String.Format("{0:d}", data) + ">" + " ");

                                index += 3;
                                Pos++;
                                thei++;
                            }
                            if (PublicDataClass.DataTy == 42)     //不带品质描述归一化值
                            {
                                Pos = PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);


                                data = PublicDataClass._DataField.Buffer[index - (6 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]
                                    + (PublicDataClass._DataField.Buffer[index - (5 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);
                                if ((Pos - 16385 + thei < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 + thei >= 0))
                                    PublicDataClass._Curve.ycdata[Pos - 16385 + thei] = Convert.ToDouble(data);
                                strB.Append("<" + String.Format("{0:d}", Pos - 16385 + thei) + "：" + String.Format("{0:d}", data) + ">" + " ");

                                index += 2;
                                Pos++;
                                thei++;
                            }

                        }
                        else if ((PublicDataClass.ComFrameMsg == "扰动事件") && (i == index) && (i < PublicDataClass._ComStructData.RxLen - 2))
                        {

                            if ((PublicDataClass.DataTy == 41))  //浮点型扰动
                            {

                                Pos = PublicDataClass._DataField.Buffer[index - (11 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]
                                    + (PublicDataClass._DataField.Buffer[index - (10 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);
                                bytes[0] = PublicDataClass._DataField.Buffer[index - (9 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)];
                                bytes[1] = PublicDataClass._DataField.Buffer[index - (8 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)];
                                bytes[2] = PublicDataClass._DataField.Buffer[index - (7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)];
                                bytes[3] = PublicDataClass._DataField.Buffer[index - (6 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)];

                                fdata = BitConverter.ToSingle(bytes, 0);
                                if ((Pos - 16385 < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 >= 0))
                                    PublicDataClass._Curve.ycdata[Pos - 16385] = Convert.ToDouble(fdata);
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：" + String.Format("{0:f4}", fdata) + ">" + " ");
                                if (PublicDataClass.inflen == 2)
                                {
                                    index += 7;
                                }
                                else if (PublicDataClass.inflen == 3)
                                {
                                    index += 8;
                                }

                            }
                            if ((PublicDataClass.DataTy == 37) || (PublicDataClass.DataTy == 39))    //带品质描述归一化值扰动
                            {

                                Pos = PublicDataClass._DataField.Buffer[index - (9 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]
                                    + (PublicDataClass._DataField.Buffer[index - (8 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);


                                data = PublicDataClass._DataField.Buffer[index - (7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]
                                      + (PublicDataClass._DataField.Buffer[index - (6 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);
                                if ((Pos - 16385 < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 >= 0))
                                    PublicDataClass._Curve.ycdata[Pos - 16385] = Convert.ToDouble(data);
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：" + String.Format("{0:d}", data) + ">" + " ");
                                if (PublicDataClass.inflen == 2)
                                {
                                    index += 5;
                                }
                                else if (PublicDataClass.inflen == 3)
                                {
                                    index += 6;
                                }

                            }
                            if (PublicDataClass.DataTy == 43)     //不带品质描述归一化值扰动
                            {

                                Pos = PublicDataClass._DataField.Buffer[index - (8 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]
                                    + (PublicDataClass._DataField.Buffer[index - (7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);
                                data = PublicDataClass._DataField.Buffer[index - (6 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]
                                    + (PublicDataClass._DataField.Buffer[index - (5 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);
                                if (Pos - 16385 < PublicDataClass.SaveText.Cfg[0].YccfgNum)
                                    if ((Pos - 16385 < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 >= 0))
                                        strB.Append("<" + String.Format("{0:d}", Pos) + "：" + String.Format("{0:d}", data) + ">" + " ");

                                if (PublicDataClass.inflen == 2)
                                {
                                    index += 4;
                                }
                                else if (PublicDataClass.inflen == 3)
                                {
                                    index += 5;
                                }


                            }



                        }
                        else if ((PublicDataClass.DataTy == 52) && (i == index) && (i < PublicDataClass._ComStructData.RxLen - 2))  //不带时标遥信和遥信变位
                        {
                            Pos = PublicDataClass._DataField.Buffer[index - (7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] + (PublicDataClass._DataField.Buffer[index - (6 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);

                            if (PublicDataClass._DataField.Buffer[index - (5 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] == 0)
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：分>" + " ");
                            else
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：合>" + " ");
                            if (PublicDataClass.inflen == 2)
                            {
                                index += 3;
                            }
                            else if (PublicDataClass.inflen == 3)
                            {
                                index += 4;
                            }

                        }
                        else if ((PublicDataClass.DataTy == 54) && (i == index) && (i < PublicDataClass._ComStructData.RxLen - 2))  //不带时标遥信和遥信变位
                        {
                            Pos = PublicDataClass._DataField.Buffer[index - (7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] + (PublicDataClass._DataField.Buffer[index - (6 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);

                            if (PublicDataClass._DataField.Buffer[index - (5 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] == 1)
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：分>" + " ");
                            else
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：合>" + " ");
                            if (PublicDataClass.inflen == 2)
                            {
                                index += 3;
                            }
                            else if (PublicDataClass.inflen == 3)
                            {
                                index += 4;
                            }

                        }
                        else if (PublicDataClass.DataTy == 56 && (i == index) && (i < PublicDataClass._ComStructData.RxLen - 2)) //带时标的遥信变位
                        {

                            Pos = PublicDataClass._DataField.Buffer[index - (14 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] +
                                     (PublicDataClass._DataField.Buffer[index - (13 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);

                            if (PublicDataClass._DataField.Buffer[index - (12 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] == 0)
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：分 ");
                            else
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：合 ");

                            DataInfo = "";
                            DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - (5 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]);   //年
                            DataInfo += "年";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - (6 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]);  //月
                            DataInfo += "月";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - (7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] & 0x1f);  //日+星期
                            DataInfo += "日";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - (8 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]);  //时
                            DataInfo += "时";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - (9 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]);  //分
                            DataInfo += "分";
                            int ms = (PublicDataClass._DataField.Buffer[index - (10 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8)
                                      + PublicDataClass._DataField.Buffer[index - (11 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)];

                            DataInfo += String.Format("{0:d}", ms);
                            DataInfo += "毫秒" + ">";
                            strB.Append(DataInfo);
                            if (PublicDataClass.inflen == 2)
                            {
                                index += 10;
                            }
                            else if (PublicDataClass.inflen == 3)
                            {
                                index += 11;
                            }


                        }
                        else if (PublicDataClass.DataTy == 58 && (i == index) && (i < PublicDataClass._ComStructData.RxLen - 2)) //带时标的双点遥信变位
                        {

                            Pos = PublicDataClass._DataField.Buffer[index - (14 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] +
                                     (PublicDataClass._DataField.Buffer[index - (13 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);

                            if (PublicDataClass._DataField.Buffer[index - (12 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] == 1)
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：分 ");
                            else
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：合 ");

                            DataInfo = "";
                            DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - (5 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]);   //年
                            DataInfo += "年";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - (6 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]);  //月
                            DataInfo += "月";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - (7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] & 0x1f);  //日+星期
                            DataInfo += "日";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - (8 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]);  //时
                            DataInfo += "时";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - (9 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]);  //分
                            DataInfo += "分";
                            int ms = (PublicDataClass._DataField.Buffer[index - (10 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8)
                                      + PublicDataClass._DataField.Buffer[index - (11 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)];

                            DataInfo += String.Format("{0:d}", ms);
                            DataInfo += "毫秒" + ">";
                            strB.Append(DataInfo);
                            if (PublicDataClass.inflen == 2)
                            {
                                index += 10;
                            }
                            else if (PublicDataClass.inflen == 3)
                            {
                                index += 11;
                            }


                        }
                        else
                            strB.Append(" ");
                    }
                }// end of if (PublicDataClass._ProtocoltyFlag.ComProFlag == 1)


                if (PublicDataClass.ComFrameMsg == "无效")     //非法帧
                {
                    strB.Append("\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.LogError(Msg);
                }
                else if (PublicDataClass.ComFrameMsg == "扰动事件" || PublicDataClass.ComFrameMsg == "变位事件")     //
                {
                    strB.Append("\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.LogWarning(Msg);
                    PublicDataClass._Message.RealTimeDataDocmentView = true;
                }
                else if (PublicDataClass.ComFrameMsg == "时间")
                {
                    DataInfo = "";
                    DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 1]);   //年
                    DataInfo += "年";
                    DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 2]);  //月
                    DataInfo += "月";
                    DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 3] & 0x1f);  //日+星期
                    DataInfo += "日";
                    DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 4]);  //时
                    DataInfo += "时";
                    DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 5]);  //分
                    DataInfo += "分";
                    int ms = (PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 6] << 8) +
                          PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 7];

                    DataInfo += String.Format("{0:d}", ms);
                    DataInfo += "毫秒";
                    strB.Append("<" + DataInfo + ">" + "\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.AckMessage(Msg);
                }
                else if (PublicDataClass.ComFrameMsg == "升级(应答)")
                {
                    DataInfo = "";
                    DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8));
                    strB.Append("<第" + DataInfo + "段>" + "\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.AckMessage(Msg);
                    PublicDataClass._Message.CodeUpdateDoing = true;

                }
                else if (PublicDataClass.ComFrameMsg == "校验(应答)")
                {
                    Msg = strB.ToString();
                    richTextBoxExtended1.AckMessage(Msg);
                }
                else if (PublicDataClass.ComFrameMsg == "参数设置（确认）")
                {
                    strB.Append("\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.AckMessage(Msg);

                }
                else if (PublicDataClass.ComFrameMsg == "参数设置（否认）")
                {
                    strB.Append("\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.LogError(Msg);
                }
                else if (PublicDataClass.ComFrameMsg == "遥测" || PublicDataClass.ComFrameMsg == "遥信" || PublicDataClass.ComFrameMsg == "浮点遥测")     //
                {
                    strB.Append("\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.ThreeYMessage(Msg);
                    PublicDataClass._Message.RealTimeDataDocmentView = true;    //zxl
                }
                else if (PublicDataClass.ComFrameMsg == "选择应答" || PublicDataClass.ComFrameMsg == "执行成功")     //
                {
                    strB.Append("\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.YkAckMessage(Msg);
                }
                else if (PublicDataClass.ComFrameMsg == "遥控撤销(确认)")     //
                {
                    strB.Append("\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.YkStopMessage(Msg);
                }
                else
                {
                    strB.Append("\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString(); richTextBoxExtended1.AckMessage(Msg);
                }

                PublicDataClass._Message.ComShowRecvTextData = false;
            }

            else if (PublicDataClass._Message.NetShowSendTextData == true)
            {
                PublicDataClass._Message.NetShowSendTextData = false;

                strB = new StringBuilder();
                strB.Append(DateTime.Now.ToString() + " ");
                strB.Append("[" + PublicDataClass.IPADDR + "：" + PublicDataClass.PORT + "]" + "Send：" + "(" + PublicDataClass.SedNetFrameMsg + ")");
                if (PublicDataClass._NetStructData.NetTBuffer[0] == 0x69)
                    PublicDataClass.OutPutMessage.S_type = 0;
                if (PublicDataClass.OutPutMessage.S_type == 1)
                    strB.Append("(U帧) ");
                if (PublicDataClass.OutPutMessage.S_type == 2)
                {

                    strB.Append("(I帧) " + " 发送序号:" + PublicDataClass.OutPutMessage.S_txseq + " 接收序号:" + PublicDataClass.OutPutMessage.S_rxseq + " TypeID:" + PublicDataClass.OutPutMessage.S_TypeID + " COT:" + PublicDataClass.OutPutMessage.S_COT + " VSQ:" + PublicDataClass.OutPutMessage.S_VSQ);

                    strB.Append("\r\n");


                }
                if (PublicDataClass.OutPutMessage.S_type == 3)
                {

                    strB.Append("(S帧) " + " 接收序号:" + PublicDataClass.OutPutMessage.S_rxseq);

                    strB.Append("\r\n");


                }
                for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                {
                    strB.Append(String.Format("{0:x2}", PublicDataClass._NetStructData.NetTBuffer[i]));
                    strB.Append(" ");
                }
                strB.Append("\r\n");
                Msg = String.Empty;
                Msg = strB.ToString();
                richTextBoxExtended1.LogMessage(Msg);
                PublicDataClass._Message.NetShowSendOver = false;

            }

            else if (PublicDataClass._Message.NetShowRecvTextData == true)
            {



                strB = new StringBuilder();
                strB.Append(DateTime.Now.ToString() + " ");
                strB.Append("[" + PublicDataClass.IPADDR + "：" + PublicDataClass.PORT + "]" + "Recv：" + "(" + PublicDataClass.RevNetFrameMsg + ")");
                if (PublicDataClass._NetStructData.NetRBuffer[0] == 0x69)
                    PublicDataClass.OutPutMessage.type = 0;
                if (PublicDataClass.OutPutMessage.type == 1)
                    strB.Append("(U帧) ");
                if (PublicDataClass.OutPutMessage.type == 2)
                {

                    strB.Append("(I帧) " + "数据单元长度:" + PublicDataClass.OutPutMessage.length + " 发送序号:" + PublicDataClass.OutPutMessage.txseq + " 接收序号:" + PublicDataClass.OutPutMessage.rxseq + " TypeID:" + PublicDataClass.OutPutMessage.TypeID + " COT:" + PublicDataClass.OutPutMessage.COT + " VSQ:" + PublicDataClass.OutPutMessage.VSQ);

                    strB.Append("\r\n");


                }
                if (PublicDataClass.OutPutMessage.type == 3)
                {

                    strB.Append("(S帧) " + " 接收序号:" + PublicDataClass.OutPutMessage.rxseq);

                    strB.Append("\r\n");


                }
                if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)//网口104显示
                {
                    if ((PublicDataClass.DataTy == 35) || (PublicDataClass.DataTy == 40))
                        index = 19;
                    if (PublicDataClass.DataTy == 36)
                        index = 17;
                    if (PublicDataClass.DataTy == 38)
                        index = 17;
                    if (PublicDataClass.DataTy == 42)
                        index = 16;
                    if ((PublicDataClass.DataTy == 50) || (PublicDataClass.DataTy == 51) || (PublicDataClass.DataTy == 52)
                                                       || (PublicDataClass.DataTy == 53) || (PublicDataClass.DataTy == 54))
                        index = 15;
                    if ((PublicDataClass.DataTy == 56) || (PublicDataClass.DataTy == 58))
                        index = 22;
                    if (PublicDataClass.DataTy == 41)
                        index = 19;
                    if (PublicDataClass.DataTy == 37)
                        index = 17;
                    if (PublicDataClass.DataTy == 39)
                        index = 17;
                    if (PublicDataClass.DataTy == 43)
                        index = 16;
                    for (int i = 0; i < PublicDataClass._NetStructData.NetRLen; i++)
                    {

                        strB.Append(PublicDataClass._NetStructData.NetRBuffer[i].ToString("X2"));

                        if ((PublicDataClass.RevNetFrameMsg == "遥信") && (i == index))
                        {
                            if (i == 15)
                            {

                                Pos = PublicDataClass._DataField.Buffer[2];
                                Pos = Pos << 16;
                                Pos += PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);

                                if (PublicDataClass.DataTy == 53)
                                {
                                    if (PublicDataClass._NetStructData.NetRBuffer[i] == 0x01)
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：分>" + " ");
                                    else
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：合>" + " ");
                                }
                                else
                                {
                                    if (PublicDataClass._NetStructData.NetRBuffer[i] == 0x00)
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：分>" + " ");
                                    else
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：合>" + " ");
                                }
                            }
                            else
                            {
                                if (PublicDataClass.DataTy == 53)
                                {
                                    if (PublicDataClass._NetStructData.NetRBuffer[i] == 0x01)
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：分>" + " ");
                                    else
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：合>" + " ");
                                }
                                else
                                {
                                    if (PublicDataClass._NetStructData.NetRBuffer[i] == 0x00)
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：分>" + " ");
                                    else
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：合>" + " ");
                                }

                            }
                            index += 1;
                            Pos++;
                        }
                        else if ((PublicDataClass.RevNetFrameMsg == "遥测") && (i == index))
                        {

                            //if (i == 19)
                            //{
                            if ((PublicDataClass.DataTy == 35) || (PublicDataClass.DataTy == 40))  //浮点型
                            {

                                Pos = PublicDataClass._DataField.Buffer[2];
                                Pos = Pos << 16;
                                Pos += PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);

                                //data = PublicDataClass._DataField.Buffer[index - 12];
                                //data = data << 16;
                                //data = PublicDataClass._DataField.Buffer[index - 14] + (PublicDataClass._DataField.Buffer[index - 13] << 8);
                                //if (data > 0x8000)
                                //    data = data - 65536; 
                                //Msg += "<" + String.Format("{0:d}", Pos - 16385) + "：" + String.Format("{0:d}", data) + ">" + " ";
                                bytes[0] = PublicDataClass._DataField.Buffer[index - 16];
                                bytes[1] = PublicDataClass._DataField.Buffer[index - 15];
                                bytes[2] = PublicDataClass._DataField.Buffer[index - 14];
                                bytes[3] = PublicDataClass._DataField.Buffer[index - 13];

                                fdata = BitConverter.ToSingle(bytes, 0);
                                if ((Pos - 16385 + thei < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 + thei >= 0))
                                    PublicDataClass._Curve.ycdata[Pos - 16385 + thei] = Convert.ToDouble(fdata);
                                strB.Append("<" + String.Format("{0:d}", Pos - 16385 + thei) + "：" + String.Format("{0:f4}", fdata) + ">" + " ");
                                index += 5;
                                Pos++;
                                thei++;

                            }
                            if ((PublicDataClass.DataTy == 36) || (PublicDataClass.DataTy == 38))     //带品质描述归一化值
                            {


                                Pos = PublicDataClass._DataField.Buffer[2];
                                Pos = Pos << 16;
                                Pos += PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);


                                data = PublicDataClass._DataField.Buffer[index - 14] + (PublicDataClass._DataField.Buffer[index - 13] << 8);
                                if ((Pos - 16385 + thei < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 + thei >= 0))
                                    PublicDataClass._Curve.ycdata[Pos - 16385 + thei] = Convert.ToDouble(data);

                                strB.Append("<" + String.Format("{0:d}", Pos - 16385 + thei) + "：" + String.Format("{0:d}", data) + ">" + " ");

                                index += 3;
                                Pos++;
                                thei++;
                            }
                            if (PublicDataClass.DataTy == 42)     //不带品质描述归一化值
                            {


                                Pos = PublicDataClass._DataField.Buffer[2];
                                Pos = Pos << 16;
                                Pos += PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);


                                data = PublicDataClass._DataField.Buffer[index - 13] + (PublicDataClass._DataField.Buffer[index - 12] << 8);
                                if ((Pos - 16385 + thei < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 + thei >= 0))
                                    PublicDataClass._Curve.ycdata[Pos - 16385 + thei] = Convert.ToDouble(data);
                                strB.Append("<" + String.Format("{0:d}", Pos - 16385 + thei) + "：" + String.Format("{0:d}", data) + ">" + " ");

                                index += 2;
                                Pos++;
                                thei++;
                            }

                            //else
                            //{
                            //    data = PublicDataClass._DataField.Buffer[index - 12];
                            //    data = data << 16;
                            //    data =PublicDataClass._DataField.Buffer[index - 14] + (PublicDataClass._DataField.Buffer[index - 13] << 8);
                            //    if (data > 0x8000)
                            //        data = data - 65536;
                            //    /*bytes[0] = PublicDataClass._DataField.Buffer[index - 16];
                            //    bytes[1] = PublicDataClass._DataField.Buffer[index - 15];
                            //    bytes[2] = PublicDataClass._DataField.Buffer[index - 14];
                            //    bytes[3] = PublicDataClass._DataField.Buffer[index - 13];

                            //    fdata = BitConverter.ToSingle(bytes, 0);*/
                            //    Msg += "<" + String.Format("{0:d}", Pos - 16385) + "：" + String.Format("{0:d}", data) + ">" + " ";


                            //}

                        }
                        else if ((PublicDataClass.RevNetFrameMsg == "扰动事件") && (i == index))
                        {

                            //if (i == 19)
                            //{
                            if ((PublicDataClass.DataTy == 41))  //浮点型扰动
                            {

                                Pos = PublicDataClass._DataField.Buffer[index - 17];
                                Pos = Pos << 16;
                                Pos += PublicDataClass._DataField.Buffer[index - 19] + (PublicDataClass._DataField.Buffer[index - 18] << 8);
                                bytes[0] = PublicDataClass._DataField.Buffer[index - 16];
                                bytes[1] = PublicDataClass._DataField.Buffer[index - 15];
                                bytes[2] = PublicDataClass._DataField.Buffer[index - 14];
                                bytes[3] = PublicDataClass._DataField.Buffer[index - 13];

                                fdata = BitConverter.ToSingle(bytes, 0);
                                if ((Pos - 16385 < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 >= 0))
                                    PublicDataClass._Curve.ycdata[Pos - 16385] = Convert.ToDouble(fdata);
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：" + String.Format("{0:f4}", fdata) + ">" + " ");
                                index += 8;

                            }
                            if ((PublicDataClass.DataTy == 37) || (PublicDataClass.DataTy == 39))    //带品质描述归一化值扰动
                            {


                                Pos = PublicDataClass._DataField.Buffer[index - 15];
                                Pos = Pos << 16;
                                Pos += PublicDataClass._DataField.Buffer[index - 17] + (PublicDataClass._DataField.Buffer[index - 16] << 8);


                                data = PublicDataClass._DataField.Buffer[index - 14] + (PublicDataClass._DataField.Buffer[index - 13] << 8);
                                if ((Pos - 16385 < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 >= 0))
                                    PublicDataClass._Curve.ycdata[Pos - 16385] = Convert.ToDouble(data);
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：" + String.Format("{0:d}", data) + ">" + " ");

                                index += 6;

                            }
                            if (PublicDataClass.DataTy == 43)     //带品质描述归一化值扰动
                            {


                                Pos = PublicDataClass._DataField.Buffer[index - 14];
                                Pos = Pos << 16;
                                Pos += PublicDataClass._DataField.Buffer[index - 16] + (PublicDataClass._DataField.Buffer[index - 15] << 8);


                                data = PublicDataClass._DataField.Buffer[index - 13] + (PublicDataClass._DataField.Buffer[index - 12] << 8);
                                if (Pos - 16385 < PublicDataClass.SaveText.Cfg[0].YccfgNum)
                                    if ((Pos - 16385 < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 >= 0))
                                        strB.Append("<" + String.Format("{0:d}", Pos) + "：" + String.Format("{0:d}", data) + ">" + " ");

                                index += 5;

                            }



                        }
                        else if ((PublicDataClass.DataTy == 52) && i == index)  //不带时标遥信和遥信变位
                        {

                            Pos = PublicDataClass._DataField.Buffer[index - 13];
                            Pos = Pos << 16;
                            Pos += PublicDataClass._DataField.Buffer[index - 15] + (PublicDataClass._DataField.Buffer[index - 14] << 8);

                            if (PublicDataClass._DataField.Buffer[index - 12] == 0)
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：分>" + " ");
                            else
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：合>" + " ");
                            index += 4;

                        }
                        else if ((PublicDataClass.DataTy == 54) && i == index)  //不带时标遥信和遥信变位
                        {

                            Pos = PublicDataClass._DataField.Buffer[index - 13];
                            Pos = Pos << 16;
                            Pos += PublicDataClass._DataField.Buffer[index - 15] + (PublicDataClass._DataField.Buffer[index - 14] << 8);

                            if (PublicDataClass._DataField.Buffer[index - 12] == 1)
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：分>" + " ");
                            else
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：合>" + " ");
                            index += 4;

                        }
                        else if (PublicDataClass.DataTy == 56 && i == index) //带时标的遥信变位
                        {
                            Pos = PublicDataClass._DataField.Buffer[index - 20];
                            Pos = Pos << 16;
                            Pos += PublicDataClass._DataField.Buffer[index - 22] + (PublicDataClass._DataField.Buffer[index - 21] << 8);

                            if (PublicDataClass._DataField.Buffer[index - 19] == 0)
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：分>" + " ");
                            else
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：合>" + " ");

                            DataInfo = "";
                            DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - 12]);   //年
                            DataInfo += "年";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - 13]);  //月
                            DataInfo += "月";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - 14] & 0x1f);  //日+星期
                            DataInfo += "日";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - 15]);  //时
                            DataInfo += "时";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - 16]);  //分
                            DataInfo += "分";
                            int ms = (PublicDataClass._DataField.Buffer[index - 17] << 8) + PublicDataClass._DataField.Buffer[index - 18];

                            DataInfo += String.Format("{0:d}", ms);
                            DataInfo += "毫秒" + ">";
                            strB.Append(DataInfo);

                            index += 11;


                        }
                        else if (PublicDataClass.DataTy == 58 && i == index) //带时标的双点遥信变位
                        {
                            Pos = PublicDataClass._DataField.Buffer[index - 20];
                            Pos = Pos << 16;
                            Pos += PublicDataClass._DataField.Buffer[index - 22] + (PublicDataClass._DataField.Buffer[index - 21] << 8);

                            if (PublicDataClass._DataField.Buffer[index - 19] == 1)
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：分 ");
                            else
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：合 ");

                            DataInfo = "";
                            DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - 12]);   //年
                            DataInfo += "年";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - 13]);  //月
                            DataInfo += "月";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - 14] & 0x1f);  //日+星期
                            DataInfo += "日";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - 15]);  //时
                            DataInfo += "时";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - 16]);  //分
                            DataInfo += "分";
                            int ms = (PublicDataClass._DataField.Buffer[index - 17] << 8) + PublicDataClass._DataField.Buffer[index - 18];

                            DataInfo += String.Format("{0:d}", ms);
                            DataInfo += "毫秒" + ">";
                            strB.Append(DataInfo);

                            index += 11;


                        }
                        else
                            strB.Append(" ");
                    }
                }//end of   if (PublicDataClass._ProtocoltyFlag.NetProFlag = 1)//网口104显示
                if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)//网口101显示
                {
                    if (PublicDataClass.DataTy == 36)
                        index = 9 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen;
                    if (PublicDataClass.DataTy == 38)
                        index = 9 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen;
                    if ((PublicDataClass.DataTy == 40) || (PublicDataClass.DataTy == 35))
                        index = 11 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen;
                    if (PublicDataClass.DataTy == 42)
                        index = 8 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen;

                    if (PublicDataClass.DataTy == 37)
                        index = 9 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen;
                    if (PublicDataClass.DataTy == 39)
                        index = 9 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen;
                    if (PublicDataClass.DataTy == 41)
                        index = 11 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen;
                    if (PublicDataClass.DataTy == 43)
                        index = 8 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen;

                    if ((PublicDataClass.DataTy == 50) || (PublicDataClass.DataTy == 51) || (PublicDataClass.DataTy == 52)  //单点遥信
                                                     || (PublicDataClass.DataTy == 53) || (PublicDataClass.DataTy == 54)) //双点遥信
                        index = 7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen;
                    if ((PublicDataClass.DataTy == 56) || (PublicDataClass.DataTy == 58))
                        index = 14 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen;

                    for (int i = 0; i < PublicDataClass._NetStructData.NetRLen; i++)
                    {
                        strB.Append(PublicDataClass._NetStructData.NetRBuffer[i].ToString("x2"));

                        if ((PublicDataClass.RevNetFrameMsg == "遥信") && (i == index) && (i < PublicDataClass._NetStructData.NetRLen - 2))
                        {
                            if ((PublicDataClass.inflen == 2) && (i == 7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen))
                            {
                                Pos = PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);

                                if (PublicDataClass.DataTy == 53)
                                {
                                    if (PublicDataClass._NetStructData.NetRBuffer[i] == 0x01)
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：分>" + " ");
                                    else
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：合>" + " ");
                                }
                                else
                                {
                                    if (PublicDataClass._NetStructData.NetRBuffer[i] == 0x00)
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：分>" + " ");
                                    else
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：合>" + " ");
                                }
                            }
                            else if ((PublicDataClass.inflen == 3) && (i == 7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen))
                            {
                                Pos = PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);

                                if (PublicDataClass.DataTy == 53)
                                {
                                    if (PublicDataClass._NetStructData.NetRBuffer[i] == 0x01)
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：分>" + " ");
                                    else
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：合>" + " ");
                                }
                                else
                                {
                                    if (PublicDataClass._NetStructData.NetRBuffer[i] == 0x00)
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：分>" + " ");
                                    else
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：合>" + " ");
                                }
                            }
                            else
                            {
                                if (PublicDataClass.DataTy == 53)
                                {
                                    if (PublicDataClass._NetStructData.NetRBuffer[i] == 0x01)
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：分>" + " ");
                                    else
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：合>" + " ");
                                }
                                else
                                {
                                    if (PublicDataClass._NetStructData.NetRBuffer[i] == 0x00)
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：分>" + " ");
                                    else
                                        strB.Append("<" + String.Format("{0:d}", Pos - 1) + "：合>" + " ");
                                }

                            }
                            index += 1;
                            Pos++;
                        }
                        else if ((PublicDataClass.RevNetFrameMsg == "遥测") && (i == index) && (i < PublicDataClass._NetStructData.NetRLen - 2))
                        {
                            if ((PublicDataClass.DataTy == 35) || (PublicDataClass.DataTy == 40))  //浮点型
                            {
                                Pos = PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);
                                bytes[0] = PublicDataClass._DataField.Buffer[index - (9 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)];
                                bytes[1] = PublicDataClass._DataField.Buffer[index - (8 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)];
                                bytes[2] = PublicDataClass._DataField.Buffer[index - (7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)];
                                bytes[3] = PublicDataClass._DataField.Buffer[index - (6 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)];
                                fdata = BitConverter.ToSingle(bytes, 0);
                                if ((Pos - 16385 + thei < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 + thei >= 0))
                                    PublicDataClass._Curve.ycdata[Pos - 16385 + thei] = Convert.ToDouble(fdata);
                                strB.Append("<" + String.Format("{0:d}", Pos - 16385 + thei) + "：" + String.Format("{0:f4}", fdata) + ">" + " ");
                                index += 5;
                                Pos++;
                                thei++;

                            }
                            if ((PublicDataClass.DataTy == 36) || (PublicDataClass.DataTy == 38))     //带品质描述归一化值
                            {
                                Pos = PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);
                                data = PublicDataClass._DataField.Buffer[index - (7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]
                                     + (PublicDataClass._DataField.Buffer[index - (6 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);
                                if ((Pos - 16385 + thei < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 + thei >= 0))
                                    PublicDataClass._Curve.ycdata[Pos - 16385 + thei] = Convert.ToDouble(data);

                                strB.Append("<" + String.Format("{0:d}", Pos - 16385 + thei) + "：" + String.Format("{0:d}", data) + ">" + " ");

                                index += 3;
                                Pos++;
                                thei++;
                            }
                            if (PublicDataClass.DataTy == 42)     //不带品质描述归一化值
                            {
                                Pos = PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);


                                data = PublicDataClass._DataField.Buffer[index - (6 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]
                                    + (PublicDataClass._DataField.Buffer[index - (5 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);
                                if ((Pos - 16385 + thei < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 + thei >= 0))
                                    PublicDataClass._Curve.ycdata[Pos - 16385 + thei] = Convert.ToDouble(data);
                                strB.Append("<" + String.Format("{0:d}", Pos - 16385 + thei) + "：" + String.Format("{0:d}", data) + ">" + " ");

                                index += 2;
                                Pos++;
                                thei++;
                            }

                        }
                        else if ((PublicDataClass.RevNetFrameMsg == "扰动事件") && (i == index) && (i < PublicDataClass._NetStructData.NetRLen - 2))
                        {

                            if ((PublicDataClass.DataTy == 41))  //浮点型扰动
                            {

                                Pos = PublicDataClass._DataField.Buffer[index - (11 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]
                                    + (PublicDataClass._DataField.Buffer[index - (10 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);
                                bytes[0] = PublicDataClass._DataField.Buffer[index - (9 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)];
                                bytes[1] = PublicDataClass._DataField.Buffer[index - (8 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)];
                                bytes[2] = PublicDataClass._DataField.Buffer[index - (7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)];
                                bytes[3] = PublicDataClass._DataField.Buffer[index - (6 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)];

                                fdata = BitConverter.ToSingle(bytes, 0);
                                if ((Pos - 16385 < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 >= 0))
                                    PublicDataClass._Curve.ycdata[Pos - 16385] = Convert.ToDouble(fdata);
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：" + String.Format("{0:f4}", fdata) + ">" + " ");
                                if (PublicDataClass.inflen == 2)
                                {
                                    index += 7;
                                }
                                else if (PublicDataClass.inflen == 3)
                                {
                                    index += 8;
                                }

                            }
                            if ((PublicDataClass.DataTy == 37) || (PublicDataClass.DataTy == 39))    //带品质描述归一化值扰动
                            {

                                Pos = PublicDataClass._DataField.Buffer[index - (9 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]
                                    + (PublicDataClass._DataField.Buffer[index - (8 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);


                                data = PublicDataClass._DataField.Buffer[index - (7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]
                                      + (PublicDataClass._DataField.Buffer[index - (6 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);
                                if ((Pos - 16385 < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 >= 0))
                                    PublicDataClass._Curve.ycdata[Pos - 16385] = Convert.ToDouble(data);
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：" + String.Format("{0:d}", data) + ">" + " ");
                                if (PublicDataClass.inflen == 2)
                                {
                                    index += 5;
                                }
                                else if (PublicDataClass.inflen == 3)
                                {
                                    index += 6;
                                }

                            }
                            if (PublicDataClass.DataTy == 43)     //不带品质描述归一化值扰动
                            {

                                Pos = PublicDataClass._DataField.Buffer[index - (8 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]
                                    + (PublicDataClass._DataField.Buffer[index - (7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);
                                data = PublicDataClass._DataField.Buffer[index - (6 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]
                                    + (PublicDataClass._DataField.Buffer[index - (5 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);
                                if (Pos - 16385 < PublicDataClass.SaveText.Cfg[0].YccfgNum)
                                    if ((Pos - 16385 < PublicDataClass.SaveText.Cfg[0].YccfgNum) && (Pos - 16385 >= 0))
                                        strB.Append("<" + String.Format("{0:d}", Pos) + "：" + String.Format("{0:d}", data) + ">" + " ");

                                if (PublicDataClass.inflen == 2)
                                {
                                    index += 4;
                                }
                                else if (PublicDataClass.inflen == 3)
                                {
                                    index += 5;
                                }


                            }



                        }
                        else if ((PublicDataClass.DataTy == 52) && (i == index) && (i < PublicDataClass._NetStructData.NetRLen - 2))  //不带时标遥信和遥信变位
                        {
                            Pos = PublicDataClass._DataField.Buffer[index - (7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] + (PublicDataClass._DataField.Buffer[index - (6 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);

                            if (PublicDataClass._DataField.Buffer[index - (5 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] == 0)
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：分>" + " ");
                            else
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：合>" + " ");
                            if (PublicDataClass.inflen == 2)
                            {
                                index += 3;
                            }
                            else if (PublicDataClass.inflen == 3)
                            {
                                index += 4;
                            }

                        }
                        else if ((PublicDataClass.DataTy == 54) && (i == index) && (i < PublicDataClass._NetStructData.NetRLen - 2))  //不带时标遥信和遥信变位
                        {
                            Pos = PublicDataClass._DataField.Buffer[index - (7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] + (PublicDataClass._DataField.Buffer[index - (6 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);

                            if (PublicDataClass._DataField.Buffer[index - (5 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] == 1)
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：分>" + " ");
                            else
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：合>" + " ");
                            if (PublicDataClass.inflen == 2)
                            {
                                index += 3;
                            }
                            else if (PublicDataClass.inflen == 3)
                            {
                                index += 4;
                            }

                        }
                        else if (PublicDataClass.DataTy == 56 && (i == index) && (i < PublicDataClass._NetStructData.NetRLen - 2)) //带时标的遥信变位
                        {

                            Pos = PublicDataClass._DataField.Buffer[index - (14 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] +
                                     (PublicDataClass._DataField.Buffer[index - (13 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);

                            if (PublicDataClass._DataField.Buffer[index - (12 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] == 0)
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：分 ");
                            else
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：合 ");

                            DataInfo = "";
                            DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - (5 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]);   //年
                            DataInfo += "年";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - (6 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]);  //月
                            DataInfo += "月";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - (7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] & 0x1f);  //日+星期
                            DataInfo += "日";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - (8 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]);  //时
                            DataInfo += "时";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - (9 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]);  //分
                            DataInfo += "分";
                            int ms = (PublicDataClass._DataField.Buffer[index - (10 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8)
                                      + PublicDataClass._DataField.Buffer[index - (11 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)];

                            DataInfo += String.Format("{0:d}", ms);
                            DataInfo += "毫秒" + ">";
                            strB.Append(DataInfo);
                            if (PublicDataClass.inflen == 2)
                            {
                                index += 10;
                            }
                            else if (PublicDataClass.inflen == 3)
                            {
                                index += 11;
                            }


                        }
                        else if (PublicDataClass.DataTy == 58 && (i == index) && (i < PublicDataClass._NetStructData.NetRLen - 2)) //带时标的双点遥信变位
                        {

                            Pos = PublicDataClass._DataField.Buffer[index - (14 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] +
                                     (PublicDataClass._DataField.Buffer[index - (13 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8);

                            if (PublicDataClass._DataField.Buffer[index - (12 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] == 1)
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：分 ");
                            else
                                strB.Append("<" + String.Format("{0:d}", Pos) + "：合 ");

                            DataInfo = "";
                            DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - (5 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]);   //年
                            DataInfo += "年";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - (6 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]);  //月
                            DataInfo += "月";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - (7 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] & 0x1f);  //日+星期
                            DataInfo += "日";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - (8 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]);  //时
                            DataInfo += "时";
                            DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[index - (9 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)]);  //分
                            DataInfo += "分";
                            int ms = (PublicDataClass._DataField.Buffer[index - (10 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)] << 8)
                                      + PublicDataClass._DataField.Buffer[index - (11 + PublicDataClass.linklen + PublicDataClass.cotlen + PublicDataClass.publen + PublicDataClass.inflen)];

                            DataInfo += String.Format("{0:d}", ms);
                            DataInfo += "毫秒" + ">";
                            strB.Append(DataInfo);
                            if (PublicDataClass.inflen == 2)
                            {
                                index += 10;
                            }
                            else if (PublicDataClass.inflen == 3)
                            {
                                index += 11;
                            }


                        }
                        else
                            strB.Append(" ");
                    }

                }//end of if (PublicDataClass._ProtocoltyFlag.NetProFlag = 2)//网口101显示

                if (PublicDataClass.RevNetFrameMsg == "无效" || PublicDataClass.RevNetFrameMsg == "参数设置（否认）" ||
                    PublicDataClass.RevNetFrameMsg == "复位否认" || PublicDataClass.RevNetFrameMsg.IndexOf("错误") >= 0)     //非法帧
                {
                    strB.Append("\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.LogError(Msg);
                }
                //else if (PublicDataClass.RevNetFrameMsg == "应答" || PublicDataClass.RevNetFrameMsg == "时间"
                //     || PublicDataClass.RevNetFrameMsg == "总召激活" || PublicDataClass.RevNetFrameMsg == "总召结束"
                //     || PublicDataClass.RevNetFrameMsg == "参数设置（确认）" || PublicDataClass.RevNetFrameMsg == "版本号"
                //     || PublicDataClass.RevNetFrameMsg == "参数读取" || PublicDataClass.RevNetFrameMsg == "升级(应答)"
                //     || PublicDataClass.RevNetFrameMsg == "校验(应答)" || PublicDataClass.RevNetFrameMsg == "器件状态")     //
                //{


                else if (PublicDataClass.RevNetFrameMsg == "时间")
                {
                    DataInfo = "";
                    DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 1]);   //年
                    DataInfo += "年";
                    DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 2]);  //月
                    DataInfo += "月";
                    DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 3] & 0x1f);  //日+星期
                    DataInfo += "日";

                    DataInfo += "[星期" + String.Format("{0:d}", (PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 3] & 0xe0) >> 5) + "]";

                    DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 4]);  //时
                    DataInfo += "时";
                    DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 5]);  //分
                    DataInfo += "分";
                    int ms = (PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 6] << 8) +
                          PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 7];

                    DataInfo += String.Format("{0:d}", ms);
                    DataInfo += "毫秒";
                    strB.Append("<" + DataInfo + ">" + "\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.AckMessage(Msg);
                }


                else if (PublicDataClass.RevNetFrameMsg == "升级(应答)")
                {
                    DataInfo = "";
                    //if (PublicDataClass.addselect == 1 || PublicDataClass.addselect == 2 || PublicDataClass.addselect == 3 || PublicDataClass.addselect == 4)
                    //    DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[14] + (PublicDataClass._DataField.Buffer[15] >> 8));
                    //else
                    DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8));
                    strB.Append("<第" + DataInfo + "段>" + "\r\n");
                    PublicDataClass._Message.CodeUpdateDoing = true;
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.AckMessage(Msg);
                }

                else if (PublicDataClass.RevNetFrameMsg == "校验(应答)")
                {
                    /* if(PublicDataClass._DataField.Buffer[0] ==0x55)
                         Msg += "<校验正确>" + "\r\n";
                     else
                         Msg += "<校验错误>" + "\r\n";*/
                    strB.Append("\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.AckMessage(Msg);
                }
                else if (PublicDataClass.RevNetFrameMsg == "参数设置（确认）")
                {
                    strB.Append("\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.AckMessage(Msg);
                }
                //else 
                //    Msg += "\r\n";


                //richTextBoxExtended1.AckMessage(Msg);

            //}
                else if (PublicDataClass.RevNetFrameMsg == "遥信")
                {


                    strB.Append("\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.ThreeYMessage(Msg);
                    PublicDataClass._Message.RealTimeDataDocmentView = true;
                }
                else if (PublicDataClass.RevNetFrameMsg == "遥测")     //
                {

                    strB.Append("\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.ThreeYMessage(Msg);
                    PublicDataClass._Message.RealTimeDataDocmentView = true;
                }
                else if (PublicDataClass.RevNetFrameMsg == "历史数据" || PublicDataClass.RevNetFrameMsg == "历史记录")
                {
                    strB.Append("\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.ThreeYMessage(Msg);
                }
                else if (PublicDataClass.RevNetFrameMsg == "扰动事件" || PublicDataClass.RevNetFrameMsg == "变位事件" ||
                         PublicDataClass.RevNetFrameMsg == "越限" || PublicDataClass.RevNetFrameMsg == "停电事件" ||
                         PublicDataClass.RevNetFrameMsg == "故障事件")     //
                {
                    strB.Append("\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.LogWarning(Msg);
                    PublicDataClass._Message.RealTimeDataDocmentView = true;
                }
                else if (PublicDataClass.RevNetFrameMsg == "选择应答" || PublicDataClass.RevNetFrameMsg == "执行成功")     //
                {
                    strB.Append("\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.YkAckMessage(Msg);
                }
                else if (PublicDataClass.RevNetFrameMsg == "遥控撤销(确认)")     //
                {
                    strB.Append("\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.YkStopMessage(Msg);
                }
                else
                {
                    strB.Append("\r\n");
                    Msg = String.Empty;
                    Msg = strB.ToString();
                    richTextBoxExtended1.AckMessage(Msg);
                }

                PublicDataClass._Message.NetShowRecvTextData = false;
            }
            else if (PublicDataClass._Message.GprsShowSendTextData == true)
            {
                PublicDataClass._Message.GprsShowSendTextData = false;
                Msg = @"";
                Msg += DateTime.Now.ToString() + " ";
                Msg += "[" + PublicDataClass.ClientInfo + "]" + "Send[无线]：" + "(" + PublicDataClass.GprsFrameMsg + ")";

                for (int i = 0; i < PublicDataClass._GprsStructData.GprsTLen; i++)
                {
                    Msg += String.Format("{0:x2}", PublicDataClass._GprsStructData.TBuffer[i]);
                    Msg += " ";
                }
                Msg += "\r\n";
                richTextBoxExtended1.LogMessage(Msg);
                //PublicDataClass._Message.NetShowSendOver = false;


            }
            else if (PublicDataClass._Message.GprsShowRecvTextData == true)
            {
                PublicDataClass._Message.GprsShowRecvTextData = false;
                Msg = @"";
                Msg += DateTime.Now.ToString() + " ";
                Msg += "[" + PublicDataClass.ClientInfo + "]" + "Recv[无线]：" + "(" + PublicDataClass.GprsFrameMsg + ")";

                for (int i = 0; i < PublicDataClass._GprsStructData.GprsRLen; i++)
                {
                    Msg += String.Format("{0:x2}", PublicDataClass._GprsStructData.RBuffer[i]);
                    Msg += " ";
                }
                PublicDataClass._GprsStructData.GprsRLen = 0;
                if (PublicDataClass.GprsFrameMsg == "升级(应答)")
                {


                    DataInfo = "";
                    if (PublicDataClass.addselect == 1 || PublicDataClass.addselect == 2 || PublicDataClass.addselect == 3 || PublicDataClass.addselect == 4)
                        DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[14] + (PublicDataClass._DataField.Buffer[15] >> 8));
                    else
                        DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] >> 8));
                    Msg += "<第" + DataInfo + "段>" + "\r\n";
                    richTextBoxExtended1.AckMessage(Msg);
                    PublicDataClass._Message.CodeUpdateDoing = true;

                }
                else if (PublicDataClass.GprsFrameMsg == "校验(应答)")
                {
                    if (PublicDataClass._DataField.Buffer[0] == 0x55)
                        Msg += "<校验正确>" + "\r\n";
                    else
                        Msg += "<校验错误>" + "\r\n";
                    richTextBoxExtended1.AckMessage(Msg);
                }
                else if (PublicDataClass.GprsFrameMsg == "时间")
                {
                    DataInfo = "";
                    DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 1]);   //年
                    DataInfo += "年";
                    DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 2]);  //月
                    DataInfo += "月";
                    DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 3] & 0x1f);  //日+星期
                    DataInfo += "日";
                    DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 4]);  //时
                    DataInfo += "时";
                    DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 5]);  //分
                    DataInfo += "分";
                    int ms = (PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 6] << 8) +
                          PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 7];

                    DataInfo += String.Format("{0:d}", ms);
                    DataInfo += "毫秒";
                    Msg += "<" + DataInfo + ">" + "\r\n";
                    richTextBoxExtended1.AckMessage(Msg);
                }
                else if (PublicDataClass.GprsFrameMsg == "版本号")
                {
                    Msg += "\r\n";
                    richTextBoxExtended1.AckMessage(Msg);

                }

            }

            else if (PublicDataClass._Message.UsbShowSendTextData == true)
            {
                PublicDataClass._Message.UsbShowSendTextData = false;
                Msg = @"";
                Msg += DateTime.Now.ToString() + " ";
                Msg += "[" + PublicDataClass.ClientInfo + "]" + "Send[无线]：" + "(" + PublicDataClass.UsbFrameMsg + ")";

                for (int i = 0; i < PublicDataClass._UsbStructData.UsbTLen; i++)
                {
                    Msg += String.Format("{0:x2}", PublicDataClass._UsbStructData.TBuffer[i]);
                    Msg += " ";
                }
                Msg += "\r\n";
                richTextBoxExtended1.LogMessage(Msg);
                //PublicDataClass._Message.NetShowSendOver = false;


            }

            else if (PublicDataClass._Message.UsbShowRecvTextData == true)
            {
                PublicDataClass._Message.UsbShowRecvTextData = false;
                Msg = @"";
                Msg += DateTime.Now.ToString() + " ";
                Msg += "[" + PublicDataClass.ClientInfo + "]" + "Recv[无线]：" + "(" + PublicDataClass.UsbFrameMsg + ")";

                for (int i = 0; i < PublicDataClass._UsbStructData.UsbRLen; i++)
                {
                    Msg += String.Format("{0:x2}", PublicDataClass._UsbStructData.RBuffer[i]);
                    Msg += " ";
                }
                PublicDataClass._UsbStructData.UsbRLen = 0;
                ///////// //临时测试用////////////////////////////////////////////
                //USB收到数据解包目前为0，无效
                richTextBoxExtended1.AckMessage(Msg);
                ////////////////////////////////////////////////////////////////////////////////////////
                if (PublicDataClass.UsbFrameMsg == "升级(应答)")
                {


                    DataInfo = "";
                    if (PublicDataClass.addselect == 1 || PublicDataClass.addselect == 2 || PublicDataClass.addselect == 3 || PublicDataClass.addselect == 4)
                        DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[14] + (PublicDataClass._DataField.Buffer[15] >> 8));
                    else
                        DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] >> 8));
                    Msg += "<第" + DataInfo + "段>" + "\r\n";
                    richTextBoxExtended1.AckMessage(Msg);
                    PublicDataClass._Message.CodeUpdateDoing = true;

                }
                else if (PublicDataClass.UsbFrameMsg == "校验(应答)")
                {
                    if (PublicDataClass._DataField.Buffer[0] == 0x55)
                        Msg += "<校验正确>" + "\r\n";
                    else
                        Msg += "<校验错误>" + "\r\n";
                    richTextBoxExtended1.AckMessage(Msg);
                }
                else if (PublicDataClass.UsbFrameMsg == "时间")
                {
                    DataInfo = "";
                    DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 1]);   //年
                    DataInfo += "年";
                    DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 2]);  //月
                    DataInfo += "月";
                    DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 3] & 0x1f);  //日+星期
                    DataInfo += "日";
                    DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 4]);  //时
                    DataInfo += "时";
                    DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 5]);  //分
                    DataInfo += "分";
                    int ms = (PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 6] << 8) +
                          PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 7];

                    DataInfo += String.Format("{0:d}", ms);
                    DataInfo += "毫秒";
                    Msg += "<" + DataInfo + ">" + "\r\n";
                    richTextBoxExtended1.AckMessage(Msg);
                }
                else if (PublicDataClass.UsbFrameMsg == "版本号")
                {
                    Msg += "\r\n";
                    richTextBoxExtended1.AckMessage(Msg);

                }

            }
            else if (PublicDataClass._Message.TcpLinkState == true)
            {
                PublicDataClass._Message.TcpLinkState = false;

                strB = new StringBuilder();
                strB.Append(DateTime.Now.ToString() + " ");
                //if (PublicDataClass.TcpLinkType == 0)
                //    Msg += "(信息)" + "No Link to Remote Host [" + PublicDataClass.IPADDR + "：" + PublicDataClass.PORT + "]";

                if (PublicDataClass.TcpLinkType == 1)
                    strB.Append("(信息)" + "No Link to Remote Host [" + PublicDataClass.IPADDR + "：" + PublicDataClass.PORT + "]");
                if (PublicDataClass.TcpLinkType == 2)
                    strB.Append("(信息)" + "Link to Remote Host [" + PublicDataClass.IPADDR + "：" + PublicDataClass.PORT + "]");
                strB.Append("\r\n");

                Msg = String.Empty;
                Msg = strB.ToString();
                richTextBoxExtended1.LinkMessage(Msg);
                if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)//网口走104
                    PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F = true;

            }
            else if (PublicDataClass._Message.GprsAcceptState == true)
            {
                PublicDataClass._Message.GprsAcceptState = false;
                strB = new StringBuilder();
                strB.Append(DateTime.Now.ToString() + " ");
                strB.Append("(信息--GPRS)" + "Accept to Remote Client [" + PublicDataClass.ClientInfo + "]");
                strB.Append("\r\n");

                Msg = String.Empty;
                Msg = strB.ToString();
                richTextBoxExtended1.LinkMessage(Msg);


            }
            else if (PublicDataClass._Message.NetShowDelayTimeMsg == true)  //超时信息
            {
                PublicDataClass._Message.NetShowDelayTimeMsg = false;
                strB = new StringBuilder();
                strB.Append(DateTime.Now.ToString() + " " + "(信息)" + "设备响应超时");
                strB.Append("\r\n");

                Msg = String.Empty;
                Msg = strB.ToString();
                richTextBoxExtended1.LogError(Msg);

            }
            initmin = DateTime.Now.Minute;
            if (initmin != mymin)
            {
                mymin = initmin;
                if (mymin == 1)
                    richTextBoxExtended1.ClearText();
            }
        }

        /// <summary>
        /// 窗体初始化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutputWindows_Load(object sender, EventArgs e)
        {
            initmin = DateTime.Now.Minute;
            mymin = initmin;
            checkBox1.Checked = true;
        }

        private void OutputWindows_VisibleChanged(object sender, EventArgs e)
        {

        }

        private void richTextBoxExtended1_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {

                timer2.Interval = Convert.ToInt16(textBox1.Text) * 1000*60;
                timer2.Enabled = true;
            }
            else
            {

                timer2.Enabled = false;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            richTextBoxExtended1.ClearText();
        }

    }
}
