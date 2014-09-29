using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FaTestSoft.CommonData;

namespace FaTestSoft
{
    namespace PROTOCOL104
    {
        class protocolty104
        {   
            public static int EncodeFrame(byte dataty)
            {
                PublicDataClass.OutPutMessage.S_length = @"";

                PublicDataClass.OutPutMessage.S_txseq = @"";

                PublicDataClass.OutPutMessage.S_rxseq = @"";

                PublicDataClass.OutPutMessage.S_TypeID = @"";

                PublicDataClass.OutPutMessage.S_COT = @"";

                PublicDataClass.OutPutMessage.S_VSQ = @"";
                PublicDataClass.OutPutMessage.S_type = 0;
                int Leng = 0;
                byte TypeID = 0;  //类型标识符
                ushort COT = 0; //传送原因
                byte COI = 0;   //可变结构限定词

                if (dataty == 1)     //启动传输
                {
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x68;  //起始字节
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x04;  //长度
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x07;  // 控制域第一字节00000111---STARTDTact=1；U格式报文
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                    PublicDataClass.OutPutMessage.S_type = 1;

                }
                else if (dataty == 2)  //启动确认，一般从站发送
                {
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x68;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x04;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x0B; // 控制域第一字节00001011---STARTDTcon=1；U格式报文
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                }
                else if (dataty == 3)  //停止命令
                {
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x68;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x04;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x13; // 控制域第一字节00010011---STOPDTact=1；U格式报文
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                    PublicDataClass.OutPutMessage.S_type = 1;
                }
                else if (dataty == 4)  //停止确认 一般从站发送
                {
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x68;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x04;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x23; // 控制域第一字节00100011---STOPDTcon=1；U格式报文
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                }
                else if (dataty == 5)  //测试命令
                {
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x68;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x04;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x43; // 控制域第一字节01000011---TESTFRact=1；U格式报文
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                    PublicDataClass.OutPutMessage.S_type = 1;
                }
                else if (dataty == 6)  //测试确认
                {
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x68;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x04;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x83; // 控制域第一字节10000011---TESTFRcon=1；U格式报文
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                }
                else if (dataty == 7)  //S格式确认帧
                {
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x68;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x04;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x01; // 控制域第一字节00000001---S格式报文
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)(PublicDataClass.RxSeq & 0x00ff);  //应该为Rxseq
                    PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)((PublicDataClass.RxSeq & 0xff00) >> 8);
                    PublicDataClass._FrameTime.W = 0;
                    PublicDataClass.OutPutMessage.S_type = 3;
                    PublicDataClass.OutPutMessage.S_rxseq = String.Format("{0:d}", (PublicDataClass._NetStructData.NetTBuffer[5] << 8) + PublicDataClass._NetStructData.NetTBuffer[4]);

              
                }
               //==================在控制方向的系统命令===============================
                else if (dataty == 10)  //总召唤
                {
                    TypeID = 100;
                    COT = 6;
                    COI = 0x14;
                    
                }
                else if (dataty == 11)  //读命令
                {
                    TypeID = 102;
                    COT = 6;
                    COI = 0x14;

                }
                else if (dataty == 12)  //时钟同步命令
                {
                    TypeID = 103;
                    COT = 6;
                }
                else if (dataty == 13)  //复位进程命令
                {
                    TypeID = 105;
                    COT = 6;
                }
                else if (dataty == 14)  //带时标CP56Time2a的测试命令       
                {
                    TypeID = 107;
                    COT = 6;
                }

                
             //===============================在控制方向的过程信息==================================
                else if (dataty == 15)  //不带时标的单点命令      
                {
                    TypeID = 45;
                    COT = 6;
                }
                else if (dataty == 16)  //不带时标的单点命令撤销      
                {
                    TypeID = 45;
                    COT = 8;
                }
                else if (dataty == 17)  //不带时标的双点命令      
                {
                    TypeID = 46;
                    COT = 6;
                }
                else if (dataty == 18)  //不带时标的双点命令 撤销     
                {
                    TypeID = 46;    
                    COT = 8;
                }
                else if (dataty == 19)  //不带时标的升降命令
                {
                    TypeID = 47;
                    COT = 6;
                }
                else if (dataty == 20)  //设定命令，归一化值      
                {
                    TypeID = 48;
                    COT = 6;
                }
                else if (dataty ==51)  //撤销命令，归一化值      
                {
                    TypeID = 48;
                    COT = 8;
                }
                else if (dataty == 21)  //设定命令，标度化值      
                {
                    TypeID = 49;
                    COT = 6;
                }
                else if (dataty == 52)  //撤销命令，标度化值      
                {
                    TypeID = 49;
                    COT = 8;
                }
                else if (dataty == 22)  //设定命令，短浮点值      
                {
                    TypeID = 50;
                    COT = 6;
                }
                else if (dataty == 53)  //撤销命令，短浮点值      
                {
                    TypeID = 50;
                    COT = 8;
                }
                else if (dataty == 23)  //带56时标的单点命令      
                {
                    TypeID = 58;
                    COT = 6;
                }
                else if (dataty == 24)  //带56时标的单点命令      
                {
                    TypeID = 58;
                    COT = 8;
                }
                else if (dataty == 25)  //带56时标的双点命令      
                {
                    TypeID = 59;
                    COT = 6;
                }
                else if (dataty == 26)  //带56时标的双点命令      
                {
                    TypeID = 59;
                    COT = 8;
                }
                else if (dataty == 27)  //带56时标的升降命令      
                {
                    TypeID = 60;
                    COT = 6;
                }
                else if (dataty == 28)  //设定命令，归一化值 带56时标     
                {
                    TypeID = 61;
                    COT = 6;
                }
                else if (dataty == 29)  //设定命令，标度化值带56时标      
                {
                    TypeID = 62;
                    COT = 6;
                }
                else if (dataty == 30)  //设定命令，短浮点值带56时标      
                {
                    TypeID = 63;
                    COT = 6;
                }
                //=============自定义===========================================
                else if (dataty == 31)  //设置参数命令      
                {
                    TypeID = 110;
                    COT = 6;
                }
                else if (dataty == 32)  //读取设置参数命令      
                {
                    TypeID = 137;
                    COT = 6;
                }
                else if (dataty == 33)  //读版本号   
                {
                    //TypeID = 144;
                    TypeID = 200;
                    COT = 6;
                }
                else if (dataty == 34)  //读时间  
                {
                    TypeID = 143;
                    COT = 6;
                }
                else if (dataty == 35)  //读历史数据  ？？？
                {
                    TypeID = 102;
                    COT = 6;
                }
                else if (dataty == 36)  //读历史记录数据  
                {
                    TypeID = 145;
                    COT = 6;
                }
                else if (dataty == 37)  //读器件状态
                {
                    TypeID = 236;
                    COT = 6;
                }
                else if (dataty == 38)  //单独招遥测
                {
                    TypeID = 245;
                    COT = 6;
                }
                else if (dataty == 39)  //单独招遥信
                {
                    TypeID = 141;
                    COT = 6;
                }
                else if (dataty == 40)  //数据转发
                {
                    TypeID = 250;
                    COT = 6;
                }
                //else if (dataty == 41)  //升级文本校验
                //{
                //    TypeID = 251;
                //    COT = 6;
                //}
                //else if (dataty == 42)  //终端升级代码
                //{
                //    TypeID = 252;
                //    COT = 6;
                //}
                //else if (dataty == 43)  //ARM升级，包括文本传输、校验、代码升级
                //{
                //    TypeID = 253;
                //    COT = 6;
                //}
                //else if (dataty == 44)  //4UDSP升级，包括文本传输、校验、代码升级
                //{
                //    TypeID = 254;
                //    COT = 6;
                //}

