using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FaTestSoft
{
    namespace ProtocolParam
    {
        class protocoltyparam
        {
            public static int EncodeFrame(byte dataty, byte[] buf, byte[] DataField, int DataFieldLen, int DevAddr, byte TargetBoard, byte seqflag, byte seq, byte SQflag, int VSQ, int InfAddr)
            {
                int Leng = 0;
                buf[0] = 0x69;			//	
                buf[3] = 0x69;				//	
                buf[4] = 0x1;				//	控制域=1：主站-》从站
                buf[5] = (byte)(DevAddr & 0x00ff);                //终端地址
                buf[6] = (byte)((DevAddr & 0xff00) >> 8);
                buf[7] = dataty;				//	
                buf[8] = TargetBoard;
                buf[9] = seqflag;				//	
                buf[10] = seq;
                buf[11] = (byte)(VSQ & 0x00ff);
                buf[12] = (byte)((VSQ & 0xff00) >> 8);
                buf[13] = SQflag;
                buf[14] = (byte)(InfAddr & 0x00ff);                //终端地址
                buf[15] = (byte)((InfAddr & 0xff00) >> 8);
                Leng = 16;
                for (int i = 0; i < DataFieldLen; i++)
                    buf[Leng++] = DataField[i];

                buf[1] = (byte)((Leng - 4) & 0x00ff);
                buf[2] = (byte)(((Leng - 4) & 0xff00) >> 8);
                buf[Leng] = GetSumCheck(buf, 4, (Leng - 4));	//	校验和
                buf[Leng + 1] = 0x16;
                Leng += 2;
                
               
                return Leng;
            }

            public static byte DecodeFrame(byte[] buf, int FLen, byte[] DataField, ref int DataFieldLen, ref int DevAddr, ref byte TargetBoard, ref byte seqflag, ref byte seq, ref byte SQflag, ref int VSQ, ref int InfAddr)
            {
                //int DevAddr; 
                //byte TargetBoard;
                //byte seqflag;
                //byte seq; 
                //byte SQflag; 
                byte AFN = 0;
                byte dataty = 0;
                int Len; 

                if ((buf[3] != 0x69) || (buf[FLen - 1] != 0x16) || (FLen < 15))  //非法帧
                    dataty = 0;
                else if (GetSumCheck(buf, 4, FLen - 6) != buf[FLen - 2])
                    dataty = 0;
                else
                {
                    Len = (buf[2] << 8) + buf[1];
                    DevAddr = (buf[6] << 8) + buf[5];
                    AFN = buf[7];
                    TargetBoard = buf[8];
                    seqflag = buf[9];
                    seq = buf[10];
                    VSQ = (buf[12] << 8) + buf[11];
                    SQflag = buf[13];
                    InfAddr = (buf[15] << 8) + buf[14];
                    switch (AFN)
                    {
                        case 1:   //参数下载
                            if (buf[16] == 0x01)
                                dataty = 100;    //确认
                            else if (buf[16] == 0x00)
                                dataty = 101;    //否认
                            break;
                            
                        case 2:   //参数读取
                            for (int i = 0; i < FLen - 18; i++)
                                DataField[i] = buf[16+i];
                            DataFieldLen = FLen - 18;
                                dataty = 102;    //

                                break;
                        case 3:   //DSP文本升级
                                for (int i = 0; i < FLen - 18; i++)
                                    DataField[i] = buf[16 + i];
                                DataFieldLen = FLen - 18;
                                dataty = 103;    //

                            break;
                        case 4:   //历史记录
                            for (int i = 0; i < FLen - 18; i++)
                                DataField[i] = buf[16 + i];
                            DataFieldLen = FLen - 18;
                            dataty = 104;    //
               
                            break;
                             
                        case 5:                         //版本号
                            for (int i = 0; i < FLen - 18; i++) 
                                DataField[i] = buf[16 + i];
                            DataFieldLen = FLen - 18;
                            dataty = 105;    //版本号
                            break;
                        case 6:                         //时间
                            for (int i = 0; i < FLen - 18; i++)
                                DataField[i] = buf[16 + i];
                            DataFieldLen = FLen - 18;
                            dataty = 106;    //时间
                            break;
                        case 7:                         //对时回复
                            for (int i = 0; i < FLen - 18; i++)
                                DataField[i] = buf[16 + i];
                            DataFieldLen = FLen - 18;
                            dataty = 107;    //时间
                            break;
                        case 8:                         //器件状态
                            for (int i = 0; i < FLen - 18; i++)
                                DataField[i] = buf[16 + i];
                            DataFieldLen = FLen - 18;
                            dataty = 108;    //器件状态回复
                            break;
                        case 9:   //ARM文本升级
                            for (int i = 0; i < FLen - 18; i++)
                                DataField[i] = buf[16 + i];
                            DataFieldLen = FLen - 18;
                            dataty = 109;    //

                            break;
                    }
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
        }
    }
}