                //else if (dataty == 45)  //软压板参数下载
                //{
                //    TypeID = 180;
                //    COT = 6;
                //}
                //else if (dataty == 46)  //软压板参数读取
                //{
                //    TypeID = 181;
                //    COT = 6;
                //}
                else if (dataty == 47)  //复归
                {
                    TypeID = 146;
                    COT = 6;
                }
                else if (dataty == 50)  //自定义转104
                {
                    TypeID = 200;
                    COT = 6;
                }
                else if (dataty == 61)  //召唤继电保护定值
                {
                    TypeID = 108;
                    COT = 5;
                }
                else if (dataty == 62)  //下装继电保护定值
                {
                    TypeID =115;
                    COT = 6;
                }
                else if (dataty == 63)  //执行激活
                {
                    TypeID = 116;
                    COT = 6;
                }
                else if (dataty == 64)  //撤销激活定值
                {
                    TypeID = 116;
                    COT = 8;
                }

                if (dataty < 10)
                    goto END;
                else
                {

                   

                    PublicDataClass._NetStructData.NetTBuffer[0] = 0x68;
                    PublicDataClass._NetStructData.NetTBuffer[2] = (byte)(PublicDataClass.TxSeq & 0x00ff);
                    PublicDataClass._NetStructData.NetTBuffer[3] = (byte)((PublicDataClass.TxSeq & 0xff00) >> 8);
                    PublicDataClass._NetStructData.NetTBuffer[4] = (byte)(PublicDataClass.RxSeq & 0x00ff);
                    PublicDataClass._NetStructData.NetTBuffer[5] = (byte)((PublicDataClass.RxSeq & 0xff00) >> 8);
                    PublicDataClass._NetStructData.NetTBuffer[6] = TypeID;
                    PublicDataClass._NetStructData.NetTBuffer[7] = (byte)(PublicDataClass._DataField.FieldVSQ);
                    PublicDataClass._NetStructData.NetTBuffer[8] = (byte)(COT & 0x00ff);
                    PublicDataClass._NetStructData.NetTBuffer[9] = (byte)((COT & 0xff00) >> 8);
                    PublicDataClass._NetStructData.NetTBuffer[10] = (byte)((PublicDataClass.DevAddr) & 0x00ff);
                    PublicDataClass._NetStructData.NetTBuffer[11] = (byte)(((PublicDataClass.DevAddr) & 0xff00) >> 8);

                     PublicDataClass.OutPutMessage.S_type = 2;

                    PublicDataClass.OutPutMessage.S_txseq = String.Format("{0:d}",( PublicDataClass._NetStructData.NetTBuffer[3] << 8) + PublicDataClass._NetStructData.NetTBuffer[2]);

                    PublicDataClass.OutPutMessage.S_rxseq = String.Format("{0:d}", (PublicDataClass._NetStructData.NetTBuffer[5] << 8)+ PublicDataClass._NetStructData.NetTBuffer[4]);

                    PublicDataClass.OutPutMessage.S_TypeID = String.Format("{0:d}",PublicDataClass._NetStructData.NetTBuffer[6] );

                    PublicDataClass.OutPutMessage.S_COT = String.Format("{0:d}", (PublicDataClass._NetStructData.NetTBuffer[9] << 8) + PublicDataClass._NetStructData.NetTBuffer[8]);

                    PublicDataClass.OutPutMessage.S_VSQ = String.Format("{0:d}", PublicDataClass._NetStructData.NetTBuffer[7] );

                    PublicDataClass._FrameTime.W=0;
                    Leng = 12;
                    switch (TypeID)
                    { 
                        case 100:        //总召唤
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x14;
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        case 103:   //对时
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen;i++ )
                                PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];

                                PublicDataClass.TxSeq += 2;
                                if (PublicDataClass.TxSeq >= 0x7fff)
                                    PublicDataClass.TxSeq = 0;
                            break;
                        case 105:   //复位进程
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x01;
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        case 107:   //带时标测试命令
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)(((PublicDataClass.ParamInfoAddr) & 0xff00) >> 8);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        case 45:   //不带时标单点控制
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)((PublicDataClass.YkStartPos) & 0x00ff);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)(((PublicDataClass.YkStartPos) & 0xff00) >> 8);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;

                        case 46:   //不带时标双点控制
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)((PublicDataClass.YkStartPos) & 0x00ff);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)(((PublicDataClass.YkStartPos) & 0xff00) >> 8);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        case 58:   //带时标单点控制
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)((PublicDataClass.YkStartPos) & 0x00ff);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)(((PublicDataClass.YkStartPos) & 0xff00) >> 8);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;

                        case 59:   //带时标双点控制
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)((PublicDataClass.YkStartPos) & 0x00ff);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)(((PublicDataClass.YkStartPos) & 0xff00) >> 8);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        //================以下为自定义类型=========================================
                        case 110:   //设置参数
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)(((PublicDataClass.ParamInfoAddr) & 0xff00) >> 8);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        case 137:   //读参数
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)(((PublicDataClass.ParamInfoAddr) & 0xff00) >> 8);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        case 144:        //读版本号
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x14;
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                     
                        
                      
                         
                        case 143:        //读时间
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x14;
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        case 102:        //历史数据
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)(((PublicDataClass.ParamInfoAddr) & 0xff00) >> 8);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        case 145:        //历史记录
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)(((PublicDataClass.ParamInfoAddr) & 0xff00) >> 8);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        case 236:        //器件状态
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x14;

                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        case 141:        //单独招遥信
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x14;

                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        case 245:        //单独招遥测
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x14;

                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        case 250:        //数据转发
                            //PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            //PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            //PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)(((PublicDataClass.ParamInfoAddr) & 0xff00) >> 8);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        //case 251:        //升级文本校验
                        //    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                        //    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                        //    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                        //    for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                        //        PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                        //    PublicDataClass.TxSeq += 2;
                        //    if (PublicDataClass.TxSeq >= 0x7fff)
                        //        PublicDataClass.TxSeq = 0;
                        //    break;
                        //case 252:        //终端升级
                        //    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                        //    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                        //    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                        //    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x14;

                        //    PublicDataClass.TxSeq += 2;
                        //    if (PublicDataClass.TxSeq >= 0x7fff)
                        //        PublicDataClass.TxSeq = 0;
                        //    break;
                        //case 253:        //ARM升级（包括文本下载、校验、升级）
                        //    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                        //    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                        //    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                        //    for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                        //        PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                        //    PublicDataClass.TxSeq += 2;
                        //    if (PublicDataClass.TxSeq >= 0x7fff)
                        //        PublicDataClass.TxSeq = 0;
                        //    break;
                        //case 254:         //4UDSP升级（包括文本下载、校验、升级）
                        //    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                        //    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                        //    PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                        //    for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                        //        PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                        //    PublicDataClass.TxSeq += 2;
                        //    if (PublicDataClass.TxSeq >= 0x7fff)
                        //        PublicDataClass.TxSeq = 0;
                        //    break;
                        //case 180:         //软压板下载
                        //    for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                        //        PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                        //    PublicDataClass.TxSeq += 2;
                        //    if (PublicDataClass.TxSeq >= 0x7fff)
                        //        PublicDataClass.TxSeq = 0;
                        //    break;
                        //case 181:         //软压板下载
                        //    for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                        //        PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                        //    PublicDataClass.TxSeq += 2;
                        //    if (PublicDataClass.TxSeq >= 0x7fff)
                        //        PublicDataClass.TxSeq = 0;
                        //    break;
                        case 146:         //复归
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)(((PublicDataClass.ParamInfoAddr) & 0xff00) >> 8);
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x14;
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
              
                        case 48:    //设定命令，归一化值    
                       
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        case 49:          //设定命令，标度化值    
                     
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        case 50:          //设定命令，短浮点值 
                       
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        case 200:        //自定义转104
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = 0x00;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass.ZDYtype;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] =  PublicDataClass.addselect;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass.seqflag;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass.seq;
                            PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)(PublicDataClass._DataField.FieldVSQ & 0x00ff);;
                             PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)((PublicDataClass._DataField.FieldVSQ & 0xff00) >> 8);
                             PublicDataClass._NetStructData.NetTBuffer[Leng++] =  PublicDataClass.SQflag;
                             PublicDataClass._NetStructData.NetTBuffer[Leng++] = (byte)(PublicDataClass.ParamInfoAddr & 0x00ff);
                             PublicDataClass._NetStructData.NetTBuffer[Leng++] =  (byte)((PublicDataClass.ParamInfoAddr & 0xff00) >> 8);
                            
                            Leng = 24;
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];

                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        case 108:   //召唤继电保护定值

                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        case 115:    //下装继电保护定值
                   
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        case 116:    //激活继电保护定值

                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._NetStructData.NetTBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            PublicDataClass.TxSeq += 2;
                            if (PublicDataClass.TxSeq >= 0x7fff)
                                PublicDataClass.TxSeq = 0;
                            break;
                        default:
                            break;
                    }
                    //PublicDataClass._FrameTime.T1 = 15;
                    //PublicDataClass._NetStructData.NetTBuffer[1] = (byte)(Leng - 2); 
                    PublicDataClass._NetStructData.NetTBuffer[1] = (byte)(Leng - 2); 
                }
            END:
                return Leng;
            }
            public static byte DecodeFrame()
            {
                PublicDataClass.OutPutMessage.length = @"";

                PublicDataClass.OutPutMessage.txseq = @"";

                PublicDataClass.OutPutMessage.rxseq = @"";

                PublicDataClass.OutPutMessage.TypeID = @"";

                PublicDataClass.OutPutMessage.COT = @"";

                PublicDataClass.OutPutMessage.VSQ = @""; 
                PublicDataClass.OutPutMessage.type = 0;
                byte dataty=0;
                byte Length=0;
                int txseq=0;
                int rxseq=0;
                byte TypeID = 0;  //类型标识符
                //byte VSQ=0;
                ushort COT = 0; //传送原因
                int Infaddr=0;
                byte COI = 0;   //可变结构限定词

                byte AFN = 0;
                byte PN = 0;
                if (PublicDataClass._NetStructData.NetRLen - PublicDataClass._NetStructData.NetRBuffer[1] != 2)  //非法帧
                {
                    dataty = 0;   //非法帧
                    //dataty = 150;   //非法帧,启动字符及数据长度错误
                }
                else if ((PublicDataClass._NetStructData.NetRBuffer[0] != 0x68) || (PublicDataClass._NetStructData.NetRLen < 6))  //非法帧
                {
                    //dataty = 0;   //非法帧
                    dataty =150;   //非法帧,启动字符及数据长度错误
                }
                else if ((PublicDataClass._NetStructData.NetRBuffer[0] == 0x68) && (PublicDataClass._NetStructData.NetRLen == 6))//固定帧
                {
                    PublicDataClass.OutPutMessage.type = 1;
                    if (PublicDataClass._NetStructData.NetRBuffer[2] == 0x0b)
                    {
                        dataty = 1;  //启动确认
                        PublicDataClass.OutPutMessage.type = 1;//U帧
                    }
                    else if (PublicDataClass._NetStructData.NetRBuffer[2] == 0x83)
                    {
                        dataty = 2;  //测试确认
                        PublicDataClass.OutPutMessage.type = 1;//U帧
                    }
                    else if (PublicDataClass._NetStructData.NetRBuffer[2] == 0x23)
                    {
                        dataty = 3;  //停止确认
                        PublicDataClass.OutPutMessage.type = 1;//U帧
                    }
                    else if (PublicDataClass._NetStructData.NetRBuffer[2] == 0x01)
                    {
                        dataty = 9;  //S确认帧
                        PublicDataClass.OutPutMessage.type = 3;//S帧
                        PublicDataClass.OutPutMessage.rxseq =  String.Format("{0:d}", (PublicDataClass._NetStructData.NetRBuffer[5] << 8) + PublicDataClass._NetStructData.NetRBuffer[4]);
                    }
                    else
                        //dataty = 0;   //非法帧
                        dataty = 151;   //非法帧固定帧控制域无效

                }

                else if ((PublicDataClass._NetStructData.NetRBuffer[0] == 0x68) && (PublicDataClass._NetStructData.NetRLen >= 15))//可变帧
                {
                    PublicDataClass.OutPutMessage.type = 2;
                    Length = PublicDataClass._NetStructData.NetRBuffer[1];
                    txseq = (PublicDataClass._NetStructData.NetRBuffer[3] << 8) + PublicDataClass._NetStructData.NetRBuffer[2];
                    rxseq = (PublicDataClass._NetStructData.NetRBuffer[5] << 8) + PublicDataClass._NetStructData.NetRBuffer[4];
                    TypeID = PublicDataClass._NetStructData.NetRBuffer[6];
                    PublicDataClass._DataField.FieldVSQ = PublicDataClass._NetStructData.NetRBuffer[7] & 0x7f;
                   // COT = Convert.ToUInt16((PublicDataClass._NetStructData.NetRBuffer[9] << 8) + PublicDataClass._NetStructData.NetRBuffer[8]);
                    PublicDataClass.ParamInfoAddr = Convert.ToInt32((PublicDataClass._NetStructData.NetRBuffer[14] << 16) + (PublicDataClass._NetStructData.NetRBuffer[13] << 8) + PublicDataClass._NetStructData.NetRBuffer[12]);
                    COI = PublicDataClass._NetStructData.NetRBuffer[15];
                    PN = (byte)(PublicDataClass._NetStructData.NetRBuffer[8] & 0x40);//01000000
                    COT = Convert.ToUInt16(PublicDataClass._NetStructData.NetRBuffer[8] & 0x3f);
                    if (txseq != PublicDataClass.RxSeq)
                    {
                        //dataty = 0;   //非法帧
                        dataty = 152;   //非法帧，发送序列号错误
                        PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F = true;
                        goto END;
                    }
                    else
                    {
                        PublicDataClass.RxSeq += 2;
                        if (PublicDataClass.RxSeq >= 0x7fff)
                            PublicDataClass.RxSeq = 0;
                        PublicDataClass._FrameTime.T1_send= 12;
                        PublicDataClass._FrameTime.W++;
                        if (PublicDataClass._FrameTime.W == 8)   //确认帧的
                        {
                            PublicDataClass._FrameTime.W = 0;
                            PublicDataClass._NetTaskFlag.Do_OKTACT = true;

                        }
                    }
                    
                    switch (TypeID)
                    {
                        case 100:   //总召
                            if (COT == 7)     //总召唤激活；
                                dataty = 4;
                            else if (COT == 10)
                                dataty = 5;  //总召结束
                            else dataty = 169;  //总召传送原因错误
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        case 103:  //对时
                            for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 15; i++)
                                PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 15];
                            PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 15;
                            dataty = 6;
                       
                            break;
                        case 105:  //复位进程

                            dataty = 7;
                     
                            break;
                        case 107:  //带时标测试

                            dataty = 8;
                          
                            break;
                        case 45:           //不带时标单点遥控
                            //if (COT == 0x0007 && ((COI & 0x80) == 0x80))   //选择确认
                            //    dataty = 10;
                            //if (COT == 0x0007 && COI == 0x00)
                            //    dataty = 11;                          //执行分成功
                            //if (COT == 0x0007 && COI == 0x01)
                            //    dataty = 11;                          //执行合成功
                            //if (COT == 0x0009)
                            //    dataty = 12;                            //遥控撤销确认
                            if (PN == 0x40)
                            { dataty = 174; }
                            else
                            {
                                if (COT == 0x0007)
                                {
                                    if ((COI & 0x80) == 0x80)//选择确认
                                        dataty = 10;
                                    if (COI == 0x00)
                                        dataty = 11;                          //执行分成功
                                    if (COI == 0x01)
                                        dataty = 11;                          //执行合成功
                                }
                                else if (COT == 0x0009)
                                    dataty = 12;                            //遥控撤销确认
                                else if (COT == 0x000a)
                                    dataty = 173;                            //
                                else dataty = 153;  //不带时标单点遥控传送原因错误
                            }
                            break;
                        case 46:           //不带时标双点遥控
                            //if (COT == 0x0007 && ((COI & 0x80) == 0x80))   //选择确认
                            //    dataty = 10;
                            //if (COT == 0x0007 && COI == 0x01)
                            //    dataty = 11;                          //执行分成功
                            //if (COT == 0x0007 && COI == 0x02)
                            //    dataty = 11;                          //执行合成功
                            //if (COT == 0x0009)
                            //    dataty = 12;                            //遥控撤销确认
                            if (PN == 0x40)
                            { dataty = 174; }
                            else
                            {
                                if (COT == 0x0007)
                                {
                                    if ((COI & 0x80) == 0x80)//选择确认
                                        dataty = 13;
                                    if (COI == 0x01)
                                        dataty = 14;                          //执行分成功
                                    if (COI == 0x02)
                                        dataty = 14;                          //执行合成功
                                }
                                else if (COT == 0x0009)
                                    dataty = 15;                            //遥控撤销确认
                                else if (COT == 0x000a)
                                    dataty = 173;
                                else dataty = 154;  //不带时标双点遥控传送原因错误
                            }
                            break;
                        case 48:           //归一化遥测置数应答
                                          
                            if (COT == 0x0007)
                            {

                                dataty = 71;   //设置确认
                            }
                            else if (COT == 0x0009)
                                dataty = 72;                            //撤销确认
                            else dataty = 172;  //归一化遥测置数应答传送原因错误

                            break;
                        case 49:           //标度化遥测置数应答

                            if (COT == 0x0007)
                            {

                                dataty = 73;   //设置确认
                            }
                            else if (COT == 0x0009)
                                dataty = 74;                            //撤销确认
                            else dataty = 170;  //归一化遥测置数应答传送原因错误

                            break;
                        case 50:           //短浮点遥测置数应答

                            if (COT == 0x0007)
                            {

                                dataty = 75;   //设置确认
                            }
                            else if (COT == 0x0009)
                                dataty = 76;                            //撤销确认
                            else dataty = 171;  //归一化遥测置数应答传送原因错误

                            break;
                        case 58:           //带时标单点遥控
                            //if (COT == 0x0007 && ((COI & 0x80) == 0x80))   //选择确认
                            //    dataty = 16;
                            //if (COT == 0x0007 && COI == 0x00)
                            //    dataty = 17;                          //执行分成功
                            //if (COT == 0x0007 && COI == 0x01)
                            //    dataty = 17;                          //执行合成功
                            //if (COT == 0x0009)
                            //    dataty = 18;                            //遥控撤销确认
                            if (COT == 0x0007)
                            {
                                if ((COI & 0x80) == 0x80)//选择确认
                                    dataty = 16;
                                if (COI == 0x00)
                                    dataty = 17;                          //执行分成功
                                if (COI == 0x01)
                                    dataty = 17;                          //执行合成功
                            }
                            else if (COT == 0x0009)
                                dataty = 18;                            //遥控撤销确认
                            else dataty = 155;  //带时标单点遥控传送原因错误
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        case 59:           //带时标双点遥控
                            //if (COT == 0x0007 && ((COI & 0x80) == 0x80))   //选择确认
                            //    dataty = 19;
                            //if (COT == 0x0007 && COI == 0x01)
                            //    dataty = 20;                          //执行分成功
                            //if (COT == 0x0007 && COI == 0x02)
                            //    dataty = 20;                          //执行合成功
                            //if (COT == 0x0009)
                            //    dataty = 21;                            //遥控撤销确认
                            if (COT == 0x0007)
                            {
                                if ((COI & 0x80) == 0x80)//选择确认
                                    dataty = 19;
                                if (COI == 0x01)
                                    dataty = 20;                          //执行分成功
                                if (COI == 0x02)
                                    dataty = 20;                          //执行合成功
                            }
                            else if (COT == 0x0009)
                                dataty = 21;                            //遥控撤销确认
                            else dataty = 156;  //带时标双点遥控传送原因错误
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        case 110:               //设置参数
                            if (COI == 1)
                                dataty = 22;     //设置参数确认
                            else if (COI == 0)
                                dataty = 23;     //设置参数否认
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        case 137:              //读参数
                            PublicDataClass._DataField.FieldVSQ = PublicDataClass._NetStructData.NetRBuffer[7];
                            for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 15; i++)
                                PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 15];
                            PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 15;
                            dataty = 24;
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        case 144:               //读版本号
                            for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 15; i++)
                                PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 15];
                            PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 15;
                            dataty = 25;
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        //case 200:               //读版本号
                        //    for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 15; i++)
                        //        PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 15];
                        //    PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 15;
                        //    dataty = 105;
                        //    //PublicDataClass.RxSeq += 2;
                        //    //if (PublicDataClass.RxSeq >= 0x7fff)
                        //    //    PublicDataClass.RxSeq = 0;
                            break;
                        case 236:               //读器件状态
                            for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 15; i++)
                                PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 15];
                            PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 15;
                            dataty = 26;
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        case 102:               //历史数据
                            PublicDataClass._DataField.FieldVSQ = PublicDataClass._NetStructData.NetRBuffer[7];
                            for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 15; i++)
                                PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 15];
                            PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 15;
                            dataty = 27;
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        case 145:               //历史记录
                            PublicDataClass._DataField.FieldVSQ = PublicDataClass._NetStructData.NetRBuffer[7];
                            for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 15; i++)
                                PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 15];
                            PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 15;
                            dataty = 28;
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        case 250:               //升级文本下载
                            for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 15; i++)
                                PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 15];
                            PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 15;
                            dataty = 29;
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        //case 251:               //升级文本校验
                        //    for (int i = 0; i < 1; i++)
                        //        PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 15];
                        //    PublicDataClass._DataField.FieldLen = 1;
                        //    dataty = 30;
                        //    //PublicDataClass.RxSeq += 2;
                        //    //if (PublicDataClass.RxSeq >= 0x7fff)
                        //    //    PublicDataClass.RxSeq = 0;
                        //    break;
                        case 253:               //ARM升级
                            for (int i = 0; i < 3; i++)
                                PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 15];
                            PublicDataClass._DataField.FieldLen = 3;
                            dataty = 31;
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        //case 254:               //4UDSP升级
                        //    for (int i = 0; i < 16; i++)
                        //        PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 15];
                        //    PublicDataClass._DataField.FieldLen = 16;
                        //    dataty = 32;
                        //    //PublicDataClass.RxSeq += 2;
                        //    //if (PublicDataClass.RxSeq >= 0x7fff)
                        //    //    PublicDataClass.RxSeq = 0;
                        //    break;
                        //case 180:               //软压板参数下载回复
                        //    for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                        //        PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                        //    PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                        //    dataty = 33;
                        //    //PublicDataClass.RxSeq += 2;
                        //    //if (PublicDataClass.RxSeq >= 0x7fff)
                        //    //    PublicDataClass.RxSeq = 0;
                        //    break;
                        //case 181:               //软压板参数读取
                        //    for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                        //        PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                        //    PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                        //    dataty = 34;
                        //    //PublicDataClass.RxSeq += 2;
                        //    //if (PublicDataClass.RxSeq >= 0x7fff)
                        //    //    PublicDataClass.RxSeq = 0;
                        //    break;
                        case 245:               //自定义单独招遥测回复
                            for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                            PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                            dataty = 35;
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        case 9:               //遥测 归一化值 2+1字节
                            if (COT == 20)  //正常响应站召唤
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 36;
                            }
                            else if (COT == 3)  //突发，扰动
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 37;
                                PublicDataClass._FrameTime.LoopTime = 15;
                            }
                            else dataty = 157;  //遥测归一化值传送原因错误
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        case 11:               //遥测 标度化值  2+1字节
                            if (COT == 20)  //正常响应站召唤
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 38;
                            }
                            else if (COT == 3)  //突发，扰动
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 39;
                                PublicDataClass._FrameTime.LoopTime = 15;
                            }
                            else dataty = 158;  //遥测标度化值传送原因错误
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        case 13:               //遥测短浮点值  4+1字节
                            if (COT == 20)  //正常响应站召唤
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 40;
                            }
                            else if (COT == 3)  //突发，扰动
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 41;
                                PublicDataClass._FrameTime.LoopTime = 15;
                            }
                            else dataty = 159;  //遥测短浮点值传送原因错误
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        case 21:               //遥测 不带品质描述的归一化值 2字节
                            if (COT == 20)  //正常响应站召唤
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 42;
                            }
                            else if (COT == 3)  //突发，扰动
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 43;
                                PublicDataClass._FrameTime.LoopTime = 15;
                            }
                            else dataty = 160;  //遥测不带品质描述的归一化值传送原因错误
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        case 34:               //遥测 带56时标归一化值 2+1+7字节
                            if (COT == 20)  //正常响应站召唤
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 44;
                            }
                            else if (COT == 3)  //突发，扰动
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 45;
                                PublicDataClass._FrameTime.LoopTime = 15;
                            }
                            else dataty = 161;  //遥测带56时标归一化值传送原因错误
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        case 35:               //遥测 带56时标的标度化值  2+1+7字节
                            if (COT == 20)  //正常响应站召唤
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 46;
                            }
                            else if (COT == 3)  //突发，扰动
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 47;
                                PublicDataClass._FrameTime.LoopTime = 15;
                            }
                            else dataty = 162;  //遥测带56时标标度化值传送原因错误
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        case 36:               //遥测 带56时标的短浮点值  4+1+7字节
                            if (COT == 20)  //正常响应站召唤
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 48;
                            }
                            else if (COT == 3)  //突发，扰动
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 49;
                                PublicDataClass._FrameTime.LoopTime = 15;
                            }
                            else dataty = 163;  //遥测带56时标的短浮点值传送原因错误
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        case 141:               //自定义单独招遥信回复
                            for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                            PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                            dataty = 50;
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        case 1:               //单点遥信
                            if (COT == 20)  //正常响应站召唤
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 51;
                            }
                            else if (COT == 3)  //突发，遥信变位
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 52;
                                PublicDataClass._FrameTime.LoopTime = 15;
                            }
                            else dataty = 164;  //单点遥信传送原因错误
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        case 3:               //双点遥信
                            if (COT == 20)  //正常响应站召唤
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 53;
                            }
                            else if (COT == 3)  //突发，双点遥信变位
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 54;
                                PublicDataClass._FrameTime.LoopTime = 15;
                            }
                            else dataty = 165;  //双点遥信传送原因错误
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        case 30:               //带56时标的单点遥信
                            if (COT == 20)  //正常响应站召唤
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 55;
                            }
                            else if (COT == 3)  //突发，遥信变位
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 56;
                                PublicDataClass._FrameTime.LoopTime = 15;
                            }
                            else dataty = 166;  //带56时标的单点遥信传送原因错误
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        case 31:               //带56时标的双点遥信
                            if (COT == 20)  //正常响应站召唤
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 57;
                            }
                            else if (COT == 3)  //突发，双点遥信变位
                            {
                                for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                                dataty = 58;
                                PublicDataClass._FrameTime.LoopTime = 15;
                            }
                            else dataty = 167;  //带56时标的双点遥信传送原因错误
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;
                        //case 22:               //停电事件
                        //    for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                        //        PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                        //    PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                        //    dataty = 59;
                        //    //PublicDataClass.RxSeq += 2;
                        //    //if (PublicDataClass.RxSeq >= 0x7fff)
                        //    //    PublicDataClass.RxSeq = 0;
                        //    break;
                        //case 23:               //故障事件
                        //    for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                        //        PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                        //    PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                        //    dataty = 60;
                        //    //PublicDataClass.RxSeq += 2;
                        //    //if (PublicDataClass.RxSeq >= 0x7fff)
                        //    //    PublicDataClass.RxSeq = 0;
                        //    break;
                        case 114:               //响应召唤继电保护定值
                            PublicDataClass._DataField.FieldVSQ = PublicDataClass._NetStructData.NetRBuffer[7];
                            for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                            PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;
                            dataty = 110;
                      
                            break;
                        case 115:               //响应设定继电保护定值
                            PublicDataClass._DataField.FieldVSQ = PublicDataClass._NetStructData.NetRBuffer[7];
                            for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                            PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;

                            if (COT == 0x0007)
                            {
                                dataty = 111;   //设置确认
                            }
                            else if (COT == 0x87)
                                //否认
                                dataty = 112;
                      
                            break;
                        case 116:               //响应激活定值
                            PublicDataClass._DataField.FieldVSQ = PublicDataClass._NetStructData.NetRBuffer[7];
                            for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 12; i++)
                                PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 12];
                            PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 12;

                            if (COT == 0x0007)
                            {
                                dataty = 113;   //激活确认
                            }
                            else if (COT == 0x0009)
                                
                                dataty = 114; //撤销确认

                            break;
                        case 200:               //104转自定义
                            AFN = PublicDataClass._NetStructData.NetRBuffer[15];
                            PublicDataClass.addselect = PublicDataClass._NetStructData.NetRBuffer[16];
                           PublicDataClass.seqflag = PublicDataClass._NetStructData.NetRBuffer[17];
                           PublicDataClass.seq = PublicDataClass._NetStructData.NetRBuffer[18];
                           PublicDataClass._DataField.FieldVSQ = (PublicDataClass._NetStructData.NetRBuffer[20] << 8) + PublicDataClass._NetStructData.NetRBuffer[19];
                           PublicDataClass.SQflag = PublicDataClass._NetStructData.NetRBuffer[21];
                           PublicDataClass.ParamInfoAddr = (PublicDataClass._NetStructData.NetRBuffer[23] << 8) + PublicDataClass._NetStructData.NetRBuffer[22];
                            switch (AFN)
                            {
                                case 1:   //参数下载
                                    if (PublicDataClass._NetStructData.NetRBuffer[24] == 0x01)
                                        dataty = 100;    //确认
                                    else if (PublicDataClass._NetStructData.NetRBuffer[24] == 0x00)
                                        dataty = 101;    //否认
                                    break;

                                case 2:   //参数读取
                                    for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 24; i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 24];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 24;
                                    dataty = 102;    //

                                    break;
                                case 3:   //DSP文本升级
                                    for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 24; i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 24];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 24;
                                    dataty = 103;    //

                                    break;
                                case 4:   //历史记录
                                    for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 24; i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 24];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 24;
                                    dataty = 104;    //

                                    break;

                                case 5:                         //版本号
                                    for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 24; i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 24];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 24;
                                    dataty = 105;    //版本号
                                    break;
                                case 6:                         //时间
                                    for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 24; i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 24];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 24;
                                    dataty = 106;    //时间
                                    break;
                                case 7:                         //对时回复
                                    for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 24; i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 24];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 24;
                                    dataty = 107;    //时间
                                    break;
                                case 8:                         //器件状态
                                    for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 24; i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 24];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 24;
                                    dataty = 108;    //器件状态回复
                                    break;
                                case 9:   //ARM文本升级
                                    for (int i = 0; i < PublicDataClass._NetStructData.NetRLen - 24; i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._NetStructData.NetRBuffer[i + 24];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._NetStructData.NetRLen - 24;
                                    dataty = 109;    //

                                    break;
                            }
                           
                            //PublicDataClass.RxSeq += 2;
                            //if (PublicDataClass.RxSeq >= 0x7fff)
                            //    PublicDataClass.RxSeq = 0;
                            break;

                        default:
                            //dataty = 0;
                            dataty = 168;  //类型标识符错误
                            break;




                    }


          
                   
                }
                else
                    dataty = 0;

            END:
                PublicDataClass.OutPutMessage.length = String.Format("{0:d}", PublicDataClass._NetStructData.NetRBuffer[1]);

                PublicDataClass.OutPutMessage.txseq = String.Format("{0:d}", (PublicDataClass._NetStructData.NetRBuffer[3] << 8) + PublicDataClass._NetStructData.NetRBuffer[2]);

                PublicDataClass.OutPutMessage.rxseq = String.Format("{0:d}", (PublicDataClass._NetStructData.NetRBuffer[5] << 8) + PublicDataClass._NetStructData.NetRBuffer[4]);

                PublicDataClass.OutPutMessage.TypeID = String.Format("{0:d}", PublicDataClass._NetStructData.NetRBuffer[6]);

                PublicDataClass.OutPutMessage.COT = String.Format("{0:d}", (PublicDataClass._NetStructData.NetRBuffer[9] << 8) + PublicDataClass._NetStructData.NetRBuffer[8]);

                PublicDataClass.OutPutMessage.VSQ = String.Format("{0:d}", PublicDataClass._DataField.FieldVSQ);
                return dataty;
            }

        }
    }
}
